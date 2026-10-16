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
   public class PedidoCompraController : AuthorizedController
   {
      private readonly IPedidoCompraProxy pedidoCompraProxy;
      private readonly ITipoProductoProxy tipoProductoProxy;
      private readonly IEstadoPedidoCompraProxy estadoPedidoCompraProxy;
      private readonly IProveedorProxy proveedorProxy;
      private readonly IPedidoCompraProductoProxy pedidoCompraProductoProxy;
      private readonly AppHelper appHelper;

      public PedidoCompraController(
         IPedidoCompraProxy pedidoCompraProxy,
         ITipoProductoProxy tipoProductoProxy,
         IEstadoPedidoCompraProxy estadoPedidoCompraProxy,
         IProveedorProxy proveedorProxy,
         IPedidoCompraProductoProxy pedidoCompraProductoProxy,
         AppHelper appHelper
         )
      {
         this.pedidoCompraProxy = pedidoCompraProxy;
         this.tipoProductoProxy = tipoProductoProxy;
         this.estadoPedidoCompraProxy = estadoPedidoCompraProxy;
         this.proveedorProxy = proveedorProxy;
         this.pedidoCompraProductoProxy = pedidoCompraProductoProxy;
         this.appHelper = appHelper;
      }

      [Route("[action]")]
      public IActionResult Index()
      {
         PedidoCompraViewModel pedidoCompraViewModel = new();

         cargarListaTipoProducto(pedidoCompraViewModel);

         MenuBarTagHelper menuBarTagHelper = new();

         pedidoCompraViewModel.MenuItemList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.CRUDAndPreview);

         pedidoCompraViewModel.MenuItemList.Add(new MenuItem()
         {
            Key = "desde_stock",
            Content = "Crear desde Punto de Pedido",
            Position = 0
         });

         return PartialView(pedidoCompraViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PedidoCompraViewModel model = JsonSerializer.Deserialize<PedidoCompraViewModel>(jsonModel);

         model.PedidoCompraDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PedidoCompraDTO.OrderBy = "pc.fecha desc,pc.pedido_co desc";

         model.PedidoCompraDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         
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
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody] PedidoCompraViewModel model)
      {
         Logger.InfoStart();

         PedidoCompraViewModel pedidoCompraViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true,
         };

         try
         {
            cargarDataParaEditar(pedidoCompraViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, pedidoCompraViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] PedidoCompraViewModel model)
      {

         Logger.InfoStart();

         PedidoCompraViewModel pedidoCompraViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(pedidoCompraViewModel, model.PedidoCompraDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, pedidoCompraViewModel);
      }
      private void cargarDataParaEditar(PedidoCompraViewModel model, int id)
      {

         PedidoCompraDTO pedidoCompraDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            pedidoCompraDTO = new PedidoCompraDTO()
            {
               Fecha = DateTime.Today,
               EsModificacion = SiNoConstante.N
            };

            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            ProveedorDTO proveedorDTO = proveedorProxy.SingleSearch(new ProveedorDTO()
            {
               EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
               Codigo = ProveedorConstante.GENERICO
            });

            pedidoCompraDTO.ProveedorDTO = proveedorDTO;
            pedidoCompraDTO.ProveedorId = proveedorDTO.Id;

            TipoProductoDTO tipoProductoDTO = tipoProductoProxy.ObtenerPorCodigo(new TipoProductoDTO() {Codigo = TipoProductoConstante.PRODUCTO});
            pedidoCompraDTO.TipoProductoId = tipoProductoDTO.Id;
            pedidoCompraDTO.TipoProductoDTO = tipoProductoDTO;

            EstadoPedidoCompraDTO estadoPedidoCompraDTO = estadoPedidoCompraProxy.ObtenerPorCodigo(new EstadoPedidoCompraDTO()
            {
               Codigo = EstadoPedidoCompraConstante.EN_EDICION
            });
            pedidoCompraDTO.EstadoPedidoCompraDTO = estadoPedidoCompraDTO;
            pedidoCompraDTO.EstadoPedidoCompraId = estadoPedidoCompraDTO.Id;
         }
         else
         {
            pedidoCompraDTO = pedidoCompraProxy.GetById(new PedidoCompraDTO() { PedidoCompraId  =  id });
         }

         model.PedidoCompraDTO = pedidoCompraDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.TipoProductoList, pedidoCompraDTO.TipoProductoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { Key="solicitar", Content="Solicitar Aprobación" },
            new MenuItem() { Key="modificar", Content="Solicitar Modificación", Disabled = true },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };

         MenuBarTagHelper menuBarTagHelper = new();

         model.MenuItemList2 = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.CRUDAndSaveCancel);

         model.MenuItemList2.Add(new MenuItem()
         {
            Key = "asociar",
            Content = "Asociar Producto"
         });

      }
      private void cargarListaTipoProducto(PedidoCompraViewModel model)
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
      private void cargarListas(PedidoCompraViewModel model)
      {
         cargarListaTipoProducto(model);
      }
      [HttpGet("ObtenerTipoProducto")]
      public JsonResult ObtenerTipoProducto(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            TipoProductoDTO tipoProductoDTO = tipoProductoProxy.GetById(new TipoProductoDTO() { TipoProductoId = id });

            resultResponse.Data = new
            {
               tipoProductoDTO.Codigo
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] PedidoCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PedidoCompraDTO  pedidoCompraDTO = cargarPedidoCompraDTO(model);

            int newId = model.PedidoCompraDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = pedidoCompraProxy.Insert(pedidoCompraDTO);
                  messageResourceKey = "CM_orden_compra_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  pedidoCompraProxy.Update(pedidoCompraDTO);
                  messageResourceKey = "CM_orden_compra_actualizada";
                  break;
            }

            pedidoCompraDTO = pedidoCompraProxy.GetById(new PedidoCompraDTO() { PedidoCompraId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, pedidoCompraDTO.Codigo);
            resultResponse.Data = new
            {
               pedidoCompraDTO.Id,
               pedidoCompraDTO.Codigo 
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
      private PedidoCompraDTO cargarPedidoCompraDTO(PedidoCompraViewModel model)
      {

         PedidoCompraDTO pedidoCompraDTO;
         try
         {

            pedidoCompraDTO = model.PedidoCompraDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

               pedidoCompraDTO = model.PedidoCompraDTO;

               pedidoCompraDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;
               pedidoCompraDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;

               EstadoPedidoCompraDTO estadoPedidoCompraDTO = estadoPedidoCompraProxy.ObtenerPorCodigo(new EstadoPedidoCompraDTO()
               {
                  Codigo = EstadoPedidoCompraConstante.EN_EDICION 
               });
               pedidoCompraDTO.EstadoPedidoCompraDTO = estadoPedidoCompraDTO;
               pedidoCompraDTO.EstadoPedidoCompraId = estadoPedidoCompraDTO.Id;
               pedidoCompraDTO.EsModificacion = SiNoConstante.N;
            }
            else
            {
               pedidoCompraDTO = pedidoCompraProxy.GetById(new PedidoCompraDTO() { PedidoCompraId = model.PedidoCompraDTO.Id});

               model.PedidoCompraDTO.TipoProductoId = pedidoCompraDTO.TipoProductoId;
               model.PedidoCompraDTO.EsModificacion = pedidoCompraDTO.EsModificacion;
               model.PedidoCompraDTO.EstadoPedidoCompraDTO = pedidoCompraDTO.EstadoPedidoCompraDTO;
               model.PedidoCompraDTO.EstadoPedidoCompraId = pedidoCompraDTO.EstadoPedidoCompraId;
               model.PedidoCompraDTO.EstadoOrdenCompraAnteriorId = pedidoCompraDTO.EstadoOrdenCompraAnteriorId;

               pedidoCompraDTO = model.PedidoCompraDTO;
            }

            if (string.IsNullOrEmpty(pedidoCompraDTO.Observacion))
               pedidoCompraDTO.Observacion = string.Empty;

            pedidoCompraDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(pedidoCompraDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return pedidoCompraDTO;
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] PedidoCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.PedidoCompraDTO.DTOState = DTOState.Modified;

            AuditDTO(model.PedidoCompraDTO);
            pedidoCompraProxy.Delete(model.PedidoCompraDTO);

            PedidoCompraDTO pedidoCompraDTO = pedidoCompraProxy.GetById(model.PedidoCompraDTO);

            resultResponse.Message = SR.GetString("CM_orden_compra_anulada", pedidoCompraDTO.Codigo);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string pedidoCompraId)
      {
         Logger.InfoStart();

         byte[] reportData = obtenerReporteData(pedidoCompraId);

         PedidoCompraDTO ordenCompraDTO = pedidoCompraProxy.GetById(new PedidoCompraDTO() { PedidoCompraId = Convert.ToInt32(pedidoCompraId) });

         return appHelper.ReportToPdf(reportData, string.Concat("PC", ordenCompraDTO.Codigo));
      }
      private byte[] obtenerReporteData(string pedidoCompraId)
      {
         Dictionary<string, string> parameters = new()
         {
            { "pedidoCompraId", pedidoCompraId }
         };

         byte[] reportData = appHelper.ExecuteReport("CM_PedidoCompra", parameters, ReportFormatConstant.PDF);

         return reportData;
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
                            f?.ProductoDTO?.Codigo,
                            ProductoNombre = f?.ProductoDTO?.Descripcion,
                            TipoProducto = f?.ProductoDTO?.FamiliaProductoDTO?.ClaseProductoDTO?.GrupoProductoDTO?.TipoProductoDTO?.Codigo,
                            f?.ProductoDTO?.UnidadMedidaDTO?.UnidadMedidaId,
                            UnidadMedidaNombre = f?.ProductoDTO?.UnidadMedidaDTO?.Abreviatura,
                            f.Descripcion,
                            f.Cantidad,
                            f.PesoBruto,
                            f.PesoNeto,
                            StockEnProveedor = f.StockEnProveedor == SiNoConstante.S ? DigitoConstante.UNO : DigitoConstante.CERO,
                            f.EsAdicional,
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
      [HttpPost("GrabarPedidoCompraProducto")]
      public ActionResult GrabarPedidoCompraProducto([FromBody] PedidoCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PedidoCompraProductoDTO pedidoCompraProductoDTO = cargarPedidoCompraProductoDTO(model);

            int newId = model.PedidoCompraProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (pedidoCompraProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = pedidoCompraProductoProxy.Insert(pedidoCompraProductoDTO);
                  messageResourceKey = "IV_producto_agregado";
                  break;

               case DTOState.Modified:
                  pedidoCompraProductoProxy.Update(pedidoCompraProductoDTO);
                  messageResourceKey = "IV_producto_actualizado";
                  break;
            }

            resultResponse.Message = SR.GetString(messageResourceKey);
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
      private PedidoCompraProductoDTO cargarPedidoCompraProductoDTO(PedidoCompraViewModel model)
      {

         PedidoCompraProductoDTO pedidoCompraProductoDTO;
         try
         {

            pedidoCompraProductoDTO = model.PedidoCompraProductoDTO;

            AuditDTO(pedidoCompraProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return pedidoCompraProductoDTO;
      }
      [HttpPost("EliminarPedidoCompraProducto")]
      public ActionResult EliminarPedidoCompraProducto([FromBody] PedidoCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.PedidoCompraProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.PedidoCompraProductoDTO);

            PedidoCompraProductoDTO pedidoCompraProductoDTO = pedidoCompraProductoProxy.GetById(model.PedidoCompraProductoDTO);

            pedidoCompraProductoProxy.Delete(pedidoCompraProductoDTO);

            resultResponse.Message = SR.GetString("VT_producto_anulado");
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
      [HttpPost("ListPopup")]
      public ActionResult ListPopup([FromBody] PedidoCompraViewModel model)
      {

         if (model == null)
            model = new PedidoCompraViewModel();

         model.ModalEditId = GetModalEditPanelId();

         return PartialView(model);
      }
      [HttpPost("ListarBandejaPopup")]
      public ActionResult ListarBandejaPopup(string jsonModel)
      {

         Logger.InfoStart();

         PedidoCompraViewModel model = JsonSerializer.Deserialize<PedidoCompraViewModel>(jsonModel);

         model.PedidoCompraDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PedidoCompraDTO.OrderBy = "pc.fecha desc,pc.pedido_co desc";

         model.PedidoCompraDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.PedidoCompraDTO.EstadoAprobacionDTO = new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.APROBADO };
         model.PedidoCompraDTO.EstadoPedidoCompraDTO = new EstadoPedidoCompraDTO() { Codigo = EstadoPedidoCompraConstante.PEDIDO };
         model.PedidoCompraDTO.Estado = EstadoConstante.ACTIVO;

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
                            f.ProveedorDTO.CondicionPagoId,
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
      [HttpPost("GrabarDesdeStockMinimo")]
      public ActionResult GrabarDesdeStockMinimo([FromBody] PedidoCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.EditAction = ViewModelEditAction.New;

            foreach (PedidoCompraProductoDTO pedidoCompraProductoDTO in model.PedidoCompraProductoDTOList)
            {
               pedidoCompraProductoDTO.StockEnProveedor = SiNoConstante.S;
               pedidoCompraProductoDTO.Estado = EstadoConstante.ACTIVO;
               AuditDTO(pedidoCompraProductoDTO);
            }

            PedidoCompraDTO pedidoCompraDTO = cargarPedidoCompraDTO(model);

            pedidoCompraDTO.Fecha = DateTime.Today;

            ProveedorDTO proveedorDTO = null;
            if (model.PedidoCompraDTO.ProveedorId != 0)
            {
               proveedorDTO = proveedorProxy.GetById(new ProveedorDTO()
               {
                  ProveedorId = model.PedidoCompraDTO.ProveedorId,
               });
            }
            else
            {
               proveedorDTO = proveedorProxy.SingleSearch(new ProveedorDTO()
               {
                  EmpresaId = pedidoCompraDTO.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
                  Codigo = ProveedorConstante.GENERICO
               });
            }

            pedidoCompraDTO.ProveedorDTO = proveedorDTO;
            pedidoCompraDTO.ProveedorId = proveedorDTO.Id;

            TipoProductoDTO tipoProductoDTO = tipoProductoProxy.ObtenerPorCodigo(new TipoProductoDTO() { Codigo = TipoProductoConstante.PRODUCTO });
            pedidoCompraDTO.TipoProductoId = tipoProductoDTO.Id;
            pedidoCompraDTO.TipoProductoDTO = tipoProductoDTO;

            pedidoCompraDTO.PedidoCompraProductoDTOList = model.PedidoCompraProductoDTOList;

            int newId = pedidoCompraProxy.AgregarDesdeStockMinimo(pedidoCompraDTO);

            resultResponse.Data = new
            {
               Id = newId
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
      [HttpPost("Solicitar")]
      public ActionResult Solicitar([FromBody] PedidoCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PedidoCompraDTO pedidoCompraDTO = cargarPedidoCompraDTO(model);

            int newId = model.PedidoCompraDTO.Id;
            string messageResourceKey = "CM_pedido_compra_aprobacion_solicitada";

            pedidoCompraProxy.SolicitarAutorizacion(pedidoCompraDTO);

            pedidoCompraDTO = pedidoCompraProxy.GetById(new PedidoCompraDTO() { PedidoCompraId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, pedidoCompraDTO.Codigo);
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
      [HttpPost("SolicitarModificacion")]
      public ActionResult SolicitarModificacion([FromBody] PedidoCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PedidoCompraDTO pedidoCompraDTO = cargarPedidoCompraDTO(model);

            int newId = model.PedidoCompraDTO.Id;
            string messageResourceKey = "CM_pedido_compra_modificacion_solicitada";

            pedidoCompraProxy.SolicitarModificacion(pedidoCompraDTO);

            pedidoCompraDTO = pedidoCompraProxy.GetById(new PedidoCompraDTO() { PedidoCompraId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, pedidoCompraDTO.Codigo);
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

      [HttpPost("GrabarProductoAsociado")]
      public ActionResult GrabarProductoAsociado([FromBody] PedidoCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.PedidoCompraProductoDTO);
            pedidoCompraProductoProxy.ActualizarProducto(model.PedidoCompraProductoDTO);

            resultResponse.Message = SR.GetString("IV_producto_actualizado");
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