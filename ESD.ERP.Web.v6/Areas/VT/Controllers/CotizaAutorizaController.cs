using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.Web.Services;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class CotizaAutorizaController : AuthorizedController
   {
      private readonly IEstadoAprobacionProxy estadoAprobacionProxy;
      private readonly ICotizacionAutorizacionProxy cotizacionAutorizacionProxy;
      private readonly ICotizacionProxy cotizacionProxy;
      private readonly ICotizacionProductoProxy cotizacionProductoProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IListaPrecioProxy listaPrecioProxy;
      private readonly IParametroSistemaProxy parametroSistemaProxy;
      private readonly ITipoCondicionPagoProxy tipoCondicionPagoProxy;
      private readonly INotifyService notifiyService;

      public CotizaAutorizaController(
         IEstadoAprobacionProxy estadoAprobacionProxy,
         ICotizacionAutorizacionProxy cotizacionAutorizacionProxy,
         ICotizacionProxy cotizacionProxy,
         ICotizacionProductoProxy cotizacionProductoProxy,
         IMonedaProxy monedaProxy,
         IListaPrecioProxy listaPrecioProxy,
         IParametroSistemaProxy parametroSistemaProxy,
         ITipoCondicionPagoProxy tipoCondicionPagoProxy,
         INotifyService notifiyService
         )
      {
         this.estadoAprobacionProxy = estadoAprobacionProxy;
         this.cotizacionAutorizacionProxy = cotizacionAutorizacionProxy;
         this.cotizacionProxy = cotizacionProxy;
         this.cotizacionProductoProxy = cotizacionProductoProxy;
         this.monedaProxy = monedaProxy;
         this.listaPrecioProxy = listaPrecioProxy;
         this.parametroSistemaProxy = parametroSistemaProxy;
         this.tipoCondicionPagoProxy = tipoCondicionPagoProxy;
         this.notifiyService = notifiyService;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         CotizaAutorizaViewModel cotizaAutorizaViewModel = new();

         cargarListaEstadoAprobacion(cotizaAutorizaViewModel);

         cotizaAutorizaViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Edit }
         };

         EstadoAprobacionDTO estadoAprobacionDTO = estadoAprobacionProxy.ObtenerPorCodigo(
            new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.PENDIENTE_APROBACION });

         WebHelper.SelectedListItemSetValue(cotizaAutorizaViewModel.EstadoAprobacionList, estadoAprobacionDTO.Id);

         return PartialView(cotizaAutorizaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         CotizaAutorizaViewModel model = JsonSerializer.Deserialize<CotizaAutorizaViewModel>(jsonModel);

         model.CotizacionAutorizacionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CotizacionAutorizacionDTO.OrderBy = "c.fecha desc,c.cotiza desc";
         model.CotizacionAutorizacionDTO.Estado = EstadoConstante.ACTIVO;

         model.CotizacionAutorizacionDTO.CotizacionDTO.PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO() { 
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
            }
         };

         try
         {
            List<CotizacionAutorizacionDTO> cotizacionAutorizacionDTOList = cotizacionAutorizacionProxy.Search(model.CotizacionAutorizacionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = cotizacionAutorizacionDTOList.Count,
               rows = from f in cotizacionAutorizacionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.CotizacionAutorizacionId,
                            f.CotizacionId,
                            EmpresaSedeNombre = f.CotizacionDTO.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.Nombre,
                            TipoAutorizacion = f.TipoAutorizacionDTO.Codigo,
                            TipoAutorizacionNombre = f.TipoAutorizacionDTO.Nombre,
                            f.CotizacionDTO.Codigo,
                            Fecha = f.CotizacionDTO.Fecha.ToShortDateString(),
                            f.CotizacionDTO.ProveedorDTO.RazonSocial,
                            f.CotizacionDTO.ProveedorDTO.NumeroDocumentoIdentidad,
                            VendedorNombre = f.CotizacionDTO.VendedorDTO.ProveedorDTO.RazonSocial,
                            Moneda = f.CotizacionDTO.MonedaDTO.Simbolo,
                            f.CotizacionDTO.MontoTotal,
                            EstadoAprobacion = f.EstadoAprobacionDTO.Codigo,
                            EstadoAprobacionNombre = f.EstadoAprobacionDTO.Nombre,
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
      public ActionResult Editar([FromBody] CotizaAutorizaViewModel model)
      {

         Logger.InfoStart();

         CotizaAutorizaViewModel cotizaAutorizaViewModel = new()
         {
            CotizacionAutorizacionDTO = new CotizacionAutorizacionDTO() { 
               CotizacionAutorizacionId = model.CotizacionAutorizacionDTO.Id,
              TipoAutorizacionDTO = model.CotizacionAutorizacionDTO.TipoAutorizacionDTO
            },
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(cotizaAutorizaViewModel, model.CotizacionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, cotizaAutorizaViewModel);
      }
      private void cargarDataParaEditar(CotizaAutorizaViewModel model, int id)
      {

         CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = id });
         model.CotizacionDTO = cotizacionDTO;

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.SingleSearch(new ParametroSistemaDTO() { 
            Codigo = ParametroSistemaConstante.VENTAS_PORCENTAJE_INCREMENTO_PU_SI_LA_CANTIDAD_ES_FRACCION });

         model.PorcentajeFraccionPU = parametroSistemaDTO.ValorNumerico;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.MonedaList, cotizacionDTO.MonedaId);
         WebHelper.SelectedListItemSetValue(model.ListaPrecioList, cotizacionDTO.ListaPrecioId);
         WebHelper.SelectedListItemSetValue(model.TipoCondicionPagoList, cotizacionDTO.TipoCondicionPagoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="aprobar", Content="Aprobar" },
            new MenuItem() { Key="desaprobar", Content="Desaprobar" },
            new MenuItem() {  MenuItemType = MenuItemType.Exit}
         };
      }
      private void cargarListaEstadoAprobacion(CotizaAutorizaViewModel model)
      {
         List<EstadoAprobacionDTO> estadoAprobacionDTOList = estadoAprobacionProxy.Search(new EstadoAprobacionDTO() { Estado = EstadoConstante.ACTIVO });
         model.EstadoAprobacionList = WebHelper.ToSelectListItem(estadoAprobacionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListas(CotizaAutorizaViewModel model)
      {
         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         if (model.CotizacionDTO.MonedaId == 0)
            model.ListaPrecioList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
         else
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.CotizacionDTO.MonedaId);

            model.ListaPrecioList = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
         }

         List<TipoCondicionPagoDTO> tipoCondicionPagoDTOList = tipoCondicionPagoProxy.Search(new TipoCondicionPagoDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoCondicionPagoList = WebHelper.ToSelectListItem(tipoCondicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private List<ListaPrecioDTO> obtenerListaPrecio(PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO, int monedaId)
      {
         List<ListaPrecioDTO> listaPrecioDTOList = listaPrecioProxy.Search(new ListaPrecioDTO()
         {
            EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            MonedaId = monedaId,
            Estado = EstadoConstante.ACTIVO
         });

         return listaPrecioDTOList;
      }
      [HttpPost("Aprobar")]
      public ActionResult Aprobar([FromBody] CotizaAutorizaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            int usuarioId = ObtenerUsuarioId();
            model.CotizacionAutorizacionDTO.UsuarioId = usuarioId;
            model.CotizacionAutorizacionDTO.UsuarioCreacionId = usuarioId;
            model.CotizacionAutorizacionDTO.UsuarioModificacionId = usuarioId;
            AuditDTO(model.CotizacionAutorizacionDTO);
            if(model.CotizacionProductoDTOList != null)
            {
               foreach(CotizacionProductoDTO cotizacionProductoDTO in model.CotizacionProductoDTOList)
               {
                  cotizacionProductoDTO.UsuarioCreacionId = usuarioId;
                  cotizacionProductoDTO.UsuarioModificacionId = usuarioId;
                  AuditDTO(cotizacionProductoDTO);
               }

               model.CotizacionAutorizacionDTO.CotizacionProductoDTOList = model.CotizacionProductoDTOList;
            }

            cotizacionAutorizacionProxy.Aprobar(model.CotizacionAutorizacionDTO);

            CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = model.CotizacionAutorizacionDTO.CotizacionId });

            string notificacionTitulo = string.Empty;
            string notificacionDescripcion = string.Empty;
            
            if(cotizacionDTO.EsPedido == SiNoConstante.N)
            {
               notificacionTitulo = "VT_cotizacion_autorizacion_notificacion_titulo";
               notificacionDescripcion = "VT_cotizacion_autorizacion_notificacion_descripcion";
            }
            else
            {
               notificacionTitulo = "VT_pedido_autorizacion_notificacion_titulo";
               notificacionDescripcion = "VT_pedido_autorizacion_notificacion_descripcion";
            }

            notifiyService.SendNotification(new NotificacionPushDTO()
            {
               UsuarioId = cotizacionDTO.VendedorDTO.ProveedorDTO.UsuarioId.Value,
               Titulo = SR.GetString(notificacionTitulo),
               Descripcion = SR.GetString(notificacionDescripcion, cotizacionDTO.Codigo, cotizacionDTO.ProveedorDTO.RazonSocial)
            });

            resultResponse.Message = SR.GetString("VT_cotizacion_autorizacion_aprobada");
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
      public ActionResult Desaprobar([FromBody] CotizaAutorizaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            int usuarioId = ObtenerUsuarioId();
            model.CotizacionAutorizacionDTO.UsuarioId = usuarioId;
            model.CotizacionAutorizacionDTO.UsuarioCreacionId = usuarioId;
            model.CotizacionAutorizacionDTO.UsuarioModificacionId = usuarioId;
            AuditDTO(model.CotizacionAutorizacionDTO);

            cotizacionAutorizacionProxy.Desaprobar(model.CotizacionAutorizacionDTO);

            resultResponse.Message = SR.GetString("VT_cotizacion_autorizacion_desaprobada");
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
      [HttpPost("ListarCotizacionProducto")]
      public ActionResult ListarCotizacionProducto(string jsonModel)
      {

         Logger.InfoStart();

         CotizaAutorizaViewModel model = JsonSerializer.Deserialize<CotizaAutorizaViewModel>(jsonModel);

         model.CotizacionProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CotizacionProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.CotizacionProductoDTO.NullRegistroComboId = SiNoConstante.S;
         model.CotizacionProductoDTO.Estado = EstadoConstante.ACTIVO;

         try
         {
            List<CotizacionProductoDTO> cotizacionProductoDTOList = cotizacionProductoProxy.Search(model.CotizacionProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = cotizacionProductoDTOList.Count,
               rows = from f in cotizacionProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.CotizacionProductoId,
                            f.CotizacionId,
                            f.ProductoId,
                            f.ProductoDTO.Codigo,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.UsarIncrementoFraccion,
                            TipoProducto = f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO.Codigo,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            TipoIgv = f.ProductoDTO.TipoIgvDTO.Codigo,
                            f.UnidadMedidaId,
                            f.Descripcion,
                            f.Cantidad,
                            f.ValorUnitarioLista,
                            f.PrecioUnitarioLista,
                            f.ValorUnitario,
                            f.PrecioUnitario,
                            PrecioAutorizado = f.PrecioUnitario,
                            f.IncluyeDescuentoListaPrecio,
                            f.SinSaldo,
                            f.SolicitaAutorizacionDescuento,
                            f.MontoBruto,
                            f.MontoNeto,
                            f.MontoIgv,
                            f.MontoTotal,
                            f.TipoAfectacionIgvId,
                            f.TipoIscId,
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
      [HttpPost("CalcularMontos")]
      public ActionResult CalcularMontos([FromBody] CotizaAutorizaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(model.CalculadorVentaItemDTO);

            resultResponse.Data = new
            {
               calculadorVentaItemDTO.PrecioUnitario,
               calculadorVentaItemDTO.ValorUnitario,
               calculadorVentaItemDTO.MontoBruto,
               calculadorVentaItemDTO.MontoNeto,
               calculadorVentaItemDTO.MontoImpuesto,
               calculadorVentaItemDTO.MontoTotal
            };
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