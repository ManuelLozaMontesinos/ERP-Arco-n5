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
   public class PedidoComAutoController : AuthorizedController
   {
      private readonly IPedidoCompraProxy pedidoCompraProxy;
      private readonly ITipoProductoProxy tipoProductoProxy;
      private readonly IPedidoCompraProductoProxy pedidoCompraProductoProxy;
      public PedidoComAutoController(
         IPedidoCompraProxy pedidoCompraProxy,
         ITipoProductoProxy tipoProductoProxy,
         IPedidoCompraProductoProxy pedidoCompraProductoProxy
         )
      {
         this.pedidoCompraProxy = pedidoCompraProxy;
         this.tipoProductoProxy = tipoProductoProxy;
         this.pedidoCompraProductoProxy = pedidoCompraProductoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         PedidoComAutoViewModel pedidoComAutoViewModel = new();

         cargarListaTipoProducto(pedidoComAutoViewModel);

         pedidoComAutoViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Edit }
         };

         return PartialView(pedidoComAutoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PedidoComAutoViewModel model = JsonSerializer.Deserialize<PedidoComAutoViewModel>(jsonModel);

         model.PedidoCompraDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PedidoCompraDTO.OrderBy = "pc.fecha desc,pc.pedido_co desc";

         model.PedidoCompraDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.PedidoCompraDTO.EstadoAprobacionDTO = new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.PENDIENTE_APROBACION };

         try
         {
            List<PedidoCompraDTO> pedidoCompraDTOList = pedidoCompraProxy.Search(model.PedidoCompraDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = pedidoCompraDTOList.Count,
               rows = from f in pedidoCompraDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PedidoCompraId,
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
                            EstadoPedidoCompra = f.EstadoPedidoCompraDTO.Codigo,
                            EstadoPedidoCompraNombre = f.EstadoPedidoCompraDTO.Nombre,
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
      public ActionResult Editar([FromBody] PedidoComAutoViewModel model)
      {

         Logger.InfoStart();

         PedidoComAutoViewModel pedidoComAutoViewModel = new PedidoComAutoViewModel
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(pedidoComAutoViewModel, model.PedidoCompraDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, pedidoComAutoViewModel);
      }
      private void cargarDataParaEditar(PedidoComAutoViewModel model, int id)
      {

         PedidoCompraDTO pedidoCompraDTO = pedidoCompraProxy.GetById(new PedidoCompraDTO() { PedidoCompraId = id });

         model.PedidoCompraDTO = pedidoCompraDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.TipoProductoList, pedidoCompraDTO.TipoProductoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="aprobar", Content="Aprobar" },
            new MenuItem() { Key="desaprobar", Content="Desaprobar" },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };
      }
      private void cargarListas(PedidoComAutoViewModel model)
      {
         cargarListaTipoProducto(model);
      }
      private void cargarListaTipoProducto(PedidoComAutoViewModel model)
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
      [HttpPost("ListarPedidoCompraProducto")]
      public ActionResult ListarPedidoCompraProducto(string jsonModel)
      {

         Logger.InfoStart();

         PedidoCompraViewModel model = JsonSerializer.Deserialize<PedidoCompraViewModel>(jsonModel);

         model.PedidoCompraProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PedidoCompraProductoDTO.OrderBy = "tp.tipo_producto,p.producto";

         try
         {
            List<PedidoCompraProductoDTO> pedidoCompraProductoDTOList = pedidoCompraProductoProxy.Search(model.PedidoCompraProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = pedidoCompraProductoDTOList.Count,
               rows = from f in pedidoCompraProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PedidoCompraProductoId,
                            f.PedidoCompraId,
                            f.ProductoId,
                            f.ProductoDTO?.Codigo,
                            ProductoNombre = f?.ProductoDTO?.Descripcion,
                            TipoProducto = f?.ProductoDTO?.FamiliaProductoDTO?.ClaseProductoDTO?.GrupoProductoDTO?.TipoProductoDTO?.Codigo,
                            f?.ProductoDTO?.UnidadMedidaDTO?.UnidadMedidaId,
                            UnidadMedidaNombre = f?.ProductoDTO?.UnidadMedidaDTO?.Abreviatura,
                            f.Descripcion,
                            f.Cantidad,
                            f.PesoBruto,
                            f.PesoNeto,
                            f.StockEnProveedor,
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
      public ActionResult Aprobar([FromBody] PedidoComAutoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            AuditDTO(model.PedidoCompraDTO);

            pedidoCompraProxy.Aprobar(model.PedidoCompraDTO);

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
      public ActionResult Desaprobar([FromBody] PedidoComAutoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            AuditDTO(model.PedidoCompraDTO);

            pedidoCompraProxy.Desaprobar(model.PedidoCompraDTO);

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