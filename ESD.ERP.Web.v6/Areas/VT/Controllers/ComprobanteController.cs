using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.PD.Models;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class ComprobanteController : AuthorizedController
   {
      private readonly IEstadoComprobanteProxy estadoComprobanteProxy;
      private readonly IComprobanteProxy comprobanteProxy;
      private readonly IComprobanteProductoProxy comprobanteProductoProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IListaPrecioProxy listaPrecioProxy;
      private readonly IVendedorProxy vendedorProxy;
      private readonly IImpuestoTasaProxy impuestoTasaProxy;
      private readonly ITipoAfectacionIgvProxy tipoAfectacionIgvProxy;
      private readonly IParametroSistemaProxy parametroSistemaProxy;
      private readonly IVendedorSerieProxy vendedorSerieProxy;
      private readonly ITipoCondicionPagoProxy tipoCondicionPagoProxy;
      private readonly IMedioPagoProxy medioPagoProxy;
      private readonly IDocumentoComercialSerieProxy documentoComercialSerieProxy;
      private readonly ITipoComprobanteProxy tipoComprobanteProxy;
      private readonly ITipoDetraccionProxy tipoDetraccionProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy;
      private readonly ITipoAutorizacionModuloProxy tipoAutorizacionModuloProxy;
      private readonly IComprobanteAutorizacionProxy comprobanteAutorizacionProxy;
      private readonly IGuiaRemisionProxy guiaRemisionProxy;
      private readonly IProveedorProxy proveedorProxy;
      private readonly IListaPrecioProveedorProxy listaPrecioProveedorProxy;
      private readonly IGrupoProveedorItemProxy grupoProveedorItemProxy;
      private readonly ITipoCambioDiaProxy tipoCambioDiaProxy;
      private readonly IComboProductoProxy comboProductoProxy;
      private readonly IAnticipoClienteProxy anticipoClienteProxy;
      private readonly ICotizacionProxy cotizacionProxy;
      private readonly ITipoDescuentoProxy tipoDescuentoProxy;
      private readonly IComprobanteTrazaProxy comprobanteTrazaProxy;
      private readonly IProductoDetalleProxy productoDetalleProxy;
      private readonly ITipoDocumentoComercialProxy tipoDocumentoComercialProxy;
      private readonly IEstadoDespachoProxy estadoDespachoProxy;
      private readonly IListaPrecioVendedorProxy listaPrecioVendedorProxy;
      private readonly AppHelper appHelper;

      public ComprobanteController(
         IEstadoComprobanteProxy estadoComprobanteProxy,
         IComprobanteProxy comprobanteProxy,
         IComprobanteProductoProxy comprobanteProductoProxy,
         IMonedaProxy monedaProxy,
         IListaPrecioProxy listaPrecioProxy,
         IVendedorProxy vendedorProxy,
         IImpuestoTasaProxy impuestoTasaProxy,
         ITipoAfectacionIgvProxy tipoAfectacionIgvProxy,
         IParametroSistemaProxy parametroSistemaProxy,
         IVendedorSerieProxy vendedorSerieProxy,
         ITipoCondicionPagoProxy tipoCondicionPagoProxy,
         IMedioPagoProxy medioPagoProxy,
         IDocumentoComercialSerieProxy documentoComercialSerieProxy,
         ITipoComprobanteProxy tipoComprobanteProxy,
         ITipoDetraccionProxy tipoDetraccionProxy,
         IMesContableProxy mesContableProxy,
         IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy,
         ITipoAutorizacionModuloProxy tipoAutorizacionModuloProxy,
         IComprobanteAutorizacionProxy comprobanteAutorizacionProxy,
         IGuiaRemisionProxy guiaRemisionProxy,
         IProveedorProxy proveedorProxy,
         IListaPrecioProveedorProxy listaPrecioProveedorProxy,
         IGrupoProveedorItemProxy grupoProveedorItemProxy,
         ITipoCambioDiaProxy tipoCambioDiaProxy,
         IComboProductoProxy comboProductoProxy,
         IAnticipoClienteProxy anticipoClienteProxy,
         ICotizacionProxy cotizacionProxy,
         ITipoDescuentoProxy tipoDescuentoProxy,
         IComprobanteTrazaProxy comprobanteTrazaProxy,
         IProductoDetalleProxy productoDetalleProxy,
         ITipoDocumentoComercialProxy tipoDocumentoComercialProxy,
         IEstadoDespachoProxy estadoDespachoProxy,
         IListaPrecioVendedorProxy listaPrecioVendedorProxy,
         AppHelper appHelper
         )
      {
         this.estadoComprobanteProxy = estadoComprobanteProxy;
         this.comprobanteProxy = comprobanteProxy;
         this.comprobanteProductoProxy = comprobanteProductoProxy;
         this.monedaProxy = monedaProxy;
         this.listaPrecioProxy = listaPrecioProxy;
         this.vendedorProxy = vendedorProxy;
         this.impuestoTasaProxy = impuestoTasaProxy;
         this.tipoAfectacionIgvProxy = tipoAfectacionIgvProxy;
         this.parametroSistemaProxy = parametroSistemaProxy;
         this.vendedorSerieProxy = vendedorSerieProxy;
         this.tipoCondicionPagoProxy = tipoCondicionPagoProxy;
         this.medioPagoProxy = medioPagoProxy;
         this.documentoComercialSerieProxy = documentoComercialSerieProxy;
         this.tipoComprobanteProxy = tipoComprobanteProxy;
         this.tipoDetraccionProxy = tipoDetraccionProxy;
         this.mesContableProxy = mesContableProxy;
         this.movimientoAlmacenSaldoProxy = movimientoAlmacenSaldoProxy;
         this.tipoAutorizacionModuloProxy = tipoAutorizacionModuloProxy;
         this.comprobanteAutorizacionProxy = comprobanteAutorizacionProxy;
         this.guiaRemisionProxy = guiaRemisionProxy;
         this.proveedorProxy = proveedorProxy;
         this.listaPrecioProveedorProxy = listaPrecioProveedorProxy;
         this.grupoProveedorItemProxy = grupoProveedorItemProxy;
         this.tipoCambioDiaProxy = tipoCambioDiaProxy;
         this.comboProductoProxy = comboProductoProxy;
         this.anticipoClienteProxy = anticipoClienteProxy;
         this.cotizacionProxy = cotizacionProxy;
         this.tipoDescuentoProxy = tipoDescuentoProxy;
         this.comprobanteTrazaProxy = comprobanteTrazaProxy;
         this.productoDetalleProxy = productoDetalleProxy;
         this.tipoDocumentoComercialProxy = tipoDocumentoComercialProxy;
         this.estadoDespachoProxy = estadoDespachoProxy;
         this.listaPrecioVendedorProxy = listaPrecioVendedorProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index(
         int? cotizacionId = null,
         string pedidoCodigo = null,
         string clienteDocumento = null,
         string clienteNombre = null)
      {
         ComprobanteViewModel comprobanteViewModel = new();

         cargarListaEstadoComprobante(comprobanteViewModel);

         MenuBarTagHelper menuBarTagHelper = new();
         comprobanteViewModel.MenuItemList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.CRUDAndPreview);

         comprobanteViewModel.MenuItemList.Add(new MenuItem() { Key = "consulta", Content = "Consulta de Precios" });
         comprobanteViewModel.MenuItemList.Add(new MenuItem() { Key = "traza", FontImage= "fas fa-info-circle" });

         if (UsuarioEnRol(new List<string>() { RolConstante.SYS_ADMIN, RolConstante.TI }))
         {
            comprobanteViewModel.MenuItemList.Add(new MenuItem() { Key = "anularInt", Content = "Eliminar", Disabled = false });
         }

         if (!EsAdministrador() && !EsCaja())
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
               comprobanteViewModel.Vendedor = vendedorDTO.ProveedorDTO.RazonSocial;
         }

         ViewBag.DashboardCotizacionId = cotizacionId.GetValueOrDefault() > 0 ? cotizacionId.Value : 0;
         ViewBag.DashboardPedidoCodigo = pedidoCodigo ?? string.Empty;
         ViewBag.DashboardClienteDocumento = clienteDocumento ?? string.Empty;
         ViewBag.DashboardClienteNombre = clienteNombre ?? string.Empty;

         return PartialView(comprobanteViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ComprobanteViewModel model = JsonSerializer.Deserialize<ComprobanteViewModel>(jsonModel);

         model.ComprobanteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteDTO.OrderBy = "c.fecha desc,c.numero_opera desc";

         model.ComprobanteDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

         try
         {
            List<ComprobanteDTO> comprobanteDTOList = comprobanteProxy.Search(model.ComprobanteDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteDTOList.Count,
               rows = from f in comprobanteDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteId,
                            TipoDocumentoComercialNombre = f.TipoDocumentoComercialDTO.Abreviatura,
                            f.SerieNumero,
                            f.ListaPrecioId,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            VendedorNombre = f.VendedorDTO.ProveedorDTO.RazonSocial,
                            f.CotizacionId,
                            f.PedidoId,
                            f.CotizacionPedido,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.MontoTotal,
                            TipoComprobante = f.TipoComprobanteDTO.Codigo,
                            EstadoComprobante = f.EstadoComprobanteDTO.Codigo,
                            EstadoComprobanteNombre = f.EstadoComprobanteDTO.Nombre,
                            EstadoSunat = f.EstadoSunatDTO != null ? f.EstadoSunatDTO.Codigo : string.Empty,
                            EstadoSunatNombre = f.EstadoSunatDTO != null ? f.EstadoSunatDTO.Nombre : string.Empty,
                            PermitirEliminar = f.Fecha != DateTime.Today ? SiNoConstante.N : SiNoConstante.S,
                            TipoDocumentoComprob = f.TipoDocumentoComercialDTO.Codigo,
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
      public ActionResult Crear([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ComprobanteViewModel comprobanteViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true,
            AutorizacionPendiente = SiNoConstante.N
         };

         try
         {
            cargarDataParaEditar(comprobanteViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, comprobanteViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ComprobanteViewModel model)
      {

         Logger.InfoStart();

         ComprobanteViewModel comprobanteViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(comprobanteViewModel, model.ComprobanteDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, comprobanteViewModel);
      }
      private void cargarDataParaEditar(ComprobanteViewModel model, int id)
      {

         ComprobanteDTO comprobanteDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            comprobanteDTO = new ComprobanteDTO() {
               Fecha = DateTime.Today,
               TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO(),
               VendedorDTO = new VendedorDTO() { ProveedorDTO = new ProveedorDTO() { } },
            };

            ProveedorDTO proveedorDTO = proveedorProxy.SingleSearch(new ProveedorDTO() { 
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
               Codigo = ProveedorConstante.GENERICO
            });

            comprobanteDTO.ProveedorDTO = proveedorDTO;
            comprobanteDTO.ProveedorId = proveedorDTO.Id;

            VendedorDTO vendedorDTO = vendedorProxy.SingleSearch(new VendedorDTO() { 
               ProveedorDTO = new ProveedorDTO()
               {
                  EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
                  UsuarioId = ObtenerUsuarioId()
               }
            });
            if (vendedorDTO != null)
            {
               comprobanteDTO.VendedorDTO = vendedorDTO;
               comprobanteDTO.VendedorId = vendedorDTO.Id;
            }

            MonedaDTO monedaDTO = monedaProxy.ObtenerPorCodigo(new MonedaDTO() { Codigo = MonedaConstante.DEFAULT });
            comprobanteDTO.MonedaId = monedaDTO.Id;
            comprobanteDTO.MonedaDTO = monedaDTO;

            TipoCondicionPagoDTO tipoCondicionPagoDTO = tipoCondicionPagoProxy.ObtenerPorCodigo(new TipoCondicionPagoDTO() { 
               Codigo = TipoCondicionPagoConstante.CONTADO });
            comprobanteDTO.TipoCondicionPagoId = tipoCondicionPagoDTO.Id;

            MedioPagoDTO medioPagoDTO = medioPagoProxy.SingleSearch(new MedioPagoDTO()
            {
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
               Codigo = MedioPagoConstante.EFECTIVO
            });
            comprobanteDTO.MedioPagoId = medioPagoDTO.Id;
            comprobanteDTO.PorcentajeTarjeta = medioPagoDTO.PorcentajeTarjeta;

            EstadoComprobanteDTO estadoComprobanteDTO = estadoComprobanteProxy.ObtenerPorCodigo(new EstadoComprobanteDTO() { Codigo = EstadoComprobanteConstante.PENDIENTE });
            comprobanteDTO.EstadoComprobanteId = estadoComprobanteDTO.Id;
            comprobanteDTO.EstadoComprobanteDTO = estadoComprobanteDTO;

            model.IgnorarIncrementoFraccion = SiNoConstante.N;
            model.EsObsequio = SiNoConstante.N;
         }
         else
         {
            comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = id });

            model.AutorizacionPendiente = obtenerAutorizacionPendiente(model, id);
            model.TieneGuia = string.IsNullOrEmpty(comprobanteDTO.NumeroGuiaRemision) ? SiNoConstante.N : SiNoConstante.S;

            model.ListaPrecioProveedorId = internalObtenerListaPrecioProveedorId(comprobanteDTO.ListaPrecioId,
               comprobanteDTO.ProveedorId);
            model.IgnorarIncrementoFraccion = obtenerIgnorarIncrementoFraccion(comprobanteDTO.ProveedorId);
            model.EsObsequio = comprobanteDTO.TipoCondicionPagoDTO.Codigo == TipoCondicionPagoConstante.OBSEQUIO ? SiNoConstante.S : SiNoConstante.N;
         }

         model.ComprobanteDTO = comprobanteDTO;

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.SingleSearch(new ParametroSistemaDTO() { 
            Codigo = ParametroSistemaConstante.VENTAS_PORCENTAJE_INCREMENTO_PU_SI_LA_CANTIDAD_ES_FRACCION });

         model.PorcentajeFraccionPU = parametroSistemaDTO.ValorNumerico;

         cargarListas(model);
         configurarServicioImpresion(model);

         WebHelper.SelectedListItemSetValue(model.TipoDocumentoComercialList, comprobanteDTO.TipoDocumentoComercialId);
         WebHelper.SelectedListItemSetValue(model.MonedaList, comprobanteDTO.MonedaId);
         WebHelper.SelectedListItemSetValue(model.ListaPrecioList, comprobanteDTO.ListaPrecioId);
         WebHelper.SelectedListItemSetValue(model.TipoCondicionPagoList, comprobanteDTO.TipoCondicionPagoId);
         WebHelper.SelectedListItemSetValue(model.MedioPagoList, comprobanteDTO.MedioPagoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { MenuItemType = MenuItemType.Exit },
            new MenuItem() { Key="soli_auto", Content="Solicitar Autorizaciones", Hidden = (model.AutorizacionPendiente == SiNoConstante.S) },
            new MenuItem() { Key="estado_auto", Content="Ver Autorizaciones", Hidden = (model.AutorizacionPendiente == SiNoConstante.N) },
            new MenuItem() { Key="cerrar", Content="Cerrar Comprobante" },
            new MenuItem() { Key="genera_gr", Content="Generar G/R", Disabled = true },
            new MenuItem() { Key="reporte_canto", Content="Reporte Tapacanto" }
         };
      }
      private string obtenerAutorizacionPendiente(ComprobanteViewModel model, int id)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<ComprobanteAutorizacionDTO> comprobanteAutorizacionDTOList = comprobanteAutorizacionProxy.Search(new ComprobanteAutorizacionDTO()
         {
            ComprobanteDTO = new ComprobanteDTO()
            {
               PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO,
            },
            ComprobanteId = id,
            EstadoAprobacionDTO = new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.PENDIENTE_APROBACION }
         });

         return comprobanteAutorizacionDTOList.Count > 0 ? SiNoConstante.S : SiNoConstante.N;
      }
      private void cargarListaEstadoComprobante(ComprobanteViewModel model)
      {
         List<EstadoComprobanteDTO> estadoComprobanteDTOList = estadoComprobanteProxy.Search(new EstadoComprobanteDTO() { Estado = EstadoConstante.ACTIVO });
         model.EstadoComprobanteList = WebHelper.ToSelectListItem(estadoComprobanteDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListas(ComprobanteViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = vendedorSerieProxy.ListarTipoDocumentoComercialPorVendedor
            (new VendedorSerieDTO() { 
               VendedorId = model.ComprobanteDTO.VendedorId,
               DocumentoComercialSerieDTO = new DocumentoComercialSerieDTO() { 
                  EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                  TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO() { EsComprobantePago = SiNoConstante.S}
               },
               Estado = EstadoConstante.ACTIVO 
            });

         var tipoDocumentoComercialDTOListAgrupado = tipoDocumentoComercialDTOList.GroupBy
         (x => new { x.Id, x.Abreviatura }) 
         .Select(g => new TipoDocumentoComercialDTO
         {
            TipoDocumentoComercialId = g.Key.Id,
            Abreviatura = g.Key.Abreviatura
         }).ToList();

            model.TipoDocumentoComercialList = WebHelper.ToSelectListItem(tipoDocumentoComercialDTOListAgrupado, x => x.Id.ToString(), x => x.Abreviatura,
            SelectListFirstOptionType.Select);

         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         if (model.ComprobanteDTO.MonedaId == 0)
            model.ListaPrecioList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
         else
         {

            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.ComprobanteDTO.MonedaId, model.ComprobanteDTO.VendedorId);

            model.ListaPrecioList = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.NombreMoneda,
               SelectListFirstOptionType.Select);

            if (model.ComprobanteDTO.ListaPrecioId == 0)
               if (listaPrecioDTOList.Count > 0)
               {
                  model.ComprobanteDTO.ListaPrecioId = listaPrecioDTOList[0].Id;
                  model.ComprobanteDTO.ImpuestoTasaDTO = listaPrecioDTOList[0].ImpuestoTasaDTO;
               }
         }

         TipoCondicionPagoDTO tipoCondicionPagoDTO = new() { Estado = EstadoConstante.ACTIVO };

         if (!EsAdministrador() && !EsPricing())
            tipoCondicionPagoDTO.NotTipoCondicionPago = TipoCondicionPagoConstante.OBSEQUIO;

         List<TipoCondicionPagoDTO> tipoCondicionPagoDTOList = tipoCondicionPagoProxy.Search(tipoCondicionPagoDTO);
         model.TipoCondicionPagoList = WebHelper.ToSelectListItem(tipoCondicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MedioPagoDTO> medioPagoDTOList = medioPagoProxy.Search(new MedioPagoDTO() { 
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO 
         });
         model.MedioPagoList = WebHelper.ToSelectListItem(medioPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoDescuentoDTO> tipoDescuentoDTOList = tipoDescuentoProxy.Search(new TipoDescuentoDTO());
         model.TipoDescuentoString = string.Join(SeparadorConstante.PUNTO_COMA, tipoDescuentoDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Abreviatura));
      }
      [HttpPost("ListarSerie")]
      public JsonResult ListarSerie([FromBody] ComprobanteViewModel model)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<VendedorSerieDTO> vendedorSerieDTOList = vendedorSerieProxy.Search(new VendedorSerieDTO()
            {
               VendedorId = model.ComprobanteDTO.VendedorId,
               DocumentoComercialSerieDTO = new DocumentoComercialSerieDTO() {
                  EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                  TipoDocumentoComercialId = model.ComprobanteDTO.TipoDocumentoComercialId 
               },
               Estado = EstadoConstante.ACTIVO
            });

            int? almacenId = vendedorSerieDTOList.Count >= 1 ? vendedorSerieDTOList.FirstOrDefault().DocumentoComercialSerieDTO.AlmacenId
               : null;
            string esContingencia = vendedorSerieDTOList.Count >= 1 ? vendedorSerieDTOList.FirstOrDefault().DocumentoComercialSerieDTO.EsContingencia
               : string.Empty;
            string tipoDocumentoComercial = vendedorSerieDTOList.Count >= 1 ? vendedorSerieDTOList.FirstOrDefault().DocumentoComercialSerieDTO.TipoDocumentoComercialDTO.Codigo
               : string.Empty;
            string validarProveedor = vendedorSerieDTOList.Count >= 1 ? vendedorSerieDTOList.FirstOrDefault().DocumentoComercialSerieDTO.TipoDocumentoComercialDTO.ValidarProveedor
               : string.Empty;
            decimal montoMaximoSinProveedor = vendedorSerieDTOList.Count >= 1 ? vendedorSerieDTOList.FirstOrDefault().DocumentoComercialSerieDTO.
               TipoDocumentoComercialDTO.MontoMaximoSinProveedor : decimal.Zero;

            resultResponse.Data = new
            {
               TipoDocumentoComercial = tipoDocumentoComercial,
               ValidarProveedor = validarProveedor,
               MontoMaximoSinProveedor = montoMaximoSinProveedor,
               EsContingencia = esContingencia,
               AlmacenId = almacenId,
               List = WebHelper.ToSelectListItem(vendedorSerieDTOList, x => x.DocumentoComercialSerieDTO.Id.ToString(), x => x.DocumentoComercialSerieDTO.Serie,
               SelectListFirstOptionType.Empty)
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
      [HttpGet("ObtenerDocumentoSerie")]
      public JsonResult ObtenerDocumentoSerie(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            DocumentoComercialSerieDTO documentoComercialSerieDTO = documentoComercialSerieProxy.GetById(new DocumentoComercialSerieDTO() { DocumentoComercialSerieId = id });

            resultResponse.Data = new
            {
               documentoComercialSerieDTO.EsContingencia,
               documentoComercialSerieDTO.AlmacenId
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
      /*private List<ListaPrecioDTO> obtenerListaPrecio(PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO, int monedaId)
      {
         List<ListaPrecioDTO> listaPrecioDTOList = listaPrecioProxy.Search(new ListaPrecioDTO()
         {
            EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            MonedaId = monedaId,
            Estado = EstadoConstante.ACTIVO
         });

         return listaPrecioDTOList;
      }*/
        private List<ListaPrecioDTO> obtenerListaPrecio(PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO, int monedaId, int vendedorId)
        {
                        
            List<ListaPrecioVendedorDTO> listaPrecioVendedorDTOList = listaPrecioVendedorProxy.Search(new ListaPrecioVendedorDTO()
            {
                VendedorId = vendedorId,
                ListaPrecioDTO = new ListaPrecioDTO()
                {
                    EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
                    EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                    MonedaId = monedaId,
                    Estado = EstadoConstante.ACTIVO,
                },
                Estado = EstadoConstante.ACTIVO
            });

            List<ListaPrecioDTO> listaPrecioDTOList = new List<ListaPrecioDTO>();

            if (EsAdministrador()){
                listaPrecioDTOList = listaPrecioProxy.Search(new ListaPrecioDTO()
                {
                    EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
                    EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                    MonedaId = monedaId,
                    Estado = EstadoConstante.ACTIVO,
                });
            }else {
                listaPrecioDTOList = listaPrecioVendedorDTOList.Select(x => x.ListaPrecioDTO).ToList();
            }        
            return listaPrecioDTOList;
        }



      [HttpPost("ListarListaPrecio")]
      public JsonResult ListarListaPrecio([FromBody] ComprobanteViewModel model)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            MonedaDTO monedaDTO = monedaProxy.GetById(new MonedaDTO()
            {
               MonedaId = model.ComprobanteDTO.MonedaId
            });

            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.ComprobanteDTO.MonedaId, model.ComprobanteDTO.VendedorId);

            decimal montoTipoCambio = decimal.Zero;
            if(monedaDTO.EsLocal == SiNoConstante.N)
            {
               TipoCambioDiaDTO tipoCambioDiaDTO = tipoCambioDiaProxy.SingleSearch(new TipoCambioDiaDTO() { 
                  TipoCambioDTO = new TipoCambioDTO() { Codigo = TipoCambioConstante.VENTAS },
                  MonedaId = monedaDTO.Id,
                  Fecha = model.ComprobanteDTO.Fecha,
                  Estado = EstadoConstante.ACTIVO
               });

               if (tipoCambioDiaDTO == null)
                  throw new Exception(SR.GetString("MG_tipo_cambio_dia_no_registrado", model.ComprobanteDTO.Fecha.ToShortDateString()));

               montoTipoCambio = tipoCambioDiaDTO.MontoVenta;
            }
            
            resultResponse.Data = new
            {
               monedaDTO.EsLocal,
               MontoTipoCambio = montoTipoCambio,
               List = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.NombreMoneda,
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
      [HttpGet("ObtenerTipoCondicionPago")]
      public JsonResult ObtenerTipoCondicionPago(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            TipoCondicionPagoDTO tipoCondicionPagoDTO = tipoCondicionPagoProxy.GetById(new TipoCondicionPagoDTO() { TipoCondicionPagoId = id });

            resultResponse.Data = new
            {
               EsObsequio = tipoCondicionPagoDTO.Codigo == TipoCondicionPagoConstante.OBSEQUIO ? SiNoConstante.S : SiNoConstante.N
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
      public ActionResult Grabar([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ComprobanteDTO comprobanteDTO = cargarComprobanteDTO(model);

            validarComprobanteDisponible(comprobanteDTO,0);

            int newId = model.ComprobanteDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = comprobanteProxy.Insert(comprobanteDTO);
                  messageResourceKey = "VT_comprobante_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  comprobanteProxy.Update(comprobanteDTO);
                  messageResourceKey = "VT_comprobante_actualizado";
                  break;
            }

            comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, comprobanteDTO.SerieNumero);
            resultResponse.Data = new
            {
               comprobanteDTO.Id,
               comprobanteDTO.NumeroOperacion
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
      private ComprobanteDTO cargarComprobanteDTO(ComprobanteViewModel model)
      {

         ComprobanteDTO comprobanteDTO;
         try
         {

            comprobanteDTO = model.ComprobanteDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

               comprobanteDTO = model.ComprobanteDTO;

               comprobanteDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;
               comprobanteDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;

               ImpuestoTasaDTO impuestoTasaDTO = impuestoTasaProxy.SingleSearch(new ImpuestoTasaDTO() { 
                  TipoImpuestoDTO = new TipoImpuestoDTO() { Codigo = TipoImpuestoConstante.IGV },
                  Periodo = periodoEmpresaSedeDTO.PeriodoDTO.Codigo
               });
               comprobanteDTO.ImpuestoTasaId = impuestoTasaDTO.Id;

               TipoComprobanteDTO tipoComprobanteDTO = tipoComprobanteProxy.ObtenerPorCodigo(new TipoComprobanteDTO()
               {
                  Codigo = TipoComprobanteConstante.NORMAL
               });
               comprobanteDTO.TipoComprobanteId = tipoComprobanteDTO.Id;

               TipoDetraccionDTO tipoDetraccionDTO = tipoDetraccionProxy.ObtenerPorCodigo(new TipoDetraccionDTO()
               {
                  Codigo = TipoDetraccionConstante.SIN_DETRACCION
               });
               comprobanteDTO.TipoDetraccionId = tipoDetraccionDTO.Id;

               MonedaDTO monedaDTO = monedaProxy.GetById(new MonedaDTO() { MonedaId = model.ComprobanteDTO.MonedaId });
               comprobanteDTO.MonedaDTO = monedaDTO;

               EstadoComprobanteDTO estadoComprobanteDTO = estadoComprobanteProxy.ObtenerPorCodigo(new EstadoComprobanteDTO() { 
                  Codigo = EstadoComprobanteConstante.PENDIENTE });
               comprobanteDTO.EstadoComprobanteDTO = estadoComprobanteDTO;
               comprobanteDTO.EstadoComprobanteId = estadoComprobanteDTO.Id;

               EstadoDespachoDTO estadoDespachoDTO = estadoDespachoProxy.ObtenerPorCodigo(new EstadoDespachoDTO()
               {
                  Codigo = EstadoDespachoConstante.PENDIENTE
               });
               comprobanteDTO.EstadoDespachoId = estadoDespachoDTO.Id;
            }
            else
            {
               comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO(){ ComprobanteId = model.ComprobanteDTO.Id });

               model.ComprobanteDTO.MonedaDTO = comprobanteDTO.MonedaDTO;
               model.ComprobanteDTO.PorcentajeTarjeta = comprobanteDTO.PorcentajeTarjeta;
               model.ComprobanteDTO.ImpuestoTasaId = comprobanteDTO.ImpuestoTasaId;
               model.ComprobanteDTO.NumeroGuiaRemision = comprobanteDTO.NumeroGuiaRemision;
               model.ComprobanteDTO.MontoBruto = comprobanteDTO.MontoBruto;
               model.ComprobanteDTO.MontoNeto = comprobanteDTO.MontoNeto;
               model.ComprobanteDTO.MontoIgv = comprobanteDTO.MontoIgv;
               model.ComprobanteDTO.MontoIsc = comprobanteDTO.MontoIsc;
               model.ComprobanteDTO.MontoTotal = comprobanteDTO.MontoTotal;
               model.ComprobanteDTO.TipoComprobanteId = comprobanteDTO.TipoComprobanteId;
               model.ComprobanteDTO.TipoDetraccionId = comprobanteDTO.TipoDetraccionId;
               model.ComprobanteDTO.PorcentajeDetraccion = comprobanteDTO.PorcentajeDetraccion;
               model.ComprobanteDTO.MontoDetraccion = comprobanteDTO.MontoDetraccion;
               model.ComprobanteDTO.EsContingencia = comprobanteDTO.EsContingencia;
               model.ComprobanteDTO.Emitido = comprobanteDTO.Emitido;
               model.ComprobanteDTO.EstadoComprobanteDTO = comprobanteDTO.EstadoComprobanteDTO;

               comprobanteDTO = model.ComprobanteDTO;
            }

            if (string.IsNullOrEmpty(comprobanteDTO.Numero))
               comprobanteDTO.Numero = string.Empty;
            if (string.IsNullOrEmpty(comprobanteDTO.NombreAlterno))
               comprobanteDTO.NombreAlterno = string.Empty;
            if (string.IsNullOrEmpty(comprobanteDTO.NumeroGuiaRemision))
               comprobanteDTO.NumeroGuiaRemision = string.Empty;
            if (string.IsNullOrEmpty(comprobanteDTO.Canjeado))
               comprobanteDTO.Canjeado = SiNoConstante.N;
            if (string.IsNullOrEmpty(comprobanteDTO.NumeroOperacionPago))
               comprobanteDTO.NumeroOperacionPago = string.Empty;
            if (string.IsNullOrEmpty(comprobanteDTO.EsContingencia))
               comprobanteDTO.EsContingencia = SiNoConstante.N;
            if (string.IsNullOrEmpty(comprobanteDTO.Emitido))
               comprobanteDTO.Emitido = SiNoConstante.N;
            if (string.IsNullOrEmpty(comprobanteDTO.CodigoHash))
               comprobanteDTO.CodigoHash = string.Empty;
            if (string.IsNullOrEmpty(comprobanteDTO.Bancarizado))
               comprobanteDTO.Bancarizado = SiNoConstante.N;
            if (string.IsNullOrEmpty(comprobanteDTO.ObservacionDespacho))
               comprobanteDTO.ObservacionDespacho = string.Empty;
            if (string.IsNullOrEmpty(comprobanteDTO.EntregaDespacho))
               comprobanteDTO.EntregaDespacho = string.Empty;

            comprobanteDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(comprobanteDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return comprobanteDTO;
      }
      private void validarComprobanteDisponible(ComprobanteDTO comprobanteDTO, int comprobanteId)
      {
         if (comprobanteDTO == null)
            comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = comprobanteId });

         if (comprobanteDTO.MonedaDTO.EsLocal == SiNoConstante.N)
         {
            if(comprobanteDTO.MontoTipoCambio == decimal.Zero)
               throw new Exception(SR.GetString("GEN_tipo_cambio_monto_requerido"));
         }

         if (comprobanteDTO.EstadoComprobanteDTO.Codigo == EstadoComprobanteConstante.EMITIDO)
            throw new Exception(SR.GetString("VT_comprobante_ya_esta_emitido"));

         if (comprobanteDTO.EstadoComprobanteDTO.Codigo == EstadoComprobanteConstante.ANULADO)
            throw new Exception(SR.GetString("VT_comprobante_ya_esta_anulado"));
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ComprobanteDTO.DTOState = DTOState.Modified;

            AuditDTO(model.ComprobanteDTO);
            comprobanteProxy.Delete(model.ComprobanteDTO);

            ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(model.ComprobanteDTO);

            resultResponse.Message = SR.GetString("VT_comprobante_anulado", comprobanteDTO.SerieNumero);
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
      public FileContentResult ReportTo(string format, string cid, string comprobanteId, int listaPrecioId)
      {
         byte[] reportData = obtenerReporteData(comprobanteId, listaPrecioId);

         return appHelper.ReportToPdf(reportData, "VT_ComprobanteE");
      }
      private byte[] obtenerReporteData(string comprobanteId, int listaPrecioId)
      {
         Dictionary<string, string> parameters = new()
         {
            { "comprobanteId", comprobanteId }
         };

         ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = Convert.ToInt32(comprobanteId) });

         string formatoReporte = string.Empty;

         if(listaPrecioId == 7) {
                formatoReporte = "VT_ComprobanteEMB";
         }else{
            if (comprobanteDTO.TipoDocumentoComercialDTO.EsInterno == SiNoConstante.N)
               formatoReporte = comprobanteDTO.MontoDetraccion == decimal.Zero ? "VT_ComprobanteE" : "VT_InvoiceWithDetraction";
            else
               formatoReporte = "VT_InternoE";
         }

         byte[] reportData = appHelper.ExecuteReport(formatoReporte, parameters, ReportFormatConstant.PDF);

         return reportData;
      }
      [HttpPost("VerificarAnticipo")]
      public ActionResult VerificarAnticipo([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ComprobanteDTO comprobanteDTO = cargarComprobanteDTO(model);

            string tieneAnticipo = SiNoConstante.S;

            if (comprobanteDTO.CotizacionId.HasValue)
            {
               CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = comprobanteDTO.CotizacionId.Value });

               if (cotizacionDTO.MontoAnticipo > decimal.Zero)
               {
                  List<AnticipoClienteDTO> anticipoClienteDTOList = anticipoClienteProxy.Search(new AnticipoClienteDTO()
                  {
                     ComprobanteId = comprobanteDTO.Id,
                     TipoOperacionAnticipoDTO = new TipoOperacionAnticipoDTO() { Codigo = TipoOperacionAnticipoConstante.APLICACION },
                     Estado = EstadoConstante.ACTIVO
                  });

                  if (anticipoClienteDTOList.Count == 0)
                     tieneAnticipo = SiNoConstante.N;
               }
            }

            resultResponse.Data = new
            {
               TieneAnticipo = tieneAnticipo
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
      [HttpPost("Cerrar")]
      public ActionResult Cerrar([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ComprobanteDTO comprobanteDTO = cargarComprobanteDTO(model);

            comprobanteProxy.Cerrar(comprobanteDTO);

            comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = model.ComprobanteDTO.Id });

            resultResponse.Message = SR.GetString("VT_comprobante_cerrado");
            resultResponse.Data = new
            {
               comprobanteDTO.Numero,
               EstadoComprobante = comprobanteDTO.EstadoComprobanteDTO.Codigo,
               EstadoComprobanteNombre = comprobanteDTO.EstadoComprobanteDTO.Nombre
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
      [HttpPost("ListarComprobanteProducto")]
      public ActionResult ListarComprobanteProducto(string jsonModel)
      {

         Logger.InfoStart();

         ComprobanteViewModel model = JsonSerializer.Deserialize<ComprobanteViewModel>(jsonModel);

         model.ComprobanteProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.ComprobanteProductoDTO.NullRegistroComboId = SiNoConstante.S;

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
                            f.ValorUnitarioLista,
                            f.PrecioUnitarioLista,
                            f.ValorUnitario,
                            f.PrecioUnitario,
                            f.CantidadBloqueada,
                            f.IncluyeDescuentoListaPrecio,
                            f.PermiteDescuento,
                            f.SolicitaAutorizacionDescuento,
                            f.MontoBruto,
                            f.TipoDescuentoId,
                            TipoDescuento = f.TipoDescuentoDTO.Codigo,
                            f.DescuentoValor,
                            f.PorcentajeDescuento,
                            f.MontoDescuento1,
                            MontoTotalDescuento = decimal.Round(f.MontoDescuento1 * (1 + decimal.Round(f.ComprobanteDTO.ImpuestoTasaDTO.Tasa/100,2)),2),
                            f.MontoNeto,
                            f.MontoIgv,
                            f.MontoTotal,
                            f.TipoDetraccionId,
                            f.PorcentajeDetraccion,
                            f.MontoDetraccion,
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
            ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = id });
            if (comprobanteDTO == null)
               comprobanteDTO = new ComprobanteDTO();

            if(comprobanteDTO.MontoAnticipoDescuento > decimal.Zero)
            {
               comprobanteDTO.MontoBruto = comprobanteDTO.MontoBrutoDocumento;
               comprobanteDTO.MontoNeto = comprobanteDTO.MontoNetoDocumento;
               comprobanteDTO.MontoIgv = comprobanteDTO.MontoIgvDocumento;
               comprobanteDTO.MontoTotal = comprobanteDTO.MontoTotalDocumento;
            }

            resultResponse.Data = new
            {
               comprobanteDTO.MontoBruto,
               comprobanteDTO.MontoNeto,
               comprobanteDTO.MontoIgv,
               comprobanteDTO.MontoTotal,
               comprobanteDTO.MontoAnticipoDescuento
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
      public ActionResult VerificarSaldoCalcularMontos([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            GrupoProductoDTO grupoProductoDTO = model.ComprobanteProductoDTO.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO
               .GrupoProductoDTO;

            if (grupoProductoDTO.TipoProductoDTO.Codigo == TipoProductoConstante.PRODUCTO)
            {
               MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO = new();
               DateTime fecha = DateTime.Today;

               if (model.ComprobanteProductoDTO.Id == 0)
               {
                  ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = model.ComprobanteProductoDTO.ComprobanteId });

                  fecha = comprobanteDTO.Fecha;

                  movimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId = comprobanteDTO.PeriodoEmpresaSedeId;
               }
               else
               {

                  ComprobanteProductoDTO comprobanteProductoDTO = comprobanteProductoProxy.GetById(new ComprobanteProductoDTO()
                  {
                     ComprobanteProductoId = model.ComprobanteProductoDTO.Id
                  });

                  fecha = comprobanteProductoDTO.ComprobanteDTO.Fecha;

                  movimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId = comprobanteProductoDTO.ComprobanteDTO.PeriodoEmpresaSedeId;
                  movimientoAlmacenSaldoDTO.CantidadAnterior = comprobanteProductoDTO.Cantidad;
               }

               MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO()
               {
                  Codigo = string.Format(StringFormatConstant.DATE_MONTH, fecha)
               });

               movimientoAlmacenSaldoDTO.ProductoId = model.ComprobanteProductoDTO.ProductoId;
               movimientoAlmacenSaldoDTO.MesContableId = mesContableDTO.MesContableId;
               movimientoAlmacenSaldoDTO.CantidadIngreso = model.ComprobanteProductoDTO.Cantidad;

               if (grupoProductoDTO.EsCombo == SiNoConstante.S)
                  validarSaldoCombo(movimientoAlmacenSaldoDTO);
               else
                  movimientoAlmacenSaldoProxy.ValidarSaldoVenta(movimientoAlmacenSaldoDTO);
            }

            if (model.EsObsequio == SiNoConstante.S){

               TipoAfectacionIgvDTO tipoAfectacionIgvDTO = tipoAfectacionIgvProxy.SingleSearch(new TipoAfectacionIgvDTO()
               {
                  TipoIgvDTO = new TipoIgvDTO() { Codigo = TipoIgvConstante.GRATUITA },
                  Codigo = TipoAfectacionIgvConstante.INAFECTO_RETIRO_POR_BONIFICACION
               });
               model.CalculadorVentaItemDTO.TipoIGV = TipoIgvConstante.GRATUITA;
               model.ComprobanteProductoDTO.TipoAfectacionIgvId = tipoAfectacionIgvDTO.TipoAfectacionIgvId;
            }

            CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(model.CalculadorVentaItemDTO);

            resultResponse.Data = new
            {
               model.ComprobanteProductoDTO.TipoAfectacionIgvId,
               calculadorVentaItemDTO.PrecioUnitario,
               calculadorVentaItemDTO.ValorUnitario,
               calculadorVentaItemDTO.MontoBruto,
               calculadorVentaItemDTO.PorcentajeDescuento,
               calculadorVentaItemDTO.MontoDescuento1,
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
      private void validarSaldoCombo(MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO)
      {

         List<ComboProductoDTO> comboProductoDTOList = comboProductoProxy.Search(new ComboProductoDTO()
         {
            ProductoId = movimientoAlmacenSaldoDTO.ProductoId,
            Estado = EstadoConstante.ACTIVO
         });

         List<MovimientoAlmacenSaldoDTO> movimientoAlmacenSaldoDTOList = new();
         foreach(ComboProductoDTO comboProductoDTO in comboProductoDTOList)
         {
            movimientoAlmacenSaldoDTOList.Add(new MovimientoAlmacenSaldoDTO(){
               PeriodoEmpresaSedeId = movimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId,
               ProductoId = comboProductoDTO.ProductoIncluidoId,
               MesContableId = movimientoAlmacenSaldoDTO.MesContableId,
               CantidadIngreso = comboProductoDTO.Cantidad * movimientoAlmacenSaldoDTO.CantidadIngreso,
               CantidadAnterior = comboProductoDTO.Cantidad * movimientoAlmacenSaldoDTO.CantidadAnterior
            });
         }

         movimientoAlmacenSaldoProxy.ValidarSaldoVentaCombo(movimientoAlmacenSaldoDTOList);
      }
      [HttpPost("AplicarTasaPorMedioPago")]
      public ActionResult AplicarTasaPorMedioPago([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            validarComprobanteDisponible(null, model.ComprobanteDTO.ComprobanteId);

            comprobanteProxy.AplicarTasaPorMedioPago(model.ComprobanteDTO);
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
      [HttpPost("GrabarComprobanteProducto")]
      public ActionResult GrabarComprobanteProducto([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ComprobanteProductoDTO comprobanteProductoDTO = cargarComprobanteProductoDTO(model);

            validarComprobanteDisponible(null, comprobanteProductoDTO.ComprobanteId);

            int newId = model.ComprobanteProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (comprobanteProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = comprobanteProductoProxy.Insert(comprobanteProductoDTO);
                  messageResourceKey = "VT_producto_agregado";
                  break;

               case DTOState.Modified:
                  comprobanteProductoProxy.Update(comprobanteProductoDTO);
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
      private ComprobanteProductoDTO cargarComprobanteProductoDTO(ComprobanteViewModel model)
      {

         ComprobanteProductoDTO comprobanteProductoDTO;
         try
         {

            if(model.ComprobanteProductoDTO.DTOState == DTOState.Added)
            {
               comprobanteProductoDTO = model.ComprobanteProductoDTO;
            }
            else
            {
               comprobanteProductoDTO = comprobanteProductoProxy.GetById(new ComprobanteProductoDTO() { ComprobanteProductoId = model.ComprobanteProductoDTO.Id });

               model.ComprobanteProductoDTO.IncluyeDescuentoListaPrecio = comprobanteProductoDTO.IncluyeDescuentoListaPrecio;
               model.ComprobanteProductoDTO.RegistroReferencialId = comprobanteProductoDTO.RegistroReferencialId;

               comprobanteProductoDTO = model.ComprobanteProductoDTO;
            }

            if (string.IsNullOrEmpty(comprobanteProductoDTO.IncluyeDescuentoListaPrecio))
               comprobanteProductoDTO.IncluyeDescuentoListaPrecio = SiNoConstante.N;
            if (string.IsNullOrEmpty(comprobanteProductoDTO.CantidadBloqueada))
               comprobanteProductoDTO.CantidadBloqueada = SiNoConstante.N;
            if (string.IsNullOrEmpty(comprobanteProductoDTO.SolicitaAutorizacionDescuento))
               comprobanteProductoDTO.SolicitaAutorizacionDescuento = SiNoConstante.N;

            string tipoIgv = comprobanteProductoDTO.ProductoDTO.TipoIgvDTO.Codigo;
            if (comprobanteProductoDTO.TipoAfectacionIgvId == 0)
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

               comprobanteProductoDTO.TipoAfectacionIgvId = tipoAfectacionIgvDTO.Id;
            }

            if (comprobanteProductoDTO.TipoDescuentoId == 0)
            {

               TipoDescuentoDTO tipoDescuentoDTO = tipoDescuentoProxy.ObtenerPorCodigo(new TipoDescuentoDTO() { Codigo = TipoDescuentoConstante.SIN_DESCUENTO });
               comprobanteProductoDTO.TipoDescuentoId = tipoDescuentoDTO.Id;
            }

            if (string.IsNullOrEmpty(comprobanteProductoDTO.ObservacionDespacho))
               comprobanteProductoDTO.ObservacionDespacho = string.Empty;
            if (string.IsNullOrEmpty(comprobanteProductoDTO.EntregaDespacho))
               comprobanteProductoDTO.EntregaDespacho = string.Empty;

            AuditDTO(comprobanteProductoDTO);
         }
         catch (Exception)
         {
            throw;
         }

         return comprobanteProductoDTO;
      }
      [HttpPost("EliminarComprobanteProducto")]
      public ActionResult EliminarComprobanteProducto([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ComprobanteProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ComprobanteProductoDTO);

            ComprobanteProductoDTO comprobanteProductoDTO = comprobanteProductoProxy.GetById(model.ComprobanteProductoDTO);

            comprobanteProductoProxy.Delete(comprobanteProductoDTO);

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
      [Route("AutorizacionEditPopup")]
      public IActionResult AutorizacionEditPopup([FromBody] ComprobanteViewModel model)
      {
         model.AutorizacionPendiente = obtenerAutorizacionPendiente(model, model.ComprobanteDTO.Id);

         string partialViewName = "AutorizacionEditPopup";
         if (model.AutorizacionPendiente == SiNoConstante.S)
            partialViewName = "EstadoAutoEditPopup";

         return PartialView(partialViewName, model);
      }
      [HttpPost("ListarComprobanteAutorizacion")]
      public ActionResult ListarComprobanteAutorizacion(string jsonModel)
      {

         Logger.InfoStart();

         ComprobanteViewModel model = JsonSerializer.Deserialize<ComprobanteViewModel>(jsonModel);

         try
         {
            List<ComprobanteAutorizacionDTO> comprobanteAutorizacionDTOList = null;
            if (model.AutorizacionPendiente == SiNoConstante.N)
            {
               List<TipoAutorizacionModuloDTO> tipoAutorizacionModuloDTOList = tipoAutorizacionModuloProxy.Search(new TipoAutorizacionModuloDTO()
               {
                  OpcionModuloDTO = new OpcionModuloDTO() { Codigo = TipoAutorizacionModuloConstante.COMPROBANTE }
               });

               List<int> tipoAutorizacionIdList = tipoAutorizacionModuloDTOList.Select(x => x.TipoAutorizacionId).ToList();

               comprobanteAutorizacionDTOList = comprobanteAutorizacionProxy.ListarPorTipoAutorizacionIdList(new ComprobanteAutorizacionDTO()
               {
                  ComprobanteId = model.ComprobanteAutorizacionDTO.ComprobanteId,
                  TipoAutorizacionIdList = tipoAutorizacionIdList
               });
            }
            else
            {
               model.ComprobanteAutorizacionDTO.ComprobanteDTO = new ComprobanteDTO()
               {
                  PeriodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID)
               };
               model.ComprobanteAutorizacionDTO.Estado = EstadoConstante.ACTIVO;
               comprobanteAutorizacionDTOList = comprobanteAutorizacionProxy.Search(model.ComprobanteAutorizacionDTO);
            }

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

         ComprobanteViewModel model = JsonSerializer.Deserialize<ComprobanteViewModel>(jsonModel);

         model.ComprobanteProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.ComprobanteProductoDTO.ProductoDTO = new ProductoDTO()
         {
            FamiliaProductoDTO = new FamiliaProductoDTO()
            {
               ClaseProductoDTO = new ClaseProductoDTO()
               {
                  GrupoProductoDTO = new GrupoProductoDTO()
                  {
                     TipoProductoDTO = new TipoProductoDTO()
                     {
                        Codigo = TipoProductoConstante.PRODUCTO
                     }
                  }
               }
            }
         };

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
      [HttpPost("GrabarComprobanteAutorizacion")]
      public ActionResult GrabarComprobanteAutorizacion([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            foreach (ComprobanteAutorizacionDTO comprobanteAutorizacionDTO in model.ComprobanteAutorizacionDTOList)
            {
               comprobanteAutorizacionDTO.Estado = EstadoConstante.ACTIVO;
               AuditDTO(comprobanteAutorizacionDTO);
            }

            if (model.ComprobanteProductoDTOList != null)
               foreach (ComprobanteProductoDTO comprobanteProductoDTO in model.ComprobanteProductoDTOList)
               {
                  AuditDTO(comprobanteProductoDTO);
               }

            comprobanteAutorizacionProxy.Solicitar(new ComprobanteAutorizacionDTO()
            {
               ComprobanteAutorizacionDTOList = model.ComprobanteAutorizacionDTOList,
               ComprobanteProductoDTOList = model.ComprobanteProductoDTOList
            });

            resultResponse.Message = SR.GetString("VT_comprobante_autorizacion_solicitada");
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
      public IActionResult EstadoAutoEditPopup([FromBody] ComprobanteViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("CancelarAutorizacion")]
      public ActionResult CancelarAutorizacion([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            foreach (ComprobanteAutorizacionDTO comprobanteAutorizacionDTO in model.ComprobanteAutorizacionDTOList)
            {
               comprobanteAutorizacionDTO.UsuarioId = ObtenerUsuarioId();
               comprobanteAutorizacionDTO.Estado = EstadoConstante.INACTIVO;
               AuditDTO(comprobanteAutorizacionDTO);
            }

            comprobanteAutorizacionProxy.Cancelar(new ComprobanteAutorizacionDTO()
            {
               ComprobanteAutorizacionDTOList = model.ComprobanteAutorizacionDTOList
            });

            resultResponse.Message = SR.GetString("VT_comprobante_autorizacion_cancelada");
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
      [HttpPost("CreatePopup")]
      public ActionResult CreatePopup([FromBody] ComprobanteViewModel model)
      {

         try
         {
            model.ComprobanteDTO.Fecha = DateTime.Today;

            cargarListas(model);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(model);
      }
      [HttpPost("GenerarGuiaRemision")]
      public ActionResult GenerarGuiaRemision([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.GuiaRemisionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

            TipoDocumentoComercialDTO tipoDocumentoComercialDTO = tipoDocumentoComercialProxy.ObtenerPorCodigo(new TipoDocumentoComercialDTO()
            {
               Codigo = TipoDocumentoComercialConstante.GUIA_REMISION_REMITENTE
            });

            model.GuiaRemisionDTO.TipoDocumentoComercialDTO = tipoDocumentoComercialDTO;
            model.GuiaRemisionDTO.TipoDocumentoComercialId = tipoDocumentoComercialDTO.TipoDocumentoComercialId;

            model.GuiaRemisionDTO.SerieDocumentoReferencia = string.Empty;
            model.GuiaRemisionDTO.NumeroDocumentoReferencia = string.Empty;
            model.GuiaRemisionDTO.EsInterno = SiNoConstante.S;
            model.GuiaRemisionDTO.Impreso = SiNoConstante.N;
            model.GuiaRemisionDTO.EsVenta = SiNoConstante.S;
            model.GuiaRemisionDTO.EsModificacion = SiNoConstante.N;
            model.GuiaRemisionDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(model.GuiaRemisionDTO);

            guiaRemisionProxy.GenerarDesdeComprobante(model.GuiaRemisionDTO);

            resultResponse.Message = SR.GetString("VT_guia_remision_generada");
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
      [HttpPost("AgregarCotizacionProducto")]
      public ActionResult AgregarCotizacionProducto([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.ComprobanteProductoDTO);

            comprobanteProductoProxy.AgregarCotizacionProducto(model.ComprobanteProductoDTO);

            resultResponse.Message = SR.GetString("VT_cotizacion_producto_multiple_seleccion_agregado");
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
      public ActionResult ListPopup([FromBody] ComprobanteViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("ListarBandejaDocumentoEmitido")]
      public ActionResult ListarBandejaDocumentoEmitido(string jsonModel)
      {

         Logger.InfoStart();

         ComprobanteViewModel model = JsonSerializer.Deserialize<ComprobanteViewModel>(jsonModel);

         model.ComprobanteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteDTO.OrderBy = "c.fecha desc,c.numero_opera desc";

         //model.ComprobanteDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

         model.ComprobanteDTO.PeriodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
         model.ComprobanteDTO.EstadoComprobanteCodigoList = new List<string>() { EstadoComprobanteConstante.EMITIDO };

         try
         {
            List<ComprobanteDTO> comprobanteDTOList = comprobanteProxy.BuscarPorEmpresaSede(model.ComprobanteDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteDTOList.Count,
               rows = from f in comprobanteDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteId,
                            TipoDocumentoComercialNombre = f.TipoDocumentoComercialDTO.Abreviatura,
                            f.Serie,
                            f.Numero,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            f.VendedorId,
                            VendedorNombre = f.VendedorDTO.ProveedorDTO.RazonSocial,
                            f.CotizacionId,
                            f.PedidoId,
                            f.CotizacionPedido,
                            f.ImpuestoTasaId,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.MontoTotal,
                            EstadoComprobante = f.EstadoComprobanteDTO.Codigo,
                            EstadoComprobanteNombre = f.EstadoComprobanteDTO.Nombre,
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
      [HttpPost("AnularEditPopup")]
      public ActionResult AnularEditPopup([FromBody] ComprobanteViewModel model)
      {
         return PartialView(model);
      }
      private void configurarServicioImpresion(ComprobanteViewModel model)
      {
         VendedorDTO vendedorDTO = vendedorProxy.SingleSearch(new VendedorDTO()
         {
            ProveedorDTO = new ProveedorDTO()
            {
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
               UsuarioId = ObtenerUsuarioId()
            }
         });
         if (vendedorDTO != null)
         {
            model.ServidorImpresion = vendedorDTO.ServidorImpresion;
         }

         string url = ObtenerDireccionServicioGeneracionFormatoImpresion();

         byte[] plainTextBytes = Encoding.UTF8.GetBytes(url);
         model.DataUrl = Convert.ToBase64String(plainTextBytes);
      }
      [HttpPost("ComprobanteProductoListPopup")]
      public ActionResult ComprobanteProductoListPopup([FromBody] ComprobanteViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("ListarComprobanteProductoPopup")]
      public ActionResult ListarComprobanteProductoPopup(string jsonModel)
      {

         Logger.InfoStart();

         ComprobanteViewModel model = JsonSerializer.Deserialize<ComprobanteViewModel>(jsonModel);

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
                            UnidadMedidaNombre = f.UnidadMedidaDTO == null ? f.ProductoDTO.UnidadMedidaDTO.Abreviatura : f.UnidadMedidaDTO.Abreviatura,
                            TipoIgv = f.ProductoDTO.TipoIgvDTO.Codigo,
                            Cantidad = f.Cantidad - f.CantidadNotaCredito,
                            f.PrecioUnitario,
                            MontoTotal = (f.Cantidad - f.CantidadNotaCredito) * f.PrecioUnitario,
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
      [HttpPost("ComproProduCanjeListPopup")]
      public ActionResult ComproProduCanjeListPopup([FromBody] ComprobanteViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("ListarComproProduCanjePopup")]
      public ActionResult ListarComproProduCanjePopup(string jsonModel)
      {

         Logger.InfoStart();

         ComprobanteViewModel model = JsonSerializer.Deserialize<ComprobanteViewModel>(jsonModel);

         model.ComprobanteProductoDTO.ComprobanteDTO = new ComprobanteDTO()
         {
            PeriodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID)
         };

         try
         {
            List<ComprobanteProductoDTO> comprobanteProductoDTOList = comprobanteProductoProxy.ListarPendienteCanje(model.ComprobanteProductoDTO);

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
                            UnidadMedidaNombre = f.UnidadMedidaDTO == null ? f.ProductoDTO.UnidadMedidaDTO.Abreviatura : f.UnidadMedidaDTO.Abreviatura,
                            TipoIgv = f.ProductoDTO.TipoIgvDTO.Codigo,
                            f.Cantidad,
                            f.PrecioUnitario,
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
      [HttpGet("ObtenerComprobante")]
      public JsonResult ObtenerComprobante(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = id });

            resultResponse.Data = new
            {
               comprobanteDTO.ComprobanteId,
               comprobanteDTO.TipoDocumentoComercialId,
               comprobanteDTO.Serie,
               comprobanteDTO.Numero,
               comprobanteDTO.MonedaId,
               comprobanteDTO.ProveedorId,
               Proveedor = comprobanteDTO.ProveedorDTO.Codigo,
               TipoNaturalezaJuridica = comprobanteDTO.ProveedorDTO.TipoNaturalezaJuridicaDTO.Codigo,
               comprobanteDTO.ImpuestoTasaDTO.Tasa
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
      [HttpPost("ListarAnticipoCliente")]
      public ActionResult ListarAnticipoCliente(string jsonModel)
      {

         Logger.InfoStart();

         ComprobanteViewModel model = JsonSerializer.Deserialize<ComprobanteViewModel>(jsonModel);

         model.AnticipoClienteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.AnticipoClienteDTO.OrderBy = "ac.numero_opera,ac.fecha,tca.tipo_car_abo";
         model.AnticipoClienteDTO.ComprobanteId = NumberUtil.WhenValueIsZero(model.AnticipoClienteDTO.ComprobanteId.Value);

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
                            Moneda = f.MonedaDTO.Simbolo,
                            f.Monto,
                            TipoDocumentoComercial = f?.TipoDocumentoComercialDTO?.Abreviatura,
                            f.Observacion,
                            TipoOperacionAnticipo = f.TipoOperacionAnticipoDTO.Codigo,
                            TipoOperacionAnticipoNombre = f.TipoOperacionAnticipoDTO.Abreviatura,
                            f.Extornado,
                            SerieNumero = f.AnticipoClientePadreDTO == null ? string.Empty : 
                            (f.AnticipoClientePadreDTO.ComprobanteDTO == null ? string.Empty : string.Concat(f.AnticipoClientePadreDTO.ComprobanteDTO.Serie, 
                            SeparadorConstante.GUION, f.AnticipoClientePadreDTO.ComprobanteDTO.Numero)),
                            EstadoComprobante = f.EstadoComprobanteDTO.Codigo,
                            EstadoComprobanteNombre = f.EstadoComprobanteDTO.Nombre,
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
      [HttpPost("AplicarAnticipoCliente")]
      public ActionResult AplicarAnticipoCliente([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.AnticipoClienteDTO);

            anticipoClienteProxy.Aplicar(model.AnticipoClienteDTO);

            resultResponse.Message = SR.GetString("VT_anticipo_aplicado");
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
      [HttpPost("AnularAnticipoCliente")]
      public ActionResult AnularAnticipoCliente([FromBody] ComprobanteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.AnticipoClienteDTO);

            anticipoClienteProxy.AnularAplicacion(model.AnticipoClienteDTO);

            resultResponse.Message = SR.GetString("VT_anticipo_anulado");
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
      [HttpPost("ComprobanteTrazaListPopup")]
      public ActionResult ComprobanteTrazaListPopup([FromBody] ComprobanteViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("ListarComprobanteTraza")]
      public ActionResult ListarComprobanteTraza(string jsonModel)
      {

         Logger.InfoStart();

         ComprobanteViewModel model = JsonSerializer.Deserialize<ComprobanteViewModel>(jsonModel);

         model.ComprobanteTrazaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteTrazaDTO.OrderBy = "ct.fecha desc";

         try
         {
            List<ComprobanteTrazaDTO> comprobanteTrazaDTOList = comprobanteTrazaProxy.Search(model.ComprobanteTrazaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteTrazaDTOList.Count,
               rows = from f in comprobanteTrazaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteId,
                            TipoAccionAuditoria = f.TipoAccionAuditoriaDTO.Nombre,
                            Fecha = string.Format(StringFormatConstant.DATE_TIME,f.Fecha),
                            f.Descripcion,
                            Usuario = f.UsuarioDTO.NombreCompleto
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

   }
}
