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
   public class ControlMermaAutoController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly IMovimientoAlmacenProxy movimientoAlmacenProxy;
      private readonly IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy;
      private readonly ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy;
      private readonly IPersonalProxy personalProxy;
      public ControlMermaAutoController(
         IAlmacenProxy almacenProxy,
         IMovimientoAlmacenProxy movimientoAlmacenProxy,
         IMovimientoAlmacenProductoProxy movimientoAlmacenProductoProxy,
         ITipoMovimientoAlmacenProxy tipoMovimientoAlmacenProxy,
         IPersonalProxy personalProxy
         )
      {
         this.almacenProxy = almacenProxy;
         this.movimientoAlmacenProxy = movimientoAlmacenProxy;
         this.movimientoAlmacenProductoProxy = movimientoAlmacenProductoProxy;
         this.tipoMovimientoAlmacenProxy = tipoMovimientoAlmacenProxy;
         this.personalProxy = personalProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ControlMermaAutoViewModel controlMermaAutoViewModel = new ControlMermaAutoViewModel();

         cargarListasCompartidas(controlMermaAutoViewModel);

         controlMermaAutoViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Edit }
         };

         return PartialView(controlMermaAutoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ControlMermaAutoViewModel model = JsonSerializer.Deserialize<ControlMermaAutoViewModel>(jsonModel);

         model.MovimientoAlmacenDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenDTO.OrderBy = "ma.fecha desc,ma.mov_almacen";

         model.MovimientoAlmacenDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.MovimientoAlmacenDTO.TransaccionAlmacenList = new List<string>() { TransaccionAlmacenConstante.INGRESO_MERMA, TransaccionAlmacenConstante.SALIDA_MERMA };
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
                            TransaccionAlmacenNombre = f.TransaccionAlmacenDTO.Nombre,
                            EstadoMovimientoAlmacenNombre = f.EstadoMovimientoAlmacenDTO.Nombre,
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
      public ActionResult Editar([FromBody] ControlMermaAutoViewModel model)
      {

         Logger.InfoStart();

         ControlMermaAutoViewModel controlMermaAutoViewModel = new ControlMermaAutoViewModel
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(controlMermaAutoViewModel, model.MovimientoAlmacenDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, controlMermaAutoViewModel);
      }
      private void cargarDataParaEditar(ControlMermaAutoViewModel model, int id)
      {

         MovimientoAlmacenDTO movimientoAlmacenDTO = movimientoAlmacenProxy.GetById(new MovimientoAlmacenDTO() { MovimientoAlmacenId = id });

         model.MovimientoAlmacenDTO = movimientoAlmacenDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.AlmacenList, movimientoAlmacenDTO.AlmacenId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="aprobar", Content="Aprobar" },
            new MenuItem() { Key="desaprobar", Content="Desaprobar" },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };
      }
      private void cargarListas(ControlMermaAutoViewModel model)
      {
         cargarListasCompartidas(model);

         List<TipoMovimientoAlmacenDTO> tipoMovimientoAlmacenDTOList = tipoMovimientoAlmacenProxy.Search(new TipoMovimientoAlmacenDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.TipoMovimientoAlmacenList = WebHelper.ToSelectListItem(tipoMovimientoAlmacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListasCompartidas(ControlMermaAutoViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            Estado = EstadoConstante.ACTIVO
         });
         model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

      }
      [HttpPost("ListarMovimientoAlmacenProducto")]
      public ActionResult ListarMovimientoAlmacenProducto(string jsonModel)
      {

         Logger.InfoStart();

         ControlMermaAutoViewModel model = JsonSerializer.Deserialize<ControlMermaAutoViewModel>(jsonModel);

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
      public ActionResult Aprobar([FromBody] ControlMermaAutoViewModel model)
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
      public ActionResult Desaprobar([FromBody] ControlMermaAutoViewModel model)
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