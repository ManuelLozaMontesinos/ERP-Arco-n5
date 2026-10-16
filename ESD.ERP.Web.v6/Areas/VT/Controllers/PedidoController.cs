using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class PedidoController : AuthorizedController
   {
      private readonly IEstadoCotizacionProxy estadoCotizacionProxy;
      private readonly ICotizacionProxy cotizacionProxy;
      private readonly ICotizacionProductoProxy cotizacionProductoProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IListaPrecioProxy listaPrecioProxy;
      private readonly IVendedorProxy vendedorProxy;
      private readonly IImpuestoTasaProxy impuestoTasaProxy;
      private readonly ITipoAfectacionIgvProxy tipoAfectacionIgvProxy;
      private readonly IParametroSistemaProxy parametroSistemaProxy;
      private readonly ITipoCondicionPagoProxy tipoCondicionPagoProxy;
      private readonly ITipoAutorizacionModuloProxy tipoAutorizacionModuloProxy;
      private readonly ICotizacionAutorizacionProxy cotizacionAutorizacionProxy;
      private readonly IAnticipoClienteProxy anticipoClienteProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy;
      private readonly IListaPrecioProveedorProxy listaPrecioProveedorProxy;
      private readonly IGrupoProveedorItemProxy grupoProveedorItemProxy;
      private readonly IComboProductoProxy comboProductoProxy;
      private readonly IOrdenProduccionProxy ordenProduccionProxy;
      private readonly IOrdenProduccionSubProcesoProxy ordenProduccionSubProcesoProxy;
      private readonly ITipoDescuentoProxy tipoDescuentoProxy;
      private readonly IProductoDetalleProxy productoDetalleProxy;
      private readonly AppHelper appHelper;

      public PedidoController(
         IEstadoCotizacionProxy estadoCotizacionProxy,
         ICotizacionProxy cotizacionProxy,
         ICotizacionProductoProxy cotizacionProductoProxy,
         IMonedaProxy monedaProxy,
         IListaPrecioProxy listaPrecioProxy,
         IVendedorProxy vendedorProxy,
         IImpuestoTasaProxy impuestoTasaProxy,
         ITipoAfectacionIgvProxy tipoAfectacionIgvProxy,
         IParametroSistemaProxy parametroSistemaProxy,
         ITipoCondicionPagoProxy tipoCondicionPagoProxy,
         ITipoAutorizacionModuloProxy tipoAutorizacionModuloProxy,
         ICotizacionAutorizacionProxy cotizacionAutorizacionProxy,
         IAnticipoClienteProxy anticipoClienteProxy,
         IMesContableProxy mesContableProxy,
         IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy,
         IListaPrecioProveedorProxy listaPrecioProveedorProxy,
         IGrupoProveedorItemProxy grupoProveedorItemProxy,
         IComboProductoProxy comboProductoProxy,
         IOrdenProduccionProxy ordenProduccionProxy,
         IOrdenProduccionSubProcesoProxy ordenProduccionSubProcesoProxy,
         ITipoDescuentoProxy tipoDescuentoProxy,
         IProductoDetalleProxy productoDetalleProxy,
         AppHelper appHelper
         )
      {
         this.estadoCotizacionProxy = estadoCotizacionProxy;
         this.cotizacionProxy = cotizacionProxy;
         this.cotizacionProductoProxy = cotizacionProductoProxy;
         this.monedaProxy = monedaProxy;
         this.listaPrecioProxy = listaPrecioProxy;
         this.vendedorProxy = vendedorProxy;
         this.impuestoTasaProxy = impuestoTasaProxy;
         this.tipoAfectacionIgvProxy = tipoAfectacionIgvProxy;
         this.parametroSistemaProxy = parametroSistemaProxy;
         this.tipoCondicionPagoProxy = tipoCondicionPagoProxy;
         this.tipoAutorizacionModuloProxy = tipoAutorizacionModuloProxy;
         this.cotizacionAutorizacionProxy = cotizacionAutorizacionProxy;
         this.anticipoClienteProxy = anticipoClienteProxy;
         this.mesContableProxy = mesContableProxy;
         this.movimientoAlmacenSaldoProxy = movimientoAlmacenSaldoProxy;
         this.listaPrecioProveedorProxy = listaPrecioProveedorProxy;
         this.grupoProveedorItemProxy = grupoProveedorItemProxy;
         this.comboProductoProxy = comboProductoProxy;
         this.ordenProduccionProxy = ordenProduccionProxy;
         this.ordenProduccionSubProcesoProxy = ordenProduccionSubProcesoProxy;
         this.tipoDescuentoProxy = tipoDescuentoProxy;
         this.productoDetalleProxy = productoDetalleProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         PedidoViewModel pedidoViewModel = new();

         cargarListaEstadoCotizacion(pedidoViewModel);

         pedidoViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Edit },
            new MenuItem() { Key = "cambiar_cotizacion", Content = "Cambiar a Cotizacion" },
            new MenuItem() { MenuItemType = MenuItemType.Preview },
            new MenuItem() { Key = "reporte_anticipo", Content = "Imprimir Recibo" },
            new MenuItem() { Key = "consulta", Content = "Consulta de Precios" }
         };

         if(UsuarioEnRol(new List<string>() { RolConstante.SYS_ADMIN, "00011", "00016", "00020" })){
            pedidoViewModel.MenuItemList.Add(new MenuItem() { Key = "cambiar_estado", Content = "Cambiar Estado" });
         }

         AplicarPermisosMenu(pedidoViewModel.MenuItemList);

         configurarServicioImpresion(pedidoViewModel);

         if (!EsAdministrador())
         {
            VendedorDTO vendedorDTO = vendedorProxy.SingleSearch(new VendedorDTO()
            {
               ProveedorDTO = new ProveedorDTO()
               {
                  EmpresaId = ObtenerEmpresaId(),
                  UsuarioId = ObtenerUsuarioId()
               }
            });
            if (vendedorDTO != null)
               pedidoViewModel.Vendedor = vendedorDTO.ProveedorDTO.RazonSocial;
         }

         return PartialView(pedidoViewModel);
      }

        [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PedidoViewModel model = JsonSerializer.Deserialize<PedidoViewModel>(jsonModel);

         model.CotizacionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CotizacionDTO.OrderBy = "c.fecha desc,c.cotiza desc";

         model.CotizacionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.CotizacionDTO.EsPedido = SiNoConstante.S;

         try
         {
            List<CotizacionDTO> cotizacionDTOList = cotizacionProxy.Search(model.CotizacionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = cotizacionDTOList.Count,
               rows = from f in cotizacionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.CotizacionId,
                            f.Codigo,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.ListaPrecioId,
                            f.ProveedorId,
                            Proveedor=f.ProveedorDTO.Codigo,
                            f.ProveedorDTO.RazonSocial,
                            TipoNaturalezaJuridica = f.ProveedorDTO.TipoNaturalezaJuridicaDTO.Codigo,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            f.VendedorId,
                            VendedorNombre = f.VendedorDTO.ProveedorDTO.RazonSocial,
                            f.MonedaId,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.TipoCondicionPagoId,
                            f.MontoAnticipo,
                            f.MontoTotal,
                            f.EnviadoProduccion,
                            EstadoCotizacion = f.EstadoCotizacionDTO.Codigo,
                            EstadoCotizacionNombre = f.EstadoCotizacionDTO.Nombre,
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
      public ActionResult Crear([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         PedidoViewModel pedidoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true,
            AutorizacionPendiente = SiNoConstante.N
         };

         try
         {
            cargarDataParaEditar(pedidoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, pedidoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] PedidoViewModel model)
      {

         Logger.InfoStart();

         PedidoViewModel pedidoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(pedidoViewModel, model.CotizacionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, pedidoViewModel);
      }

      private void cargarDataParaEditar(PedidoViewModel model, int id)
      {

         CotizacionDTO cotizacionDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            cotizacionDTO = new CotizacionDTO() {
               Fecha = DateTime.Today,
               ProveedorDTO = new ProveedorDTO(),
               VendedorDTO = new VendedorDTO() { ProveedorDTO = new ProveedorDTO()}
            };

            VendedorDTO vendedorDTO = vendedorProxy.SingleSearch(new VendedorDTO() { 
               ProveedorDTO = new ProveedorDTO()
               {
                  EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
                  UsuarioId = ObtenerUsuarioId()
               }
            });
            if (vendedorDTO != null)
            {
               cotizacionDTO.VendedorDTO = vendedorDTO;
               cotizacionDTO.VendedorId = vendedorDTO.Id;
            }

            MonedaDTO monedaDTO = monedaProxy.ObtenerPorCodigo(new MonedaDTO() { Codigo = MonedaConstante.DEFAULT });
            cotizacionDTO.MonedaId = monedaDTO.Id;
            cotizacionDTO.MonedaDTO = monedaDTO;

            TipoCondicionPagoDTO tipoCondicionPagoDTO = tipoCondicionPagoProxy.ObtenerPorCodigo(new TipoCondicionPagoDTO()
            {
               Codigo = TipoCondicionPagoConstante.CONTADO
            });
            cotizacionDTO.TipoCondicionPagoId = tipoCondicionPagoDTO.Id;

            EstadoCotizacionDTO estadoCotizacionDTO = estadoCotizacionProxy.ObtenerPorCodigo(new EstadoCotizacionDTO() { Codigo = EstadoCotizacionConstante.COTIZACION });
            cotizacionDTO.EstadoCotizacionId = estadoCotizacionDTO.Id;
            cotizacionDTO.EstadoCotizacionDTO = estadoCotizacionDTO;

            model.IgnorarIncrementoFraccion = SiNoConstante.N;
         }
         else
         {
            cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = id });

            model.AutorizacionPendiente = obtenerAutorizacionPendiente(model, id);

            model.ListaPrecioProveedorId = internalObtenerListaPrecioProveedorId(cotizacionDTO.ListaPrecioId,
               cotizacionDTO.ProveedorId);
            model.IgnorarIncrementoFraccion = obtenerIgnorarIncrementoFraccion(cotizacionDTO.ProveedorId);
         }

         decimal cortesGratis = cotizacionProxy.CortesGratisParcial(new CotizacionDTO() { CotizacionId = id });

         cotizacionDTO.CortesGratis = cortesGratis;

         model.CotizacionDTO = cotizacionDTO;

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.SingleSearch(new ParametroSistemaDTO() { 
            Codigo = ParametroSistemaConstante.VENTAS_PORCENTAJE_INCREMENTO_PU_SI_LA_CANTIDAD_ES_FRACCION });

         model.PorcentajeFraccionPU = parametroSistemaDTO.ValorNumerico;

         cargarListas(model);
         configurarServicioImpresion(model);

         if(model.CotizacionDTO.EnviadoProduccion == SiNoConstante.S)
         {
            model.OrdenProduccionId = obtenerOrderProduccionId(id);
         }

         if (model.CotizacionDTO.EnviadoProduccion == SiNoConstante.S)
         {
            CotizacionAutorizacionDTO cotizacionAutorizacionDTO = cotizacionAutorizacionProxy.SingleSearch(new CotizacionAutorizacionDTO()
            {
               CotizacionDTO = new CotizacionDTO() {
                  PeriodoEmpresaSedeDTO = cotizacionDTO.PeriodoEmpresaSedeDTO
               },
               CotizacionId = cotizacionDTO.CotizacionId,
               TipoAutorizacionDTO = new TipoAutorizacionDTO() { Codigo = TipoAutorizacionConstante.PEDIDO_CON_OP_MODIFICAR_ITEMS },
                   EstadoAprobacionDTO = new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.APROBADO }
            });

            model.AutorizacionProduccion = string.Empty;
            if (cotizacionAutorizacionDTO != null)
               model.AutorizacionProduccion = SiNoConstante.S;
         }

         WebHelper.SelectedListItemSetValue(model.MonedaList, cotizacionDTO.MonedaId);
         WebHelper.SelectedListItemSetValue(model.ListaPrecioList, cotizacionDTO.ListaPrecioId);
         WebHelper.SelectedListItemSetValue(model.TipoCondicionPagoList, cotizacionDTO.TipoCondicionPagoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Save, Disabled = (model.AutorizacionPendiente == SiNoConstante.S)  },
            new MenuItem() { MenuItemType = MenuItemType.Exit },
            new MenuItem() { Key="soli_auto", Content="Solicitar Autorizaciones", Hidden = (model.AutorizacionPendiente == SiNoConstante.S) },
            new MenuItem() { Key="estado_auto", Content="Ver Autorizaciones", Hidden = (model.AutorizacionPendiente == SiNoConstante.N) },
            new MenuItem() { Key="enviar_prod", Content="Enviar a Producción"},
            new MenuItem() { Key="reporte_canto", Content="Reporte Tapacanto" }
         };

         MenuBarTagHelper menuBarTagHelper = new();

         model.MenuAnticipoList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.NewEdit);
         model.MenuAnticipoList.Add(new MenuItem() { MenuItemType = MenuItemType.Preview });
         model.MenuAnticipoList.Add(new MenuItem() { Key = "cerrar", Content = "Cerrar Anticipo" });
         model.MenuAnticipoList.Add(new MenuItem() { MenuItemType = MenuItemType.Delete });
         model.MenuAnticipoList.Add(new MenuItem() { Key = "extornar", Content = "Extornar" });

         AplicarPermisosMenu(model.MenuItemList);
         AplicarPermisosMenu(model.MenuAnticipoList);
      }
      private int obtenerOrderProduccionId(int cotizacionId)
      {
         OrdenProduccionDTO ordenProduccionDTO = ordenProduccionProxy.SingleSearch(new OrdenProduccionDTO()
         {
            CotizacionId = NumberUtil.WhenValueIsZero(cotizacionId),
            Estado = EstadoConstante.ACTIVO
         });

         if (ordenProduccionDTO != null)
           return ordenProduccionDTO.Id;

         return 0;
      }
      private string obtenerAutorizacionPendiente(PedidoViewModel model, int id)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<CotizacionAutorizacionDTO> cotizacionAutorizacionDTOList = cotizacionAutorizacionProxy.Search(new CotizacionAutorizacionDTO()
         {
            CotizacionDTO = new CotizacionDTO()
            {
               PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO,
            },
            CotizacionId = id,
            EstadoAprobacionDTO = new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.PENDIENTE_APROBACION }
         });

         return cotizacionAutorizacionDTOList.Count > 0 ? SiNoConstante.S : SiNoConstante.N;
      }
      private void cargarListaEstadoCotizacion(PedidoViewModel model)
      {
         List<EstadoCotizacionDTO> estadoCotizacionDTOList = estadoCotizacionProxy.Search(new EstadoCotizacionDTO() { Estado = EstadoConstante.ACTIVO });
         model.EstadoCotizacionList = WebHelper.ToSelectListItem(estadoCotizacionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListas(PedidoViewModel model)
      {
         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         if (model.CotizacionDTO.MonedaId == 0)
            model.ListaPrecioList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
         else
         {

            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.CotizacionDTO.MonedaId);

            model.ListaPrecioList = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

            if (model.CotizacionDTO.ListaPrecioId == 0)
               if (listaPrecioDTOList.Count > 0)
                  model.CotizacionDTO.ListaPrecioId = (listaPrecioDTOList.Count > 0 ? listaPrecioDTOList[0].Id : 0);
         }

         List<TipoCondicionPagoDTO> tipoCondicionPagoDTOList = tipoCondicionPagoProxy.Search(new TipoCondicionPagoDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoCondicionPagoList = WebHelper.ToSelectListItem(tipoCondicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoDescuentoDTO> tipoDescuentoDTOList = tipoDescuentoProxy.Search(new TipoDescuentoDTO());
         model.TipoDescuentoString = string.Join(SeparadorConstante.PUNTO_COMA, tipoDescuentoDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Abreviatura));
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
      [HttpPost("ListarListaPrecio")]
      public JsonResult ListarListaPrecio([FromBody] PedidoViewModel model)
      {
                
         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            MonedaDTO monedaDTO = monedaProxy.GetById(new MonedaDTO()
            {
               MonedaId = model.CotizacionDTO.MonedaId
            });

            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.CotizacionDTO.MonedaId);

            resultResponse.Data = new
            {
               monedaDTO.EsLocal,
               List = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select)
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
      [HttpGet("ObtenerListaPrecio")]
      public JsonResult ObtenerListaPrecio(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            ListaPrecioDTO listaPrecioDTO = listaPrecioProxy.GetById(new ListaPrecioDTO() { ListaPrecioId = id });

            resultResponse.Data = new
            {
               listaPrecioDTO.ImpuestoTasaDTO.Tasa
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
      [HttpPost("ObtenerListaPrecioProveedorId")]
      public JsonResult ObtenerListaPrecioProveedorId([FromBody] CotizacionViewModel model)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            resultResponse.Data = new
            {
               ListaPrecioProveedorId = internalObtenerListaPrecioProveedorId(model.ListaPrecioProveedorDTO.ListaPrecioId,
                  model.ListaPrecioProveedorDTO.ProveedorId),
               IgnorarIncrementoFraccion = obtenerIgnorarIncrementoFraccion(model.ListaPrecioProveedorDTO.ProveedorId)
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
      private int internalObtenerListaPrecioProveedorId(int listaPrecioId, int proveedorId)
      {
         List<ListaPrecioProveedorDTO> listaPrecioProveedorDTOList = listaPrecioProveedorProxy.ListarPorProveedorId(new ListaPrecioProveedorDTO()
         {
            ListaPrecioId = listaPrecioId,
            ProveedorId = proveedorId
         });

         return listaPrecioProveedorDTOList.Count > 0 ? listaPrecioProveedorDTOList[0].ListaPrecioProveedorId : 0;
      }
      private string obtenerIgnorarIncrementoFraccion(int proveedorId)
      {
         GrupoProveedorItemDTO grupoProveedorItemDTO = grupoProveedorItemProxy.SingleSearch(new GrupoProveedorItemDTO()
         {
            ProveedorId = proveedorId,
            GrupoProveedorDTO = new GrupoProveedorDTO()
            {
               IgnorarIncrementoFraccion = SiNoConstante.S
            }
         });

         return grupoProveedorItemDTO != null ? SiNoConstante.S : SiNoConstante.N;
      }
      [HttpGet("ObtenerTipoDescuento")]
      public JsonResult ObtenerTipoDescuento(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            TipoDescuentoDTO tipoDescuentoDTO = tipoDescuentoProxy.GetById(new TipoDescuentoDTO() { TipoDescuentoId = id });

            resultResponse.Data = new
            {
               tipoDescuentoDTO.Codigo
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
      public ActionResult Grabar([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         if (!TienePermisoEscritura())
         {
            resultResponse.Success = false;
            resultResponse.Message = "No cuenta con permisos de escritura para registrar o modificar pedidos.";
            return Json(resultResponse);
         }

         try
         {
            CotizacionDTO cotizacionDTO = cargarCotizacionDTO(model);

            int newId = model.CotizacionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = cotizacionProxy.Insert(cotizacionDTO);
                  messageResourceKey = "VT_cotizacion_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  cotizacionProxy.Update(cotizacionDTO);
                  messageResourceKey = "VT_cotizacion_actualizada";
                  break;
            }

            cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, cotizacionDTO.Codigo);
            resultResponse.Data = new
            {
               cotizacionDTO.Id,
               cotizacionDTO.Codigo
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
      private CotizacionDTO cargarCotizacionDTO(PedidoViewModel model)
      {

         CotizacionDTO cotizacionDTO;
         try
         {

            cotizacionDTO = model.CotizacionDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               cotizacionDTO = model.CotizacionDTO;
               cotizacionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

               PeriodoDTO periodoDTO = ObtenerPeriodoDTO(model.ViewModelState.CID);

               ImpuestoTasaDTO impuestoTasaDTO = impuestoTasaProxy.SingleSearch(new ImpuestoTasaDTO() { 
                  TipoImpuestoDTO = new TipoImpuestoDTO() { Codigo = TipoImpuestoConstante.IGV },
                  Periodo = periodoDTO.Codigo
               });
               cotizacionDTO.ImpuestoTasaId = impuestoTasaDTO.Id;

               EstadoCotizacionDTO estadoCotizacionDTO = estadoCotizacionProxy.ObtenerPorCodigo(new EstadoCotizacionDTO() { 
                  Codigo = EstadoCotizacionConstante.COTIZACION });
               cotizacionDTO.EstadoCotizacionId = estadoCotizacionDTO.Id;
            }
            else
            {
               cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO(){ CotizacionId = model.CotizacionDTO.Id });

               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
               model.CotizacionDTO.PeriodoEmpresaSedeId = cotizacionDTO.PeriodoEmpresaSedeId;
               model.CotizacionDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;
               model.CotizacionDTO.ImpuestoTasaId = cotizacionDTO.ImpuestoTasaId;
               model.CotizacionDTO.MontoBruto = cotizacionDTO.MontoBruto;
               model.CotizacionDTO.MontoNeto = cotizacionDTO.MontoNeto;
               model.CotizacionDTO.MontoIgv = cotizacionDTO.MontoIgv;
               model.CotizacionDTO.MontoIsc = cotizacionDTO.MontoIsc;
               model.CotizacionDTO.MontoTotal = cotizacionDTO.MontoTotal;
               model.CotizacionDTO.EsPedido = cotizacionDTO.EsPedido;

               cotizacionDTO = model.CotizacionDTO;
            }

            if (string.IsNullOrEmpty(cotizacionDTO.EsPedido))
               cotizacionDTO.EsPedido = SiNoConstante.N;

            cotizacionDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(cotizacionDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return cotizacionDTO;
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         if (!TienePermisoEscritura())
         {
            resultResponse.Success = false;
            resultResponse.Message = "No cuenta con permisos de escritura para anular pedidos.";
            return Json(resultResponse);
         }

         try
         {
            model.CotizacionDTO.DTOState = DTOState.Modified;

            CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(model.CotizacionDTO);

            EstadoCotizacionDTO estadoCotizacionDTO = estadoCotizacionProxy.ObtenerPorCodigo(new EstadoCotizacionDTO()
            {
               Codigo = EstadoCotizacionConstante.ANULADO
            });

            cotizacionDTO.EstadoCotizacionId = estadoCotizacionDTO.Id;

            AuditDTO(cotizacionDTO);

            cotizacionProxy.Delete(cotizacionDTO);

            resultResponse.Message = SR.GetString("VT_cotizacion_anulada", cotizacionDTO.Codigo);
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
        public FileContentResult ReportTo(string format, string cid, string cotizacionId, string incluyePeso, int listaPrecioId)
        {

            byte[] reportData = obtenerReporteData(cotizacionId, incluyePeso, listaPrecioId);

            CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = Convert.ToInt32(cotizacionId) });

            return appHelper.ReportToPdf(reportData, string.Concat("PE", cotizacionDTO.Codigo));
        }
        private byte[] obtenerReporteData(string cotizacionId, string incluyePeso, int ListaPrecioId)
        {
            Dictionary<string, string> parameters = new()
         {
            { "cotizacionId", cotizacionId }
         };

            string reportName = string.Empty;

            switch (incluyePeso)
            {
                case SiNoConstante.N:
                    if (ListaPrecioId == 7)
                    {
                        reportName = "VT_CotizacionPedidoMB";
                    }
                    else
                    {
                        reportName = "VT_CotizacionPedido";
                    }
                    break;
                case SiNoConstante.S:
                    if (ListaPrecioId == 7)
                    {
                        reportName = "VT_CotizacionPedidoMBPeso";
                    }
                    else
                    {
                        reportName = "VT_CotizacionPedidoPeso";
                    }
                    break;
            }

            byte[] reportData = appHelper.ExecuteReport(reportName, parameters, ReportFormatConstant.PDF);

            return reportData;
        }



        [HttpPost("ListarCotizacionProducto")]
      public ActionResult ListarCotizacionProducto(string jsonModel)
      {

         Logger.InfoStart();

         PedidoViewModel model = JsonSerializer.Deserialize<PedidoViewModel>(jsonModel);

         model.CotizacionProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CotizacionProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.CotizacionProductoDTO.NullRegistroComboId = SiNoConstante.S;

         try
         {
            List<CotizacionProductoDTO> cotizacionProductoDTOList = cotizacionProductoProxy.Search(model.CotizacionProductoDTO);
                
            int index = 1;
            var jsonData = new
                
            {

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
                            f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.EsCombo,
                            TipoProducto = f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO.Codigo,
                            UnidadMedidaNombre = f.UnidadMedidaDTO == null ? f.ProductoDTO.UnidadMedidaDTO.Abreviatura : f.UnidadMedidaDTO.Abreviatura,
                            TipoIgv = f.ProductoDTO.TipoIgvDTO.Codigo,
                            f.ProductoDTO.DetalleRequerido,
                            f.UnidadMedidaId,
                            f.Descripcion,
                            f.Cantidad,
                            f.CantidadPresentacion,
                            f.CantidadEquivalente,
                            CantidadVenta = f.CantidadPresentacion == decimal.Zero ? f.Cantidad : f.CantidadPresentacion,
                            f.CantidadParcial,
                            f.ValorUnitarioLista,
                            f.PrecioUnitarioLista,
                            f.ValorUnitario,
                            f.PrecioUnitario,
                            f.CantidadBloqueada,
                            f.IncluyeDescuentoListaPrecio,
                            f.PermiteDescuento,
                            f.SinSaldo,
                            f.SolicitaAutorizacionDescuento,
                            f.MontoBruto,
                            f.TipoDescuentoId,
                            TipoDescuento = f.TipoDescuentoDTO.Codigo,
                            f.DescuentoValor,
                            f.PorcentajeDescuento,
                            f.MontoDescuento1,
                            MontoTotalDescuento = decimal.Round(f.MontoDescuento1 * (1 + decimal.Round(f.CotizacionDTO.ImpuestoTasaDTO.Tasa / 100, 2)), 2),
                            f.MontoNeto,
                            f.MontoIgv,
                            f.MontoTotal,
                            f.PesoNeto,
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
      [HttpGet("ObtenerTotales")]
      public JsonResult ObtenerTotales(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = id });
            if (cotizacionDTO == null)
               cotizacionDTO = new CotizacionDTO();

            resultResponse.Data = new
            {
               cotizacionDTO.MontoBruto,
               cotizacionDTO.MontoNeto,
               cotizacionDTO.MontoIgv,
               cotizacionDTO.MontoTotal,
               cotizacionDTO.MontoAnticipo,
               cotizacionDTO.MontoSaldo
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
      [HttpGet("ListarProductoDetalle")]
      public JsonResult ListarProductoDetalle(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {

            List<ProductoDetalleDTO> productoDetalleDTOList = productoDetalleProxy.Search(new ProductoDetalleDTO() { ProductoId = id });

            resultResponse.Data = productoDetalleDTOList.Select(x => x.Nombre).ToList();
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
      [HttpPost("VerificarSaldoCalcularMontos")]
      public ActionResult VerificarSaldoCalcularMontos([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            GrupoProductoDTO grupoProductoDTO = model.CotizacionProductoDTO.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO
               .GrupoProductoDTO;

            if (grupoProductoDTO.TipoProductoDTO.Codigo == TipoProductoConstante.PRODUCTO)
            {
               MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO = new();
               DateTime fecha = DateTime.Today;

               if (model.CotizacionProductoDTO.Id == 0)
               {
                  CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = model.CotizacionProductoDTO.CotizacionId });

                  fecha = cotizacionDTO.Fecha;

                  movimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId = cotizacionDTO.PeriodoEmpresaSedeId;
               }
               else
               {

                  CotizacionProductoDTO cotizacionProductoDTO = cotizacionProductoProxy.GetById(new CotizacionProductoDTO()
                  {
                     CotizacionProductoId = model.CotizacionProductoDTO.Id
                  });

                  fecha = cotizacionProductoDTO.CotizacionDTO.Fecha;

                  movimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId = cotizacionProductoDTO.CotizacionDTO.PeriodoEmpresaSedeId;
                  movimientoAlmacenSaldoDTO.CantidadAnterior = cotizacionProductoDTO.Cantidad;
               }

               MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO()
               {
                  Codigo = string.Format(StringFormatConstant.DATE_MONTH, fecha)
               });

               movimientoAlmacenSaldoDTO.ProductoId = model.CotizacionProductoDTO.ProductoId;
               movimientoAlmacenSaldoDTO.MesContableId = mesContableDTO.MesContableId;
               movimientoAlmacenSaldoDTO.CantidadIngreso = model.CotizacionProductoDTO.Cantidad;

               if (grupoProductoDTO.EsCombo == SiNoConstante.S)
                  validarSaldoCombo(movimientoAlmacenSaldoDTO);
               else
                  movimientoAlmacenSaldoProxy.ValidarSaldoVenta(movimientoAlmacenSaldoDTO);
            }

            CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(model.CalculadorVentaItemDTO);

            resultResponse.Data = new
            {
               calculadorVentaItemDTO.PrecioUnitario,
               calculadorVentaItemDTO.ValorUnitario,
               calculadorVentaItemDTO.MontoBruto,
               calculadorVentaItemDTO.PorcentajeDescuento,
               calculadorVentaItemDTO.MontoDescuento1,
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
      private void validarSaldoCombo(MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO)
      {

         List<ComboProductoDTO> comboProductoDTOList = comboProductoProxy.Search(new ComboProductoDTO()
         {
            ProductoId = movimientoAlmacenSaldoDTO.ProductoId,
            Estado = EstadoConstante.ACTIVO
         });

         List<MovimientoAlmacenSaldoDTO> movimientoAlmacenSaldoDTOList = new();
         foreach (ComboProductoDTO comboProductoDTO in comboProductoDTOList)
         {
            movimientoAlmacenSaldoDTOList.Add(new MovimientoAlmacenSaldoDTO()
            {
               PeriodoEmpresaSedeId = movimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId,
               ProductoId = comboProductoDTO.ProductoIncluidoId,
               MesContableId = movimientoAlmacenSaldoDTO.MesContableId,
               CantidadIngreso = comboProductoDTO.Cantidad * movimientoAlmacenSaldoDTO.CantidadIngreso,
               CantidadAnterior = comboProductoDTO.Cantidad * movimientoAlmacenSaldoDTO.CantidadAnterior
            });
         }

         movimientoAlmacenSaldoProxy.ValidarSaldoVentaCombo(movimientoAlmacenSaldoDTOList);
      }
      [HttpPost("GrabarCotizacionProducto")]
      public ActionResult GrabarCotizacionProducto([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CotizacionProductoDTO cotizacionProductoDTO = cargarCotizacionProductoDTO(model);

            int newId = model.CotizacionProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (cotizacionProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = cotizacionProductoProxy.Insert(cotizacionProductoDTO);
                  messageResourceKey = "VT_producto_agregado";
                  break;

               case DTOState.Modified:
                  cotizacionProductoProxy.Update(cotizacionProductoDTO);
                  messageResourceKey = "VT_producto_actualizado";
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
      private CotizacionProductoDTO cargarCotizacionProductoDTO(PedidoViewModel model)
      {

         CotizacionProductoDTO cotizacionProductoDTO;
         try
         {

            cotizacionProductoDTO = model.CotizacionProductoDTO;

            if (cotizacionProductoDTO.CotizacionDTO == null)
               cotizacionProductoDTO.CotizacionDTO = new CotizacionDTO() { EsPedido = SiNoConstante.S};

            if (string.IsNullOrEmpty(cotizacionProductoDTO.IncluyeDescuentoListaPrecio))
               cotizacionProductoDTO.IncluyeDescuentoListaPrecio = SiNoConstante.N;
            if (string.IsNullOrEmpty(cotizacionProductoDTO.SinSaldo))
               cotizacionProductoDTO.SinSaldo = SiNoConstante.N;
            if (string.IsNullOrEmpty(cotizacionProductoDTO.CantidadBloqueada))
               cotizacionProductoDTO.CantidadBloqueada = SiNoConstante.N;
            if (string.IsNullOrEmpty(cotizacionProductoDTO.SolicitaAutorizacionDescuento))
               cotizacionProductoDTO.SolicitaAutorizacionDescuento = SiNoConstante.N;

            string tipoIgv = cotizacionProductoDTO.ProductoDTO.TipoIgvDTO.Codigo;
            if (cotizacionProductoDTO.TipoAfectacionIgvId == 0)
            {
               TipoAfectacionIgvDTO tipoAfectacionIgvDTO = null;
               switch (tipoIgv)
               {
                  case TipoIgvConstante.GRAVADO:
                     tipoAfectacionIgvDTO = tipoAfectacionIgvProxy.SingleSearch(new TipoAfectacionIgvDTO()
                     {
                        TipoIgvDTO = new TipoIgvDTO() { Codigo = tipoIgv },
                        Codigo = TipoAfectacionIgvConstante.GRAVADO_OPERACION_ONEROSA
                     });
                     break;
               }

               cotizacionProductoDTO.TipoAfectacionIgvId = tipoAfectacionIgvDTO.Id;
            }

            if (cotizacionProductoDTO.TipoDescuentoId == 0)
            {

               TipoDescuentoDTO tipoDescuentoDTO = tipoDescuentoProxy.ObtenerPorCodigo(new TipoDescuentoDTO() { Codigo = TipoDescuentoConstante.SIN_DESCUENTO });
               cotizacionProductoDTO.TipoDescuentoId = tipoDescuentoDTO.Id;
            }

            AuditDTO(cotizacionProductoDTO);
         }
         catch (Exception)
         {
            throw;
         }

         return cotizacionProductoDTO;
      }
      [HttpPost("EliminarCotizacionProducto")]
      public ActionResult EliminarCotizacionProducto([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CotizacionProductoDTO cotizacionProductoDTO = cotizacionProductoProxy.GetById(model.CotizacionProductoDTO);
            cotizacionProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(cotizacionProductoDTO);

            cotizacionProductoProxy.Delete(cotizacionProductoDTO);

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
      [HttpPost("ListarAnticipoCliente")]
      public ActionResult ListarAnticipoCliente(string jsonModel)
      {

         Logger.InfoStart();

         AnticipoClienteViewModel model = JsonSerializer.Deserialize<AnticipoClienteViewModel>(jsonModel);

         model.AnticipoClienteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.AnticipoClienteDTO.OrderBy = "ac.numero_opera,ac.fecha,tca.tipo_car_abo";
         model.AnticipoClienteDTO.CotizacionId = NumberUtil.WhenValueIsZero(model.AnticipoClienteDTO.CotizacionId.Value);

         try
         {
            List<AnticipoClienteDTO> anticipoClienteDTOList = anticipoClienteProxy.Search(model.AnticipoClienteDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = anticipoClienteDTOList.Count,
               rows = from f in anticipoClienteDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.AnticipoClienteId,
                            f.NumeroOperacion,
                            Fecha = string.Format(StringFormatConstant.DATE, f.Fecha),
                            MedioPago = f.MedioPagoDTO.Abreviatura,
                            Banco = f?.BancoDTO?.Nombre,
                            FechaReferencia = f.FechaReferencia == null ? string.Empty : string.Format(StringFormatConstant.DATE, f.FechaReferencia.Value),
                            f.NumeroReferencia,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.Monto,
                            f.TipoDocumentoComercialId,
                            TipoDocumentoComercial = f?.TipoDocumentoComercialDTO?.Abreviatura,
                            f.Serie,
                            f.Observacion,
                            TipoOperacionAnticipo = f.TipoOperacionAnticipoDTO.Codigo,
                            TipoOperacionAnticipoNombre = f.TipoOperacionAnticipoDTO.Abreviatura,
                            f.Extornado,
                            EstadoComprobante = f.EstadoComprobanteDTO.Codigo,
                            EstadoComprobanteNombre = f.EstadoComprobanteDTO.Nombre,
                            f.RegistradoEnCotizacion,
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
      [HttpPost("ExtornarAnticipoCliente")]
      public ActionResult ExtornarAnticipoCliente([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.AnticipoClienteDTO.DTOState = DTOState.Modified;
            AuditDTO(model.AnticipoClienteDTO);

            anticipoClienteProxy.Extornar(model.AnticipoClienteDTO);

            resultResponse.Message = SR.GetString("VT_anticipo_extornado");
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
      [HttpPost("CerrarAnticipoCliente")]
      public ActionResult CerrarAnticipoCliente([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.AnticipoClienteDTO);
            anticipoClienteProxy.Cerrar(model.AnticipoClienteDTO);
            resultResponse.Message = SR.GetString("VT_anticipo_cerrado");
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
      [HttpPost("AgregarAnticipoCliente")]
      public ActionResult AgregarAnticipoCliente([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AnticipoClienteDTO anticipoClienteDTO = anticipoClienteProxy.GetById(new AnticipoClienteDTO()
            {
               AnticipoClienteId = model.AnticipoClienteDTO.Id
            });

            AuditDTO(anticipoClienteDTO);
            anticipoClienteDTO.CotizacionId = model.AnticipoClienteDTO.CotizacionId;

            anticipoClienteProxy.Update(anticipoClienteDTO);
            resultResponse.Message = SR.GetString("VT_anticipo_cerrado");
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
      [HttpPost("EliminarAnticipoCliente")]
      public ActionResult EliminarAnticipoCliente([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AnticipoClienteDTO anticipoClienteDTO = anticipoClienteProxy.GetById(new AnticipoClienteDTO()
            {
               AnticipoClienteId = model.AnticipoClienteDTO.Id
            });

            AuditDTO(anticipoClienteDTO);

            if (anticipoClienteDTO.EstadoComprobanteDTO.Codigo == EstadoComprobanteConstante.EMITIDO
               && anticipoClienteDTO.RegistradoEnCotizacion == SiNoConstante.S)
               throw new Exception(SR.GetString("VT_anticipo_ya_emitido"));

            if (anticipoClienteDTO.RegistradoEnCotizacion == SiNoConstante.N)
               anticipoClienteProxy.AnularAsignacionPedido(anticipoClienteDTO);
            else
               anticipoClienteProxy.Delete(anticipoClienteDTO);

            resultResponse.Message = SR.GetString("VT_anticipo_eliminado");
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
      [Route("AutorizacionEditPopup")]
      public IActionResult AutorizacionEditPopup([FromBody] PedidoViewModel model)
      {
         model.AutorizacionPendiente = obtenerAutorizacionPendiente(model, model.CotizacionDTO.Id);

         string partialViewName = "AutorizacionEditPopup";
         if (model.AutorizacionPendiente == SiNoConstante.S)
            partialViewName = "EstadoAutoEditPopup";

         return PartialView(partialViewName,model);
      }
      [HttpPost("ListarCotizacionAutorizacion")]
      public ActionResult ListarCotizacionAutorizacion(string jsonModel)
      {

         Logger.InfoStart();

         PedidoViewModel model = JsonSerializer.Deserialize<PedidoViewModel>(jsonModel);

         try
         {
            List<CotizacionAutorizacionDTO> cotizacionAutorizacionDTOList = null;
            if (model.AutorizacionPendiente == SiNoConstante.N)
            {
               List<TipoAutorizacionModuloDTO> tipoAutorizacionModuloDTOList = tipoAutorizacionModuloProxy.Search(new TipoAutorizacionModuloDTO()
               {
                  OpcionModuloDTO = new OpcionModuloDTO() { Codigo = TipoAutorizacionModuloConstante.PEDIDO }
               });

               List<int> tipoAutorizacionIdList = tipoAutorizacionModuloDTOList.Select(x => x.TipoAutorizacionId).ToList();

               cotizacionAutorizacionDTOList = cotizacionAutorizacionProxy.ListarPorTipoAutorizacionIdList(new CotizacionAutorizacionDTO()
                  {
                     CotizacionId = model.CotizacionAutorizacionDTO.CotizacionId,
                     TipoAutorizacionIdList = tipoAutorizacionIdList
                  });
            }
            else
            {
               model.CotizacionAutorizacionDTO.CotizacionDTO = new CotizacionDTO()
               {
                  PeriodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID)
               };
               model.CotizacionAutorizacionDTO.Estado = EstadoConstante.ACTIVO;
               cotizacionAutorizacionDTOList = cotizacionAutorizacionProxy.Search(model.CotizacionAutorizacionDTO);
            }

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
                            f.TipoAutorizacionId,
                            TipoAutorizacion = f.TipoAutorizacionDTO.Codigo,
                            TipoAutorizacionNombre = f.TipoAutorizacionDTO.Nombre,
                            f.UsuarioId,
                            UsuarioNombre = f.UsuarioDTO != null ? f.UsuarioDTO.NombreCompleto : string.Empty,
                            FechaAutorizacion = f.FechaAutorizacion.HasValue ? string.Format(StringFormatConstant.DATE_TIME, f.FechaAutorizacion.Value) : string.Empty,
                            EstadoAprobacion = f.EstadoAprobacionDTO != null ? f.EstadoAprobacionDTO.Codigo : string.Empty,
                            EstadoAprobacionNombre = f.EstadoAprobacionDTO != null ? f.EstadoAprobacionDTO.Nombre : string.Empty,
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
      [HttpPost("ListarProductoAutorizacion")]
      public ActionResult ListarProductoAutorizacion(string jsonModel)
      {

         Logger.InfoStart();

         PedidoViewModel model = JsonSerializer.Deserialize<PedidoViewModel>(jsonModel);

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
                            f.ProductoDTO.Codigo,
                            ProductoNombre = f.ProductoDTO.Descripcion,
                            f.Cantidad,
                            f.PrecioUnitario,
                            f.CantidadBloqueada,
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
      [HttpPost("GrabarCotizacionAutorizacion")]
      public ActionResult GrabarCotizacionAutorizacion([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            int usuarioId = ObtenerUsuarioId();
            foreach(CotizacionAutorizacionDTO cotizacionAutorizacionDTO in model.CotizacionAutorizacionDTOList)
            {
               cotizacionAutorizacionDTO.Estado = EstadoConstante.ACTIVO;
               cotizacionAutorizacionDTO.UsuarioId = usuarioId;
               cotizacionAutorizacionDTO.UsuarioCreacionId = usuarioId;
               cotizacionAutorizacionDTO.UsuarioModificacionId = usuarioId;
               AuditDTO(cotizacionAutorizacionDTO);
            }

            if(model.CotizacionProductoDTOList != null)
               foreach (CotizacionProductoDTO cotizacionProductoDTO in model.CotizacionProductoDTOList)
               {
                  cotizacionProductoDTO.UsuarioCreacionId = usuarioId;
                  cotizacionProductoDTO.UsuarioModificacionId = usuarioId;
                  AuditDTO(cotizacionProductoDTO);
               }

            CotizacionAutorizacionDTO cotizacionAutorizacionDTORequest = new CotizacionAutorizacionDTO() { 
               CotizacionAutorizacionDTOList = model.CotizacionAutorizacionDTOList,
               CotizacionProductoDTOList = model.CotizacionProductoDTOList,
               UsuarioId = usuarioId,
               UsuarioCreacionId = usuarioId,
               UsuarioModificacionId = usuarioId
            };
            AuditDTO(cotizacionAutorizacionDTORequest);

            cotizacionAutorizacionProxy.Solicitar(cotizacionAutorizacionDTORequest);

            resultResponse.Message = SR.GetString("VT_cotizacion_autorizacion_solicitada");
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
      [Route("EstadoAutoEditPopup")]
      public IActionResult EstadoAutoEditPopup([FromBody] PedidoViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("CancelarAutorizacion")]
      public ActionResult CancelarAutorizacion([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            int usuarioId = ObtenerUsuarioId();
            foreach (CotizacionAutorizacionDTO cotizacionAutorizacionDTO in model.CotizacionAutorizacionDTOList)
            {
               cotizacionAutorizacionDTO.UsuarioId = usuarioId;
               cotizacionAutorizacionDTO.UsuarioCreacionId = usuarioId;
               cotizacionAutorizacionDTO.UsuarioModificacionId = usuarioId;
               cotizacionAutorizacionDTO.Estado = EstadoConstante.INACTIVO;
               AuditDTO(cotizacionAutorizacionDTO);
            }

            CotizacionAutorizacionDTO cotizacionAutorizacionDTORequest = new CotizacionAutorizacionDTO()
            {
               CotizacionAutorizacionDTOList = model.CotizacionAutorizacionDTOList,
               UsuarioId = usuarioId,
               UsuarioCreacionId = usuarioId,
               UsuarioModificacionId = usuarioId
            };
            AuditDTO(cotizacionAutorizacionDTORequest);

            cotizacionAutorizacionProxy.Cancelar(cotizacionAutorizacionDTORequest);

            resultResponse.Message = SR.GetString("VT_cotizacion_autorizacion_cancelada");
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
      [Route("ListPopup")]
      public IActionResult ListPopup([FromBody] PedidoViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("ListarBandejaPopup")]
      public ActionResult ListarBandejaPopup(string jsonModel)
      {

         Logger.InfoStart();

         PedidoViewModel model = JsonSerializer.Deserialize<PedidoViewModel>(jsonModel);

         model.CotizacionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CotizacionDTO.OrderBy = "c.fecha desc,c.cotiza desc";

         model.CotizacionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.CotizacionDTO.EsPedido = SiNoConstante.S;
         model.CotizacionDTO.EstadoCotizacionList = new List<string>() {
            EstadoCotizacionConstante.PEDIDO, EstadoCotizacionConstante.FACTURADO_PARCIAL};

         try
         {
            List<CotizacionDTO> cotizacionDTOList = cotizacionProxy.Search(model.CotizacionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = cotizacionDTOList.Count,
               rows = from f in cotizacionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.CotizacionId,
                            f.Codigo,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.ProveedorId,
                            Proveedor = f.ProveedorDTO.Codigo,
                            f.ProveedorDTO.RazonSocial,
                            TipoNaturalezaJuridica = f.ProveedorDTO.TipoNaturalezaJuridicaDTO.Codigo,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            f.VendedorId,
                            VendedorNombre = f.VendedorDTO.ProveedorDTO.RazonSocial,
                            f.MonedaId,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.ListaPrecioId,
                            f.TipoCondicionPagoId,
                            f.MontoAnticipo,
                            f.MontoTotal,
                            EstadoCotizacion = f.EstadoCotizacionDTO.Codigo,
                            EstadoCotizacionNombre = f.EstadoCotizacionDTO.Nombre,
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
      [Route("CotizacionProductoListPopup")]
      public IActionResult CotizacionProductoListPopup([FromBody] PedidoViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("ListarCotizacionProductoPopup")]
      public ActionResult ListarCotizacionProductoPopup(string jsonModel)
      {

         Logger.InfoStart();

         PedidoViewModel model = JsonSerializer.Deserialize<PedidoViewModel>(jsonModel);

         model.CotizacionProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CotizacionProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.CotizacionProductoDTO.NullRegistroComboId = SiNoConstante.S;
         model.CotizacionProductoDTO.ConCantidadPendiente = SiNoConstante.S;
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
                            UnidadMedidaNombre = f.UnidadMedidaDTO == null ? f.ProductoDTO.UnidadMedidaDTO.Abreviatura : f.UnidadMedidaDTO.Abreviatura,
                            f.Descripcion,
                            Cantidad = f.CantidadPendiente,
                            f.PrecioUnitario,
                            MontoTotal = f.CantidadPendiente * f.PrecioUnitario,
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

      [Route("OrdenProduccionListPopup")]
      public IActionResult OrdenProduccionListPopup([FromBody] PedidoViewModel model)
      {
         return PartialView(model);
      }

      [HttpGet("ReporteAnticipo")]
      public FileContentResult ReporteAnticipo(string format, string cid, string anticipoClienteId)
      {

         Dictionary<string, string> parameters = new()
         {
            { "anticipoClienteId", anticipoClienteId }
         };

         string reportName = "VT_ReciboAnticipo";

         byte[] reportData = appHelper.ExecuteReport(reportName, parameters, ReportFormatConstant.PDF);
         byte[] mergePdf = appHelper.MergePDFFiles(new System.IO.MemoryStream[]
            {
               new System.IO.MemoryStream(reportData),
               new System.IO.MemoryStream(reportData)
            },
            new PdfWaterMark[] {
                  new PdfWaterMark(),
                  new PdfWaterMark() { WaterMark="CONTROL ADMINISTRATIVO",Position=20 } }
         );

         return appHelper.ReportToPdf(mergePdf, reportName);
      }
      private void configurarServicioImpresion(PedidoViewModel pedidoViewModel)
      {
         int empresaId = 0;
         if (!string.IsNullOrEmpty(pedidoViewModel.ViewModelState.CID))
            empresaId = ObtenerEmpresaId(pedidoViewModel.ViewModelState.CID);
         else
            empresaId = ObtenerEmpresaId();

         VendedorDTO vendedorDTO = vendedorProxy.SingleSearch(new VendedorDTO()
         {
            ProveedorDTO = new ProveedorDTO()
            {
               EmpresaId = empresaId,
               UsuarioId = ObtenerUsuarioId()
            }
         });
         if (vendedorDTO != null)
         {
            pedidoViewModel.ServidorImpresion = vendedorDTO.ServidorImpresion;
         }

         string url = ObtenerDireccionServicioGeneracionFormatoImpresion();

         byte[] plainTextBytes = Encoding.UTF8.GetBytes(url);
         pedidoViewModel.DataUrl = Convert.ToBase64String(plainTextBytes);
      }
      [HttpPost("CambiarEstadoEditPopup")]
      public IActionResult CambiarEstadoEditPopup([FromBody] PedidoViewModel model)
      {
         CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = model.CotizacionDTO.CotizacionId });

         string estadoCotizacion = cotizacionDTO.EstadoCotizacionDTO.Codigo;
         List<string> estadoCotizacionList = null;

         switch (estadoCotizacion)
         {
            case EstadoCotizacionConstante.PEDIDO:
               estadoCotizacionList = new List<string>() { EstadoCotizacionConstante.FACTURADO };
               break;

            case EstadoCotizacionConstante.FACTURADO_PARCIAL:
               estadoCotizacionList = new List<string>() { EstadoCotizacionConstante.FACTURADO };
               break;
         }

         List<EstadoCotizacionDTO> estadoCotizacionDTOList = estadoCotizacionProxy.Search(new EstadoCotizacionDTO() {
            EstadoCotizacionList = estadoCotizacionList,
            Estado = EstadoConstante.ACTIVO
         }); ;
         model.EstadoCotizacionList = WebHelper.ToSelectListItem(estadoCotizacionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Empty);

         return PartialView(model);
      }
      [HttpPost("CambiarEstado")]
      public ActionResult CambiarEstado([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         if (!TienePermisoEscritura())
         {
            resultResponse.Success = false;
            resultResponse.Message = "No cuenta con permisos de escritura para cambiar estado.";
            return Json(resultResponse);
         }

         try
         {
            AuditDTO(model.CotizacionDTO);

            cotizacionProxy.CambiarEstado(model.CotizacionDTO);

            resultResponse.Message = SR.GetString("VT_cotizacion_estado_actualizado");
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
      [HttpPost("CambiarEstadoCotizacion")]
      public ActionResult CambiarEstadoCotizacion([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         if (!TienePermisoEscritura())
         {
            resultResponse.Success = false;
            resultResponse.Message = "No cuenta con permisos de escritura para cambiar a cotización.";
            return Json(resultResponse);
         }

         try
         {

            EstadoCotizacionDTO estadoCotizacionDTO = estadoCotizacionProxy.ObtenerPorCodigo(new EstadoCotizacionDTO() { Codigo = EstadoCotizacionConstante.COTIZACION });

            model.CotizacionDTO.EstadoCotizacionId = estadoCotizacionDTO.EstadoCotizacionId;

            AuditDTO(model.CotizacionDTO);

            cotizacionProxy.CambiarEstado(model.CotizacionDTO);

            resultResponse.Message = SR.GetString("VT_cotizacion_estado_actualizado");
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
      [HttpPost("ListarOrdenProduccionSubProceso")]
      public ActionResult ListarOrdenProduccionSubProceso(string jsonModel)
      {

         Logger.InfoStart();

         PedidoViewModel model = JsonSerializer.Deserialize<PedidoViewModel>(jsonModel);

         model.OrdenProduccionSubProcesoDTO.Estado = EstadoConstante.ACTIVO;
         model.OrdenProduccionSubProcesoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenProduccionSubProcesoDTO.OrderBy = "opsp.orden";
        
         model.OrdenProduccionSubProcesoDTO.OrdenProduccionId = NumberUtil.WhenValueIsZero(model.OrdenProduccionSubProcesoDTO.OrdenProduccionId);

         try
         {
            List<OrdenProduccionSubProcesoDTO> ordenProduccionSubProcesoDTOList = ordenProduccionSubProcesoProxy.Search(model.OrdenProduccionSubProcesoDTO);    

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = ordenProduccionSubProcesoDTOList.Count,
               rows = from f in ordenProduccionSubProcesoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.OrdenProduccionSubProcesoId,
                            f.OrdenProduccionId,
                            SubProcesoProduccionNombre = string.Concat(f.SubProcesoProduccionDTO.ProcesoProduccionDTO.Nombre, SeparadorConstante.GUION,
                            f.SubProcesoProduccionDTO.Nombre),
                            FechaInicio = f.FechaInicio.HasValue ? string.Format(StringFormatConstant.DATE_TIME, f.FechaInicio.Value) : string.Empty,
                            FechaFin = f.FechaFin.HasValue ? string.Format(StringFormatConstant.DATE_TIME, f.FechaFin.Value) : string.Empty,
                            EstadoOrdenProduccionSubProcesoNombre = f.EstadoOrdenProduccionSubProcesoDTO.Nombre,
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

      [Route("EnviarProduccionEditPopup")]
      public IActionResult EnviarProduccionEditPopup([FromBody] PedidoViewModel model)
      {
         return PartialView(model);
      }

      [Route("EnviarProduccionParcialEditPopup")]
      public IActionResult EnviarProduccionParcialEditPopup([FromBody] PedidoViewModel model)
      {
         return PartialView(model); 
      }

        [HttpPost("ListarEnvioProduccion")]
        public ActionResult ListarEnvioProduccion(string jsonModel)
        {
            Logger.InfoStart();

            PedidoViewModel model = JsonSerializer.Deserialize<PedidoViewModel>(jsonModel);

            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
            model.CotizacionProductoDTO.Estado = EstadoConstante.ACTIVO;

            try
            {
                List<CotizacionProductoDTO> cotizacionProductoDTOList = new List<CotizacionProductoDTO>();
                                               
                cotizacionProductoDTOList = cotizacionProductoProxy.CortesGratis(model.CotizacionProductoDTO);
               

                int index = 1;
                var jsonData = new
                {
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
                                   Codigo = f.ProductoDTO?.Codigo,
                                   ProductoNombre = f.ProductoDTO?.Descripcion,
                                   TipoProducto = f.ProductoDTO?.FamiliaProductoDTO?.ClaseProductoDTO?.GrupoProductoDTO?.TipoProductoDTO?.Codigo,
                                   UnidadMedidaNombre = f.UnidadMedidaDTO?.Abreviatura ?? f.ProductoDTO?.UnidadMedidaDTO?.Abreviatura,
                                   TipoIgv = f.ProductoDTO?.TipoIgvDTO?.Codigo,
                                   f.ProductoDTO?.DetalleRequerido,
                                   f.UnidadMedidaId,
                                   f.Descripcion,
                                   f.Cantidad,
                                   f.CantidadPresentacion,
                                   f.CantidadEquivalente,
                                   CantidadVenta = f.CantidadPresentacion == decimal.Zero ? f.Cantidad : f.CantidadPresentacion,
                                   f.Estado,
                                   CortesGratis = Math.Ceiling(f.CortesGratis),
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

        [HttpPost("ListarEnvioProduccionParcial")]
        public ActionResult ListarEnvioProduccionParcial(string jsonModel)
        {
            Logger.InfoStart();

            PedidoViewModel model = JsonSerializer.Deserialize<PedidoViewModel>(jsonModel);

            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            model.CotizacionProductoDTO.OrderBy = "p.producto";

            try
            {
                List<CotizacionProductoDTO> cotizacionProductoDTOList = new List<CotizacionProductoDTO>();

                cotizacionProductoDTOList = cotizacionProductoProxy.ProductosParcial(model.CotizacionProductoDTO);

                int index = 1;
                var jsonData = new
                {
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
                                   Codigo = f.ProductoDTO?.Codigo,
                                   ProductoNombre = f.ProductoDTO?.Descripcion,
                                   TipoProducto = f.ProductoDTO?.FamiliaProductoDTO?.ClaseProductoDTO?.GrupoProductoDTO?.TipoProductoDTO?.Codigo,
                                   UnidadMedidaNombre = f.UnidadMedidaDTO?.Abreviatura ?? f.ProductoDTO?.UnidadMedidaDTO?.Abreviatura,
                                   TipoIgv = f.ProductoDTO?.TipoIgvDTO?.Codigo,
                                   f.ProductoDTO?.DetalleRequerido,
                                   f.UnidadMedidaId,
                                   f.Descripcion,
                                   f.Cantidad,
                                   f.CantidadPresentacion,
                                   f.CantidadEquivalente,
                                   f.CantidadParcial,
                                   CantidadVenta = f.CantidadPresentacion == decimal.Zero ? f.Cantidad : f.CantidadPresentacion,
                                   CantidadRestante = (f.CantidadPresentacion == decimal.Zero ? f.Cantidad : f.CantidadPresentacion) - f.CantidadParcial,
                                   CantidadEnvio = decimal.Zero,
                                   f.Estado,
                                   CortesGratis = Math.Ceiling(f.CortesGratis),
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

      [HttpPost("GenerarOrdenProduccion")]
      public ActionResult GenerarOrdenProduccion([FromBody] PedidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
            model.CotizacionDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;
            model.CotizacionDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;

            AuditDTO(model.CotizacionDTO);

            model.CotizacionDTO.CotizacionProductoDTOList = model.CotizacionProductoDTOList;

            cotizacionProxy.GenerarOrdenProduccion(model.CotizacionDTO);

            resultResponse.Message = SR.GetString("PD_orden_produccion_agregada");
            resultResponse.Data = new
            {
               Id = obtenerOrderProduccionId(model.CotizacionDTO.CotizacionId),
               EnviadoProduccion = SiNoConstante.S
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


      [HttpPost("GenerarOrdenProduccionParcial")]
      public ActionResult GenerarOrdenProduccionParcial([FromBody] PedidoViewModel model)
      {
          Logger.InfoStart();

          ResultResponse resultResponse = new();

          try
          {

              PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
              model.CotizacionDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;
              model.CotizacionDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;

              AuditDTO(model.CotizacionDTO);

              model.CotizacionDTO.CotizacionProductoDTOList = model.CotizacionProductoDTOList;
              model.CotizacionDTO.CortesGratis = model.CortesGratis;

              cotizacionProxy.GenerarOrdenProduccionParcial(model.CotizacionDTO);

              resultResponse.Message = SR.GetString("PD_orden_produccion_agregada");
              resultResponse.Data = new
              {
                  Id = obtenerOrderProduccionId(model.CotizacionDTO.CotizacionId),
                  EnviadoProduccion = SiNoConstante.S
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
