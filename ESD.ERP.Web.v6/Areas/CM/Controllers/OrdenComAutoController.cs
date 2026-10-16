using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.CM.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.CM.Controllers
{
   [Area("CM")]
   [Route("[controller]")]
   public class OrdenComAutoController : AuthorizedController
   {
      private readonly IOrdenCompraProxy ordenCompraProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly ITipoProductoProxy tipoProductoProxy;
      private readonly IOrdenCompraProductoProxy ordenCompraProductoProxy;
      private readonly IOrdenCompraEntregaProxy ordenCompraEntregaProxy;
      private readonly ICondicionPagoProxy condicionPagoProxy;
      private readonly IImpuestoTasaProxy impuestoTasaProxy;
      public OrdenComAutoController(
         IOrdenCompraProxy ordenCompraProxy,
         IMonedaProxy monedaProxy,
         ITipoProductoProxy tipoProductoProxy,
         IOrdenCompraProductoProxy ordenCompraProductoProxy,
         IOrdenCompraEntregaProxy ordenCompraEntregaProxy,
         ICondicionPagoProxy condicionPagoProxy,
         IImpuestoTasaProxy impuestoTasaProxy
         )
      {
         this.ordenCompraProxy = ordenCompraProxy;
         this.monedaProxy = monedaProxy;
         this.tipoProductoProxy = tipoProductoProxy;
         this.ordenCompraProductoProxy = ordenCompraProductoProxy;
         this.ordenCompraEntregaProxy = ordenCompraEntregaProxy;
         this.condicionPagoProxy = condicionPagoProxy;
         this.impuestoTasaProxy = impuestoTasaProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         OrdenComAutoViewModel ordenComAutoViewModel = new();

         cargarListaTipoProducto(ordenComAutoViewModel);

         ordenComAutoViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Edit }
         };

         return PartialView(ordenComAutoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         OrdenComAutoViewModel model = JsonSerializer.Deserialize<OrdenComAutoViewModel>(jsonModel);

         model.OrdenCompraDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenCompraDTO.OrderBy = "oc.fecha desc,oc.orden_co desc";

         model.OrdenCompraDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.OrdenCompraDTO.EstadoAprobacionDTO = new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.PENDIENTE_APROBACION };

         try
         {
            List<OrdenCompraDTO> ordenCompraDTOList = ordenCompraProxy.Search(model.OrdenCompraDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = ordenCompraDTOList.Count,
               rows = from f in ordenCompraDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.OrdenCompraId,
                            f.Codigo,
                            f.TipoProductoId,
                            TipoProducto = f.TipoProductoDTO.Nombre,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.ProveedorId,
                            Proveedor = f.ProveedorDTO.Codigo,
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            f.EsModificacion,
                            EstadoAprobacion = f?.EstadoAprobacionDTO?.Codigo,
                            EstadoAprobacionNombre = f?.EstadoAprobacionDTO?.Nombre,
                            EstadoOrdenCompra = f.EstadoOrdenCompraDTO.Codigo,
                            EstadoOrdenCompraNombre = f.EstadoOrdenCompraDTO.Nombre,
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
      public ActionResult Editar([FromBody] OrdenComAutoViewModel model)
      {

         Logger.InfoStart();

         OrdenComAutoViewModel ordenComAutoViewModel = new OrdenComAutoViewModel
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(ordenComAutoViewModel, model.OrdenCompraDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, ordenComAutoViewModel);
      }
      private void cargarDataParaEditar(OrdenComAutoViewModel model, int id)
      {

         OrdenCompraDTO ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId = id });

         model.OrdenCompraDTO = ordenCompraDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.MonedaList, ordenCompraDTO.MonedaId);
         WebHelper.SelectedListItemSetValue(model.TipoProductoList, ordenCompraDTO.TipoProductoId);
         WebHelper.SelectedListItemSetValue(model.ImpuestoTasaList, ordenCompraDTO.ImpuestoTasaId);
         WebHelper.SelectedListItemSetValue(model.CondicionPagoList, ordenCompraDTO.CondicionPagoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="aprobar", Content="Aprobar" },
            new MenuItem() { Key="desaprobar", Content="Desaprobar" },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };
      }
      private void cargarListas(OrdenComAutoViewModel model)
      {

         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<CondicionPagoDTO> condicionPagoDTOList = condicionPagoProxy.Search(new CondicionPagoDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO
         });
         model.CondicionPagoList = WebHelper.ToSelectListItem(condicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<ImpuestoTasaDTO> impuestoTasaDTOList = impuestoTasaProxy.Search(new ImpuestoTasaDTO()
         {
            Estado = EstadoConstante.ACTIVO,
         });
         model.ImpuestoTasaList = WebHelper.ToSelectListItem(impuestoTasaDTOList, x => x.Id.ToString(), x => x.ImpuestoNombreTasa,
            SelectListFirstOptionType.Select);

         cargarListaTipoProducto(model);
      }
      private void cargarListaTipoProducto(OrdenComAutoViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<TipoProductoDTO> tipoProductoDTOList = tipoProductoProxy.Search(new TipoProductoDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO
         }
         );
         model.TipoProductoList = WebHelper.ToSelectListItem(tipoProductoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("ListarOrdenCompraProducto")]
      public ActionResult ListarOrdenCompraProducto(string jsonModel)
      {

         Logger.InfoStart();

         OrdenComAutoViewModel model = JsonSerializer.Deserialize<OrdenComAutoViewModel>(jsonModel);

         model.OrdenCompraProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenCompraProductoDTO.OrderBy = "tp.tipo_producto,p.producto";

         try
         {
            List<OrdenCompraProductoDTO> ordenCompraProductoDTOList = ordenCompraProductoProxy.Search(model.OrdenCompraProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = ordenCompraProductoDTOList.Count,
               rows = from f in ordenCompraProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.OrdenCompraProductoId,
                            f.OrdenCompraId,
                            f.ProductoId,
                            f.ProductoDTO.Codigo,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            TipoProducto = f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO.Codigo,
                            f.ProductoDTO.UnidadMedidaDTO.UnidadMedidaId,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            TipoIgv = f.ProductoDTO.TipoIgvDTO.Codigo,
                            f.Descripcion,
                            f.Cantidad,
                            f.ValorUnitario,
                            f.PrecioUnitario,
                            f.MontoBruto,
                            f.MontoImpuesto,
                            f.MontoTotal,
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
      [HttpPost("ListarOrdenCompraEntrega")]
      public ActionResult ListarOrdenCompraEntrega(string jsonModel)
      {

         Logger.InfoStart();

         OrdenComAutoViewModel model = JsonSerializer.Deserialize<OrdenComAutoViewModel>(jsonModel);

         model.OrdenCompraEntregaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenCompraEntregaDTO.OrderBy = "oce.fecha";

         model.OrdenCompraEntregaDTO.OrdenCompraProductoId = NumberUtil.WhenValueIsZero(model.OrdenCompraEntregaDTO.OrdenCompraProductoId);

         try
         {
            List<OrdenCompraEntregaDTO> ordenCompraEntregaDTOList = ordenCompraEntregaProxy.Search(model.OrdenCompraEntregaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = ordenCompraEntregaDTOList.Count,
               rows = from f in ordenCompraEntregaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.OrdenCompraEntregaId,
                            f.OrdenCompraProductoId,
                            f.Cantidad,
                            Fecha = f.Fecha == null ? string.Empty : f.Fecha.Value.ToShortDateString(),
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
      public ActionResult Aprobar([FromBody] OrdenComAutoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            AuditDTO(model.OrdenCompraDTO);

            ordenCompraProxy.Aprobar(model.OrdenCompraDTO);

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
      public ActionResult Desaprobar([FromBody] OrdenComAutoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            AuditDTO(model.OrdenCompraDTO);

            ordenCompraProxy.Desaprobar(model.OrdenCompraDTO);

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