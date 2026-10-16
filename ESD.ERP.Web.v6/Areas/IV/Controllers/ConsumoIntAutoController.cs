using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.IV.Controllers
{
   [Area("IV")]
   [Route("[controller]")]
   public class ConsumoIntAutoController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly IMovimientoAlmacenProxy movimientoAlmacenProxy;
      private readonly IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy;
      private readonly ICentroCostoProxy centroCostoProxy;
      private readonly IPersonalProxy personalProxy;
      public ConsumoIntAutoController(
         IAlmacenProxy almacenProxy,
         IMovimientoAlmacenProxy movimientoAlmacenProxy,
         IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy,
         ICentroCostoProxy centroCostoProxy,
         IPersonalProxy personalProxy
         )
      {
         this.almacenProxy = almacenProxy;
         this.movimientoAlmacenProxy = movimientoAlmacenProxy;
         this.movimientoAlmacenProductoProxy = movimientoAlmacenProductoProxy;
         this.centroCostoProxy = centroCostoProxy;
         this.personalProxy = personalProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ConsumoIntAutoViewModel consumoIntAutoViewModel = new ConsumoIntAutoViewModel();

         cargarListasCompartidas(consumoIntAutoViewModel);

         consumoIntAutoViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Edit }
         };

         return PartialView(consumoIntAutoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ConsumoIntAutoViewModel model = JsonSerializer.Deserialize<ConsumoIntAutoViewModel>(jsonModel);

         model.MovimientoAlmacenDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenDTO.OrderBy = "ma.fecha desc,ma.mov_almacen";

         model.MovimientoAlmacenDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.MovimientoAlmacenDTO.TransaccionAlmacenDTO = new TransaccionAlmacenDTO() { Codigo = TransaccionAlmacenConstante.SALIDA_CONSUMO_INTERNO };
         model.MovimientoAlmacenDTO.EstadoAprobacionDTO = new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.PENDIENTE_APROBACION };
         model.MovimientoAlmacenDTO.Estado = EstadoConstante.ACTIVO;

         try
         {
            List<MovimientoAlmacenDTO> movimientoAlmacenDTOList = movimientoAlmacenProxy.Search(model.MovimientoAlmacenDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = movimientoAlmacenDTOList.Count,
               rows = from f in movimientoAlmacenDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.MovimientoAlmacenId,
                            f.Codigo,
                            Fecha = f.Fecha.ToShortDateString(),
                            AlmacenNombre = f.AlmacenDTO.Nombre,
                            PersonalNombre = f.PersonalDTO == null ? string.Empty : f.PersonalDTO.ProveedorDTO.RazonSocial,
                            CentroCostoNombre = f.CentroCostoDTO == null ? string.Empty : f.CentroCostoDTO.Nombre,
                            EstadoMovimientoAlmacenNombre = f.EstadoMovimientoAlmacenDTO.Nombre,
                            f.EsEditable,
                            f.Estado
                         }
                      }
            };

            Logger.InfoFinish();
            return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ConsumoIntAutoViewModel model)
      {

         Logger.InfoStart();

         ConsumoIntAutoViewModel consumoIntAutoViewModel = new ConsumoIntAutoViewModel
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(consumoIntAutoViewModel, model.MovimientoAlmacenDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, consumoIntAutoViewModel);
      }
      private void cargarDataParaEditar(ConsumoIntAutoViewModel model, int id)
      {

         MovimientoAlmacenDTO movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = id });

         model.MovimientoAlmacenDTO = movimientoAlmacenDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.AlmacenList, movimientoAlmacenDTO.AlmacenId);
         WebHelper.SelectedListItemSetValue(model.CentroCostoList, movimientoAlmacenDTO.CentroCostoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="aprobar", Content="Aprobar" },
            new MenuItem() { Key="desaprobar", Content="Desaprobar" },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };
      }
      private void cargarListas(ConsumoIntAutoViewModel model)
      {
         cargarListasCompartidas(model);

         if(model.MovimientoAlmacenDTO.AlmacenId > 0)
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

            List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
            {
               EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
               NotAlmacenId = model.MovimientoAlmacenDTO.AlmacenId,
               Estado = EstadoConstante.ACTIVO
            });

            model.AlmacenDestinoList = WebHelper.ToSelectListItem(almacenDTOList, x => x.AlmacenId.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
         }
         else
            model.AlmacenDestinoList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
      }
      private void cargarListasCompartidas(ConsumoIntAutoViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO() { 
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            Estado = EstadoConstante.ACTIVO 
         });
         model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<CentroCostoDTO> centroCostoDTOList = centroCostoProxy.Search(new CentroCostoDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO
         });
         model.CentroCostoList = WebHelper.ToSelectListItem(centroCostoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("ListarMovimientoAlmacenProducto")]
      public ActionResult ListarMovimientoAlmacenProducto(string jsonModel)
      {

         Logger.InfoStart();

         ConsumoIntAutoViewModel model = JsonSerializer.Deserialize<ConsumoIntAutoViewModel>(jsonModel);

         model.MovimientoAlmacenProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenProductoDTO.OrderBy = "p.descripcion";

         try
         {
            List<MovimientoAlmacenProductoDTO> movimientoAlmacenProductoDTOList = movimientoAlmacenProductoProxy.Search(model.MovimientoAlmacenProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = movimientoAlmacenProductoDTOList.Count,
               rows = from f in movimientoAlmacenProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.MovimientoAlmacenProductoId,
                            f.MovimientoAlmacenId,
                            f.ProductoId,
                            f.ProductoDTO.Codigo,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            f.Descripcion,
                            f.CantidadDocumento,
                            f.PesoNetoDocumento,
                            f.PesoBrutoDocumento,
                            f.Cantidad,
                            f.PesoNeto,
                            f.PesoBruto,
                            f.ValorUnitario,
                            Total = f.ValorUnitario * f.Cantidad,
                            f.Estado,
                            DTOState = DTOState.Unchanged
                         }
                      }
            };

            Logger.InfoFinish();
            return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }
      }
      [HttpPost("Aprobar")]
      public ActionResult Aprobar([FromBody] ConsumoIntAutoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            PersonalDTO personalDTO = personalProxy.SingleSearch(new PersonalDTO() { 
               ProveedorDTO = new ProveedorDTO() { UsuarioId = ObtenerUsuarioId()}
            });

            model.MovimientoAlmacenDTO.PersonalAutorizaId = personalDTO.Id;

            AuditDTO(model.MovimientoAlmacenDTO);

            movimientoAlmacenProxy.Aprobar(model.MovimientoAlmacenDTO);

            resultResponse.Message = SR.GetString("IV_movimiento_almacen_autorizacion_aprobada");
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpPost("Desaprobar")]
      public ActionResult Desaprobar([FromBody] ConsumoIntAutoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            AuditDTO(model.MovimientoAlmacenDTO);

            movimientoAlmacenProxy.Desaprobar(model.MovimientoAlmacenDTO);

            resultResponse.Message = SR.GetString("IV_movimiento_almacen_autorizacion_desaprobada");
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }

   }
}