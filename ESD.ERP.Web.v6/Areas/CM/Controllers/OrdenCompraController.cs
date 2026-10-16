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
   public class OrdenCompraController : AuthorizedController
   {
      private readonly IOrdenCompraProxy ordenCompraProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly ITipoProductoProxy tipoProductoProxy;
      private readonly IEstadoOrdenCompraProxy estadoOrdenCompraProxy;
      private readonly ICondicionPagoProxy condicionPagoProxy;
      private readonly IProveedorProxy proveedorProxy;
      private readonly IOrdenCompraProductoProxy ordenCompraProductoProxy;
      private readonly IImpuestoTasaProxy impuestoTasaProxy;
      private readonly IOrdenCompraEntregaProxy ordenCompraEntregaProxy;
      private readonly ITipoCambioDiaProxy tipoCambioDiaProxy;
      private readonly IProveedorListaPrecioProxy proveedorListaPrecioProxy;
      private readonly IProveedorListaPrecioProductoProxy proveedorListaPrecioProductoProxy;
      private readonly AppHelper appHelper;

      public OrdenCompraController(
         IOrdenCompraProxy ordenCompraProxy,
         IMonedaProxy monedaProxy,
         ITipoProductoProxy tipoProductoProxy,
         IEstadoOrdenCompraProxy estadoOrdenCompraProxy,
         ICondicionPagoProxy condicionPagoProxy,
         IProveedorProxy proveedorProxy,
         IOrdenCompraProductoProxy ordenCompraProductoProxy,
         IImpuestoTasaProxy impuestoTasaProxy,
         IOrdenCompraEntregaProxy ordenCompraEntregaProxy,
         ITipoCambioDiaProxy tipoCambioDiaProxy,
         IProveedorListaPrecioProxy proveedorListaPrecioProxy,
         IProveedorListaPrecioProductoProxy proveedorListaPrecioProductoProxy,
         AppHelper appHelper
         )
      {
         this.ordenCompraProxy = ordenCompraProxy;
         this.monedaProxy = monedaProxy;
         this.tipoProductoProxy = tipoProductoProxy;
         this.estadoOrdenCompraProxy = estadoOrdenCompraProxy;
         this.condicionPagoProxy = condicionPagoProxy;
         this.proveedorProxy = proveedorProxy;
         this.ordenCompraProductoProxy = ordenCompraProductoProxy;
         this.impuestoTasaProxy = impuestoTasaProxy;
         this.ordenCompraEntregaProxy = ordenCompraEntregaProxy;
         this.tipoCambioDiaProxy = tipoCambioDiaProxy;
         this.proveedorListaPrecioProxy = proveedorListaPrecioProxy;
         this.proveedorListaPrecioProductoProxy = proveedorListaPrecioProductoProxy;
         this.appHelper = appHelper;
      }

      [Route("[action]")]
      public IActionResult Index()
      {
         OrdenCompraViewModel ordenCompraViewModel = new();

         cargarListaTipoProducto(ordenCompraViewModel);
         cargarListaEstadoOrdenCompra(ordenCompraViewModel);

         MenuBarTagHelper menuBarTagHelper = new();

         ordenCompraViewModel.MenuItemList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.CRUDAndPreview);

         ordenCompraViewModel.MenuItemList.Add(new MenuItem()
         {
            Key = "cambiar_estado",
            Content = "Cambiar Estado",
            Position = 3
         });

         return PartialView(ordenCompraViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         OrdenCompraViewModel model = JsonSerializer.Deserialize<OrdenCompraViewModel>(jsonModel);

         model.OrdenCompraDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenCompraDTO.OrderBy = "oc.fecha desc,oc.orden_co desc";

         model.OrdenCompraDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         
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
                            f.MonedaId,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.MontoBruto,
                            f.MontoImpuesto,
                            f.MontoTotal,
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
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         OrdenCompraViewModel ordenCompraViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true,
         };

         try
         {
            cargarDataParaEditar(ordenCompraViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, ordenCompraViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] OrdenCompraViewModel model)
      {

         Logger.InfoStart();

         OrdenCompraViewModel ordenCompraViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(ordenCompraViewModel, model.OrdenCompraDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, ordenCompraViewModel);
      }
      private void cargarDataParaEditar(OrdenCompraViewModel model, int id)
      {

         OrdenCompraDTO ordenCompraDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            ordenCompraDTO = new OrdenCompraDTO()
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

            ordenCompraDTO.ProveedorDTO = proveedorDTO;
            ordenCompraDTO.ProveedorId = proveedorDTO.Id;

            MonedaDTO monedaDTO = monedaProxy.ObtenerPorCodigo(new MonedaDTO() { Codigo = MonedaConstante.DEFAULT });
            ordenCompraDTO.MonedaId = monedaDTO.Id;
            ordenCompraDTO.MonedaDTO = monedaDTO;

            TipoProductoDTO tipoProductoDTO = tipoProductoProxy.ObtenerPorCodigo(new TipoProductoDTO() {Codigo = TipoProductoConstante.PRODUCTO});
            ordenCompraDTO.TipoProductoId = tipoProductoDTO.Id;
            ordenCompraDTO.TipoProductoDTO = tipoProductoDTO;

            ImpuestoTasaDTO impuestoTasaDTO = impuestoTasaProxy.SingleSearch(new ImpuestoTasaDTO()
            {
               TipoImpuestoDTO = new TipoImpuestoDTO() { Codigo = TipoImpuestoConstante.IGV },
               Periodo = periodoEmpresaSedeDTO.PeriodoDTO.Codigo
            });
            ordenCompraDTO.ImpuestoTasaDTO = impuestoTasaDTO;
            ordenCompraDTO.ImpuestoTasaId = impuestoTasaDTO.Id;

            EstadoOrdenCompraDTO estadoOrdenCompraDTO = estadoOrdenCompraProxy.ObtenerPorCodigo(new EstadoOrdenCompraDTO() { Codigo = EstadoOrdenCompraConstante.EN_EDICION});
            ordenCompraDTO.EstadoOrdenCompraId = estadoOrdenCompraDTO.Id;
            ordenCompraDTO.EstadoOrdenCompraDTO = estadoOrdenCompraDTO;

            CondicionPagoDTO condicionPagoDTO = condicionPagoProxy.SingleSearch(new CondicionPagoDTO()
            {
               EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
               Codigo = CondicionPagoConstante.CONTADO
            });
            ordenCompraDTO.CondicionPagoId = condicionPagoDTO.Id;
         }
         else
         {
            ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId  =  id });
            model.EsFechaEntrega = ordenCompraDTO.FechaEntrega.HasValue;
         }

         model.OrdenCompraDTO = ordenCompraDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.MonedaList, ordenCompraDTO.MonedaId);
         WebHelper.SelectedListItemSetValue(model.TipoProductoList, ordenCompraDTO.TipoProductoId);
         WebHelper.SelectedListItemSetValue(model.ImpuestoTasaList, ordenCompraDTO.ImpuestoTasaId);
         WebHelper.SelectedListItemSetValue(model.CondicionPagoList, ordenCompraDTO.CondicionPagoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { Key="solicitar", Content="Solicitar Aprobación" },
            new MenuItem() { Key="modificar", Content="Solicitar Modificación", Disabled = true },
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };

      }
      private void cargarListas(OrdenCompraViewModel model)
      {
         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<CondicionPagoDTO> condicionPagoDTOList = condicionPagoProxy.Search(new CondicionPagoDTO() { 
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
      private void cargarListaTipoProducto(OrdenCompraViewModel model)
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
      private void cargarListaEstadoOrdenCompra(OrdenCompraViewModel model)
      {
         List<EstadoOrdenCompraDTO> estadoOrdenCompraDTOList = estadoOrdenCompraProxy.Search(new EstadoOrdenCompraDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.EstadoOrdenCompraList = WebHelper.ToSelectListItem(estadoOrdenCompraDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("ObtenerTipoCambio")]
      public JsonResult ObtenerTipoCambio([FromBody] OrdenCompraViewModel model)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            MonedaDTO monedaDTO = monedaProxy.GetById(new MonedaDTO()
            {
               MonedaId = model.OrdenCompraDTO.MonedaId
            });

            decimal montoTipoCambio = decimal.Zero;
            if (monedaDTO.EsLocal == SiNoConstante.N)
            {
               TipoCambioDiaDTO tipoCambioDiaDTO = tipoCambioDiaProxy.SingleSearch(new TipoCambioDiaDTO()
               {
                  TipoCambioDTO = new TipoCambioDTO() { Codigo = TipoCambioConstante.OFICIAL },
                  MonedaId = model.OrdenCompraDTO.MonedaId,
                  Fecha = model.OrdenCompraDTO.Fecha,
                  Estado = EstadoConstante.ACTIVO
               });

               if (tipoCambioDiaDTO == null)
                  throw new Exception(SR.GetString("MG_tipo_cambio_dia_no_registrado", model.OrdenCompraDTO.Fecha.ToShortDateString()));

               montoTipoCambio = tipoCambioDiaDTO.MontoVenta;
            }

            resultResponse.Data = new
            {
               monedaDTO.EsLocal,
               MontoTipoCambio = montoTipoCambio,
            };
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse);
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
      [HttpGet("ObtenerImpuestoTasa")]
      public JsonResult ObtenerImpuestoTasa(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            ImpuestoTasaDTO impuestoTasaDTO = impuestoTasaProxy.GetById(new ImpuestoTasaDTO() { ImpuestoTasaId = id });

            resultResponse.Data = new
            {
               impuestoTasaDTO.Tasa
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
      [HttpGet("ObtenerTotales")]
      public JsonResult ObtenerTotales(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            OrdenCompraDTO ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId = id });
            if (ordenCompraDTO == null)
               ordenCompraDTO = new OrdenCompraDTO();

            resultResponse.Data = new
            {
               ordenCompraDTO.MontoBruto,
               ordenCompraDTO.MontoImpuesto,
               ordenCompraDTO.MontoTotal
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
      public ActionResult Grabar([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            OrdenCompraDTO  ordenCompraDTO = cargarOrdenCompraDTO(model);

            validarOrdenCompra(ordenCompraDTO, 0);

            int newId = model.OrdenCompraDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = ordenCompraProxy.Insert(ordenCompraDTO);
                  messageResourceKey = "CM_orden_compra_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  ordenCompraProxy.Update(ordenCompraDTO);
                  messageResourceKey = "CM_orden_compra_actualizada";
                  break;
            }

            ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, ordenCompraDTO.Codigo);
            resultResponse.Data = new
            {
               ordenCompraDTO.Id,
               ordenCompraDTO.Codigo 
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
      private void validarOrdenCompra(OrdenCompraDTO ordenCompraDTO, int ordenCompraId)
      {
         if (ordenCompraDTO == null)
            ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId = ordenCompraId });

         if (ordenCompraDTO.MonedaDTO.EsLocal == SiNoConstante.N)
         {
            if (ordenCompraDTO.TipoCambio == decimal.Zero)
               throw new Exception(SR.GetString("GEN_tipo_cambio_monto_requerido"));
         }
      }
      private OrdenCompraDTO cargarOrdenCompraDTO(OrdenCompraViewModel model)
      {

         OrdenCompraDTO ordenCompraDTO;
         try
         {

            ordenCompraDTO = model.OrdenCompraDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

               ordenCompraDTO = model.OrdenCompraDTO;

               ordenCompraDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;
               ordenCompraDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;

               MonedaDTO monedaDTO = monedaProxy.GetById(new MonedaDTO() { MonedaId = model.OrdenCompraDTO.MonedaId });
               ordenCompraDTO.MonedaDTO = monedaDTO;

               EstadoOrdenCompraDTO estadoOrdenCompraDTO = estadoOrdenCompraProxy.ObtenerPorCodigo(new EstadoOrdenCompraDTO()
               {
                  Codigo = EstadoOrdenCompraConstante.EN_EDICION 
               });
               ordenCompraDTO.EstadoOrdenCompraDTO = estadoOrdenCompraDTO;
               ordenCompraDTO.EstadoOrdenCompraId = estadoOrdenCompraDTO.Id;
               ordenCompraDTO.PermitePagoParcial = SiNoConstante.S;
               ordenCompraDTO.EsModificacion = SiNoConstante.N;
            }
            else
            {
               ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId = model.OrdenCompraDTO.Id});

               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
               model.OrdenCompraDTO.PeriodoEmpresaSedeId = ordenCompraDTO.PeriodoEmpresaSedeId;
               model.OrdenCompraDTO.ImpuestoTasaId = ordenCompraDTO.ImpuestoTasaId;
               model.OrdenCompraDTO.TipoProductoId = ordenCompraDTO.TipoProductoId;
               model.OrdenCompraDTO.MonedaDTO = ordenCompraDTO.MonedaDTO;
               model.OrdenCompraDTO.EsModificacion = ordenCompraDTO.EsModificacion;
               model.OrdenCompraDTO.EstadoAprobacionDTO = ordenCompraDTO.EstadoAprobacionDTO;
               model.OrdenCompraDTO.EstadoAprobacionId = ordenCompraDTO.EstadoAprobacionId;
               model.OrdenCompraDTO.EstadoOrdenCompraAnteriorId = ordenCompraDTO.EstadoOrdenCompraAnteriorId;
               model.OrdenCompraDTO.EstadoOrdenCompraDTO = ordenCompraDTO.EstadoOrdenCompraDTO;
               model.OrdenCompraDTO.EstadoOrdenCompraId = ordenCompraDTO.EstadoOrdenCompraId;  

               ordenCompraDTO = model.OrdenCompraDTO;
            }

            ordenCompraDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(ordenCompraDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return ordenCompraDTO;
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.OrdenCompraDTO.DTOState = DTOState.Modified;

            AuditDTO(model.OrdenCompraDTO);
            ordenCompraProxy.Delete(model.OrdenCompraDTO);

            OrdenCompraDTO ordenCompraDTO = ordenCompraProxy.GetById(model.OrdenCompraDTO);

            resultResponse.Message = SR.GetString("CM_orden_compra_anulada", ordenCompraDTO.Codigo);
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
      [HttpPost("Solicitar")]
      public ActionResult Solicitar([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            OrdenCompraDTO ordenCompraDTO = cargarOrdenCompraDTO(model);

            validarOrdenCompra(ordenCompraDTO, 0);

            int newId = model.OrdenCompraDTO.Id;
            string messageResourceKey = "CM_orden_compra_aprobacion_solicitada";

            ordenCompraProxy.SolicitarAutorizacion(ordenCompraDTO);

            ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, ordenCompraDTO.Codigo);
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
      public ActionResult SolicitarModificacion([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            OrdenCompraDTO ordenCompraDTO = cargarOrdenCompraDTO(model);

            validarOrdenCompra(ordenCompraDTO, 0);

            int newId = model.OrdenCompraDTO.Id;
            string messageResourceKey = "CM_orden_compra_modificacion_solicitada";

            ordenCompraProxy.SolicitarModificacion(ordenCompraDTO);

            ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, ordenCompraDTO.Codigo);
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

      [HttpPost("ListarOrdenCompraProducto")]
      public ActionResult ListarOrdenCompraProducto(string jsonModel)
      {

         Logger.InfoStart();

         OrdenCompraViewModel model = JsonSerializer.Deserialize<OrdenCompraViewModel>(jsonModel);

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
                            f.ProductoDTO.CodigoReferencial,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            TipoProducto = f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO.Codigo,
                            f.ProductoDTO.UnidadMedidaDTO.UnidadMedidaId,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            TipoIgv = f.ProductoDTO.TipoIgvDTO.Codigo,
                            f.Descripcion,
                            f.Cantidad,
                            f.CantidadIngresada,
                            f.ValorUnitario,
                            f.PrecioUnitario,
                            f.MontoBruto,
                            f.MontoImpuesto,
                            f.MontoTotal,
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
      [HttpPost("CalcularMontos")]
      public ActionResult CalcularMontos([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(model.CalculadorVentaItemDTO, 4);

            resultResponse.Data = new
            {
               calculadorVentaItemDTO.PrecioUnitario,
               calculadorVentaItemDTO.ValorUnitario,
               calculadorVentaItemDTO.MontoBruto,
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
      [HttpPost("GrabarOrdenCompraProducto")]
      public ActionResult GrabarOrdenCompraProducto([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            OrdenCompraProductoDTO ordenCompraProductoDTO = cargarOrdenCompraProductoDTO(model);

            int newId = model.OrdenCompraProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (ordenCompraProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = ordenCompraProductoProxy.Insert(ordenCompraProductoDTO);
                  messageResourceKey = "IV_producto_agregado";
                  break;

               case DTOState.Modified:
                  ordenCompraProductoProxy.Update(ordenCompraProductoDTO);
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
      private OrdenCompraProductoDTO cargarOrdenCompraProductoDTO(OrdenCompraViewModel model)
      {

         OrdenCompraProductoDTO ordenCompraProductoDTO;
         try
         {

            ordenCompraProductoDTO = model.OrdenCompraProductoDTO;

            AuditDTO(ordenCompraProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return ordenCompraProductoDTO;
      }
      [HttpPost("EliminarOrdenCompraProducto")]
      public ActionResult EliminarOrdenCompraProducto([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.OrdenCompraProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.OrdenCompraProductoDTO);

            OrdenCompraProductoDTO ordenCompraProductoDTO = ordenCompraProductoProxy.GetById(model.OrdenCompraProductoDTO);

            ordenCompraProductoProxy.Delete(ordenCompraProductoDTO);

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
      [HttpPost("ObtenerPrecio")]
      public ActionResult ObtenerPrecio([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            ProveedorListaPrecioDTO proveedorListaPrecioDTO = proveedorListaPrecioProxy.SingleSearch(new ProveedorListaPrecioDTO()
            {
               ProveedorId = model.OrdenCompraProductoDTO.OrdenCompraDTO.ProveedorId,
               MonedaId = model.OrdenCompraProductoDTO.OrdenCompraDTO.MonedaId,
               Estado = EstadoConstante.ACTIVO
            });

            ProveedorListaPrecioProductoDTO proveedorListaPrecioProductoDTO = null;
            if (proveedorListaPrecioDTO != null)
               proveedorListaPrecioProductoDTO = proveedorListaPrecioProductoProxy.SingleSearch(
                  new ProveedorListaPrecioProductoDTO()
                  {
                     ProveedorListaPrecioId = proveedorListaPrecioDTO.Id,
                     ProductoId = model.OrdenCompraProductoDTO.ProductoId,
                     Estado = EstadoConstante.ACTIVO
                  }
               );

            CalculadorVentaItemDTO calculadorVentaItemDTO = new();
            if (proveedorListaPrecioProductoDTO != null)
            {
               model.CalculadorVentaItemDTO.PrecioUnitario = proveedorListaPrecioProductoDTO.PrecioUnitario;
               model.CalculadorVentaItemDTO.TipoDescuento = TipoDescuentoConstante.SIN_DESCUENTO;
               calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(model.CalculadorVentaItemDTO, 4);
            }

            resultResponse.Data = new
            {
               calculadorVentaItemDTO.PrecioUnitario,
               calculadorVentaItemDTO.ValorUnitario,
               calculadorVentaItemDTO.MontoBruto,
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
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string ordenCompraId)
      {
         Logger.InfoStart();

         byte[] reportData = obtenerReporteData(ordenCompraId);

         OrdenCompraDTO ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId = Convert.ToInt32(ordenCompraId) });

         string reportPrefix = string.Empty;
         if (ordenCompraDTO.TipoProductoDTO.Codigo == TipoProductoConstante.PRODUCTO)
            reportPrefix = "OC";
         else
            reportPrefix = "OS";

         return appHelper.ReportToPdf(reportData, string.Concat(reportPrefix, ordenCompraDTO.Codigo));
      }
      private byte[] obtenerReporteData(string ordenCompraId)
      {
         Dictionary<string, string> parameters = new()
         {
            { "ordenCompraId", ordenCompraId }
         };

         byte[] reportData = appHelper.ExecuteReport("CM_OrdenCompra", parameters, ReportFormatConstant.PDF);

         return reportData;
      }
      [HttpPost("ListPopup")]
      public ActionResult ListPopup([FromBody] OrdenCompraViewModel model)
      {

         if (model == null)
            model = new OrdenCompraViewModel();

         model.ModalEditId = GetModalEditPanelId();

         return PartialView(model);
      }
      [Route("OrdenCompraProductoListPopup")]
      public IActionResult OrdenCompraProductoListPopup([FromBody] OrdenCompraViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("ListarOrdenCompraProductoPopup")]
      public ActionResult ListarOrdenCompraProductoPopup(string jsonModel)
      {

         Logger.InfoStart();

         OrdenCompraViewModel model = JsonSerializer.Deserialize<OrdenCompraViewModel>(jsonModel);

         model.OrdenCompraProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenCompraProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.OrdenCompraProductoDTO.ConCantidadPendiente = SiNoConstante.S;
         model.OrdenCompraProductoDTO.Estado = EstadoConstante.ACTIVO;

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
                            f.ProductoDTO.CodigoReferencial,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            TipoProducto = f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO.Codigo,
                            f.ProductoDTO.UnidadMedidaDTO.UnidadMedidaId,
                            UnidadMedidaNombre = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            f.Descripcion,
                            f.CantidadPendiente,
                            f.ValorUnitario,
                            f.PrecioUnitario,
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
      [HttpPost("ListarOrdenCompraEntrega")]
      public ActionResult ListarOrdenCompraEntrega(string jsonModel)
      {

         Logger.InfoStart();

         OrdenCompraViewModel model = JsonSerializer.Deserialize<OrdenCompraViewModel>(jsonModel);

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
                            Fecha = f.Fecha == null ? string.Empty :  f.Fecha.Value.ToShortDateString(),
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
      [HttpPost("GrabarOrdenCompraEntrega")]
      public ActionResult GrabarOrdenCompraEntrega([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            OrdenCompraEntregaDTO ordenCompraEntregaDTO = cargarOrdenCompraEntregaDTO(model);

            int newId = model.OrdenCompraEntregaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (ordenCompraEntregaDTO.DTOState)
            {
               case DTOState.Added:
                  newId = ordenCompraEntregaProxy.Insert(ordenCompraEntregaDTO);
                  messageResourceKey = "CM_entrega_agregada";
                  break;

               case DTOState.Modified:
                  ordenCompraEntregaProxy.Update(ordenCompraEntregaDTO);
                  messageResourceKey = "CM_entrega_actualizada";
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
      private OrdenCompraEntregaDTO cargarOrdenCompraEntregaDTO(OrdenCompraViewModel model)
      {

         OrdenCompraEntregaDTO ordenCompraEntregaDTO;
         try
         {

            ordenCompraEntregaDTO = model.OrdenCompraEntregaDTO;

            AuditDTO(ordenCompraEntregaDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return ordenCompraEntregaDTO;
      }
      [HttpPost("EliminarOrdenCompraEntrega")]
      public ActionResult EliminarOrdenCompraEntrega([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.OrdenCompraEntregaDTO.DTOState = DTOState.Modified;
            AuditDTO(model.OrdenCompraEntregaDTO);

            OrdenCompraEntregaDTO ordenCompraEntregaDTO = ordenCompraEntregaProxy.GetById(model.OrdenCompraEntregaDTO);

            ordenCompraEntregaProxy.Delete(ordenCompraEntregaDTO);

            resultResponse.Message = SR.GetString("CM_entrega_anulada");
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
      [HttpPost("AgregarEditPopup")]
      public ActionResult AgregarEditPopup([FromBody] PedidoCompraViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("AgregarTodosProductos")]
      public ActionResult AgregarTodosProductos([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ordenCompraEntregaProxy.AgregarDesdeOrdenCompraProducto(model.OrdenCompraEntregaDTO);

            resultResponse.Message = SR.GetString("CM_productos_entregables_agregados");
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
      [HttpPost("CambiarEstadoEditPopup")]
      public IActionResult CambiarEstadoEditPopup([FromBody] OrdenCompraViewModel model)
      {
         OrdenCompraDTO ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId = model.OrdenCompraDTO.OrdenCompraId });

         string estadoCotizacion = ordenCompraDTO.EstadoOrdenCompraDTO.Codigo;
         List<string> estadoOrdenCompraList = new List<string>() { EstadoOrdenCompraConstante.ATENDIDA };

         List<EstadoOrdenCompraDTO> estadoOrdenCompraDTOList = estadoOrdenCompraProxy.Search(new EstadoOrdenCompraDTO()
         {
            EstadoOrdenCompraList = estadoOrdenCompraList,
            Estado = EstadoConstante.ACTIVO
         }); ;
         model.EstadoOrdenCompraList = WebHelper.ToSelectListItem(estadoOrdenCompraDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Empty);

         return PartialView(model);
      }
      [HttpPost("CambiarEstado")]
      public ActionResult CambiarEstado([FromBody] OrdenCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.OrdenCompraDTO);

            ordenCompraProxy.CambiarEstado(model.OrdenCompraDTO);

            resultResponse.Message = SR.GetString("CM_orden_compra_estado_actualizado");
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