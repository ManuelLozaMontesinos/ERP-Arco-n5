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
   public class ComproAutorizaController : AuthorizedController
   {
      private readonly IEstadoAprobacionProxy estadoAprobacionProxy;
      private readonly IComprobanteAutorizacionProxy comprobanteAutorizacionProxy;
      private readonly IComprobanteProxy comprobanteProxy;
      private readonly IComprobanteProductoProxy comprobanteProductoProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IListaPrecioProxy listaPrecioProxy;
      private readonly IParametroSistemaProxy parametroSistemaProxy;
      private readonly ITipoCondicionPagoProxy tipoCondicionPagoProxy;
      private readonly INotifyService notifiyService;

      public ComproAutorizaController(
         IEstadoAprobacionProxy estadoAprobacionProxy,
         IComprobanteAutorizacionProxy comprobanteAutorizacionProxy,
         IComprobanteProxy comprobanteProxy,
         IComprobanteProductoProxy comprobanteProductoProxy,
         IMonedaProxy monedaProxy,
         IListaPrecioProxy listaPrecioProxy,
         IParametroSistemaProxy parametroSistemaProxy,
         ITipoCondicionPagoProxy tipoCondicionPagoProxy,
         INotifyService notifiyService
         )
      {
         this.estadoAprobacionProxy = estadoAprobacionProxy;
         this.comprobanteAutorizacionProxy = comprobanteAutorizacionProxy;
         this.comprobanteProxy = comprobanteProxy;
         this.comprobanteProductoProxy = comprobanteProductoProxy;
         this.monedaProxy = monedaProxy;
         this.listaPrecioProxy = listaPrecioProxy;
         this.parametroSistemaProxy = parametroSistemaProxy;
         this.tipoCondicionPagoProxy = tipoCondicionPagoProxy;
         this.notifiyService = notifiyService;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ComproAutorizaViewModel cotizaAutorizaViewModel = new ComproAutorizaViewModel();

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

         ComproAutorizaViewModel model = JsonSerializer.Deserialize<ComproAutorizaViewModel>(jsonModel);

         model.ComprobanteAutorizacionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteAutorizacionDTO.OrderBy = "c.fecha desc";

         model.ComprobanteAutorizacionDTO.ComprobanteDTO.PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO() { 
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
            }
         };

         try
         {
            List<ComprobanteAutorizacionDTO> comprobanteAutorizacionDTOList = comprobanteAutorizacionProxy.Search(model.ComprobanteAutorizacionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteAutorizacionDTOList.Count,
               rows = from f in comprobanteAutorizacionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteAutorizacionId,
                            f.ComprobanteId,
                            EmpresaSedeNombre = f.ComprobanteDTO.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.Nombre,                            
                            TipoAutorizacion = f.TipoAutorizacionDTO.Codigo,
                            TipoAutorizacionNombre = f.TipoAutorizacionDTO.Nombre,
                            TipoDocumentoComercialNombre = f.ComprobanteDTO.TipoDocumentoComercialDTO.Abreviatura,
                            f.ComprobanteDTO.Serie,
                            Fecha = f.ComprobanteDTO.Fecha.ToShortDateString(),
                            f.ComprobanteDTO.ProveedorDTO.RazonSocial,
                            f.ComprobanteDTO.ProveedorDTO.NumeroDocumentoIdentidad,
                            VendedorNombre = f.ComprobanteDTO.VendedorDTO.ProveedorDTO.RazonSocial,
                            Moneda = f.ComprobanteDTO.MonedaDTO.Simbolo,
                            f.ComprobanteDTO.MontoTotal,
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
      public ActionResult Editar([FromBody] ComproAutorizaViewModel model)
      {

         Logger.InfoStart();

         ComproAutorizaViewModel cotizaAutorizaViewModel = new ComproAutorizaViewModel
         {
            ComprobanteAutorizacionDTO = new ComprobanteAutorizacionDTO() { 
               ComprobanteAutorizacionId = model.ComprobanteAutorizacionDTO.Id,
              TipoAutorizacionDTO = model.ComprobanteAutorizacionDTO.TipoAutorizacionDTO
            },
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(cotizaAutorizaViewModel, model.ComprobanteDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, cotizaAutorizaViewModel);
      }
      private void cargarDataParaEditar(ComproAutorizaViewModel model, int id)
      {

         ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = id });
         model.ComprobanteDTO = comprobanteDTO;

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.SingleSearch(new ParametroSistemaDTO() { 
            Codigo = ParametroSistemaConstante.VENTAS_PORCENTAJE_INCREMENTO_PU_SI_LA_CANTIDAD_ES_FRACCION });

         model.PorcentajeFraccionPU = parametroSistemaDTO.ValorNumerico;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.MonedaList, comprobanteDTO.MonedaId);
         WebHelper.SelectedListItemSetValue(model.ListaPrecioList, comprobanteDTO.ListaPrecioId);
         WebHelper.SelectedListItemSetValue(model.TipoCondicionPagoList, comprobanteDTO.TipoCondicionPagoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="aprobar", Content="Aprobar" },
            new MenuItem() { Key="desaprobar", Content="Desaprobar" },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };
      }
      private void cargarListaEstadoAprobacion(ComproAutorizaViewModel model)
      {
         List<EstadoAprobacionDTO> estadoAprobacionDTOList = estadoAprobacionProxy.Search(new EstadoAprobacionDTO() { Estado = EstadoConstante.ACTIVO });
         model.EstadoAprobacionList = WebHelper.ToSelectListItem(estadoAprobacionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListas(ComproAutorizaViewModel model)
      {
         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         if (model.ComprobanteDTO.MonedaId == 0)
            model.ListaPrecioList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
         else
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.ComprobanteDTO.MonedaId);

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
      public ActionResult Aprobar([FromBody] ComproAutorizaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            AuditDTO(model.ComprobanteAutorizacionDTO);
            if(model.ComprobanteProductoDTOList != null)
            {
               foreach(ComprobanteProductoDTO comprobanteProductoDTO in model.ComprobanteProductoDTOList)
                  AuditDTO(comprobanteProductoDTO);

               model.ComprobanteAutorizacionDTO.ComprobanteProductoDTOList = model.ComprobanteProductoDTOList;
            }

            comprobanteAutorizacionProxy.Aprobar(model.ComprobanteAutorizacionDTO);

            ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = model.ComprobanteAutorizacionDTO.ComprobanteId });

            notifiyService.SendNotification(new NotificacionPushDTO()
            {
               UsuarioId = comprobanteDTO.VendedorDTO.ProveedorDTO.UsuarioId.Value,
               Titulo = SR.GetString("VT_comprobante_autorizacion_notificacion_titulo"),
               Descripcion = SR.GetString("VT_comprobante_autorizacion_notificacion_descripcion", comprobanteDTO.SerieNumero,
                  comprobanteDTO.ProveedorDTO.RazonSocial)
            });

            resultResponse.Message = SR.GetString("VT_comprobante_autorizacion_aprobada");
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
      public ActionResult Desaprobar([FromBody] ComproAutorizaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            AuditDTO(model.ComprobanteAutorizacionDTO);

            comprobanteAutorizacionProxy.Desaprobar(model.ComprobanteAutorizacionDTO);

            resultResponse.Message = SR.GetString("VT_comprobante_autorizacion_desaprobada");
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
      [HttpPost("ListarComprobanteProducto")]
      public ActionResult ListarComprobanteProducto(string jsonModel)
      {

         Logger.InfoStart();

         ComproAutorizaViewModel model = JsonSerializer.Deserialize<ComproAutorizaViewModel>(jsonModel);

         model.ComprobanteProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.ComprobanteProductoDTO.NullRegistroComboId = SiNoConstante.S;
         model.ComprobanteProductoDTO.Estado = EstadoConstante.ACTIVO;

         try
         {
            List<ComprobanteProductoDTO> comprobanteProductoDTOList = comprobanteProductoProxy.Search(model.ComprobanteProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteProductoDTOList.Count,
               rows = from f in comprobanteProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteProductoId,
                            f.ComprobanteId,
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
      public ActionResult CalcularMontos([FromBody] ComproAutorizaViewModel model)
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
               calculadorVentaItemDTO.MontoTotal,
               calculadorVentaItemDTO.MontoDetraccion
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