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
   public class GuiaRemiAutoController : AuthorizedController
   {

      private readonly IGuiaRemisionProxy guiaRemisionProxy;
      private readonly IGuiaRemisionProductoProxy guiaRemisionProductoProxy;
      private readonly IAlmacenProxy almacenProxy;
      private readonly ITransportistaProxy transportistaProxy;
      private readonly ITransportistaConductorProxy transportistaConductorProxy;
      private readonly ITransportistaUnidadProxy transportistaUnidadProxy;
      private readonly IMotivoTrasladoProxy motivoTrasladoProxy;

      public GuiaRemiAutoController(
         IGuiaRemisionProxy guiaRemisionProxy,
         IGuiaRemisionProductoProxy guiaRemisionProductoProxy,
         IAlmacenProxy almacenProxy,
         ITransportistaProxy transportistaProxy,
         ITransportistaConductorProxy transportistaConductorProxy,
         ITransportistaUnidadProxy transportistaUnidadProxy,
         IMotivoTrasladoProxy motivoTrasladoProxy
         )
      {
         this.guiaRemisionProxy = guiaRemisionProxy;
         this.guiaRemisionProductoProxy = guiaRemisionProductoProxy;
         this.almacenProxy = almacenProxy;
         this.transportistaProxy = transportistaProxy;
         this.transportistaConductorProxy = transportistaConductorProxy;
         this.transportistaUnidadProxy = transportistaUnidadProxy;
         this.motivoTrasladoProxy = motivoTrasladoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         GuiaRemiAutoViewModel guiaRemiAutoViewModel = new();

         guiaRemiAutoViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Edit }
         };

         return PartialView(guiaRemiAutoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         GuiaRemiAutoViewModel model = JsonSerializer.Deserialize<GuiaRemiAutoViewModel>(jsonModel);

         model.GuiaRemisionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.GuiaRemisionDTO.OrderBy = "gr.fecha desc,gr.serie,gr.numero";

         model.GuiaRemisionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.GuiaRemisionDTO.EsVenta = SiNoConstante.N;
         model.GuiaRemisionDTO.EstadoAprobacionDTO = new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.PENDIENTE_APROBACION };

         try
         {
            List<GuiaRemisionDTO> guiaRemisionDTOList = guiaRemisionProxy.Search(model.GuiaRemisionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = guiaRemisionDTOList.Count,
               rows = from f in guiaRemisionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.GuiaRemisionId,
                            f.Serie,
                            f.Numero,
                            Fecha = f.Fecha.ToShortDateString(),
                            MovimientoAlmacen = f.MovimientoAlmacenDTO.Codigo,
                            RazonSocial = f.MovimientoAlmacenDTO.ProveedorDTO == null ? string.Empty : f.MovimientoAlmacenDTO.ProveedorDTO.RazonSocial,
                            NumeroDocumentoIdentidad = f.MovimientoAlmacenDTO.ProveedorDTO == null ? string.Empty : f.MovimientoAlmacenDTO.ProveedorDTO.NumeroDocumentoIdentidad,
                            FechaInicioTraslado = f.FechaInicioTraslado.ToShortDateString(),
                            Usuario = f.UsuarioDTO.UsuarioLogon,
                            EstadoGuiaRemision = f.EstadoGuiaRemisionDTO.Codigo,
                            EstadoGuiaRemisionNombre = f.EstadoGuiaRemisionDTO.Nombre,
                            f.Impreso,
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
      public ActionResult Editar([FromBody] GuiaRemiAutoViewModel model)
      {

         Logger.InfoStart();

         GuiaRemiAutoViewModel guiaRemiAutoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(guiaRemiAutoViewModel, model.GuiaRemisionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, guiaRemiAutoViewModel);
      }
      private void cargarDataParaEditar(GuiaRemiAutoViewModel model, int id)
      {

         GuiaRemisionDTO guiaRemisionDTO = guiaRemisionProxy.GetById(new GuiaRemisionDTO() { GuiaRemisionId = id });

         model.GuiaRemisionDTO = guiaRemisionDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.AlmacenList, guiaRemisionDTO.MovimientoAlmacenDTO.AlmacenId);
         WebHelper.SelectedListItemSetValue(model.TransportistaConductorList, guiaRemisionDTO.TransportistaConductorId);
         WebHelper.SelectedListItemSetValue(model.TransportistaUnidadList, guiaRemisionDTO.TransportistaUnidadId);
         WebHelper.SelectedListItemSetValue(model.MotivoTrasladoList, guiaRemisionDTO.MotivoTrasladoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="aprobar", Content="Aprobar" },
            new MenuItem() { Key="desaprobar", Content="Desaprobar" },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };
      }
      private void cargarListas(GuiaRemiAutoViewModel model)
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

         List<TransportistaDTO> transportistaDTOList = transportistaProxy.Search(new TransportistaDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId
         });
         model.TransportistaList = WebHelper.ToSelectListItem(transportistaDTOList, x => x.Id.ToString(), x => x.RazonSocial,
               SelectListFirstOptionType.Select);

         TransportistaConductorDTO transportistaConductorDTO = null;
         if (model.GuiaRemisionDTO.TransportistaConductorId.HasValue)
            transportistaConductorDTO = transportistaConductorProxy.GetById(new TransportistaConductorDTO()
            {
               TransportistaConductorId = model.GuiaRemisionDTO.TransportistaConductorId.Value
            });

         if (model.GuiaRemisionDTO.TransportistaConductorId.HasValue)
         {
            List<TransportistaConductorDTO> transportistaConductorDTOList = transportistaConductorProxy.Search(new TransportistaConductorDTO()
            {
               TransportistaId = transportistaConductorDTO.TransportistaId
            });
            model.TransportistaConductorList = WebHelper.ToSelectListItem(transportistaConductorDTOList, x => x.Id.ToString(), x => x.ApellidosNombres,
                  SelectListFirstOptionType.Select);

            WebHelper.SelectedListItemSetValue(model.TransportistaList, transportistaConductorDTOList[0].TransportistaId);
         }
         else
            model.TransportistaConductorList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);

         if (model.GuiaRemisionDTO.TransportistaConductorId.HasValue)
         {
            List<TransportistaUnidadDTO> transportistaUnidadDTOList = transportistaUnidadProxy.Search(new TransportistaUnidadDTO()
            {
               TransportistaId = transportistaConductorDTO.TransportistaId
            });
            model.TransportistaUnidadList = WebHelper.ToSelectListItem(transportistaUnidadDTOList, x => x.Id.ToString(), x => x.MarcaModeloPlata,
                  SelectListFirstOptionType.Select);
         }
         else
            model.TransportistaUnidadList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);

         List<MotivoTrasladoDTO> motivoTrasladoDTOList = motivoTrasladoProxy.Search(new MotivoTrasladoDTO() { Estado = EstadoConstante.ACTIVO });
         model.MotivoTrasladoList = WebHelper.ToSelectListItem(motivoTrasladoDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

         if (model.SelectedItemList != null)
         {
            SelectedItem selectedItem = model.SelectedItemList.Where(x => x.Name == "MotivoTraslado").FirstOrDefault();
            if (selectedItem != null)
            {
               MotivoTrasladoDTO motivoTrasladoDTO = motivoTrasladoDTOList.Where(x => x.Codigo == selectedItem.Value).FirstOrDefault();
               if (motivoTrasladoDTO != null)
                  WebHelper.SelectedListItemSetValue(model.MotivoTrasladoList, motivoTrasladoDTO.Id);
            }

         }
      }
      [HttpPost("ListarGuiaRemisionProducto")]
      public ActionResult ListarGuiaRemisionProducto(string jsonModel)
      {

         Logger.InfoStart();

         GuiaRemiViewModel model = JsonSerializer.Deserialize<GuiaRemiViewModel>(jsonModel);

         model.GuiaRemisionProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.GuiaRemisionProductoDTO.OrderBy = "p.descripcion";

         try
         {
            List<GuiaRemisionProductoDTO> guiaRemisionProductoDTOList = guiaRemisionProductoProxy.Search(model.GuiaRemisionProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = guiaRemisionProductoDTOList.Count,
               rows = from f in guiaRemisionProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.GuiaRemisionProductoId,
                            f.GuiaRemisionId,
                            f.MovimientoAlmacenProductoId,
                            f.MovimientoAlmacenProductoDTO.ProductoDTO.Codigo,
                            ProductoNombre = f.MovimientoAlmacenProductoDTO.ProductoDTO.Descripcion,
                            UnidadMedidaNombre = f.MovimientoAlmacenProductoDTO.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            f.Descripcion,
                            f.Cantidad,
                            f.PesoNeto,
                            f.PesoBruto,
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
      public ActionResult Aprobar([FromBody] GuiaRemiAutoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            AuditDTO(model.GuiaRemisionDTO);

            guiaRemisionProxy.Aprobar(model.GuiaRemisionDTO);

            resultResponse.Message = SR.GetString("IV_guia_remision_autorizacion_aprobada");
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
      public ActionResult Desaprobar([FromBody] GuiaRemiAutoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            AuditDTO(model.GuiaRemisionDTO);

            guiaRemisionProxy.Desaprobar(model.GuiaRemisionDTO);

            resultResponse.Message = SR.GetString("IV_guia_remision_autorizacion_desaprobada");
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