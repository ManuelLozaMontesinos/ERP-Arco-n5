using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
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
   [Route("Cotizacion")]
   public class CotizacionController : AuthorizedController
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
      private readonly IVendedorSerieProxy vendedorSerieProxy;
      private readonly IListaPrecioProveedorProxy listaPrecioProveedorProxy;
      private readonly IGrupoProveedorItemProxy grupoProveedorItemProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly ITipoCambioDiaProxy tipoCambioDiaProxy;
      private readonly IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy;
      private readonly ITipoDescuentoProxy tipoDescuentoProxy;
      private readonly IProductoDetalleProxy productoDetalleProxy;
        private readonly IListaPrecioVendedorProxy listaPrecioVendedorProxy;
      private readonly AppHelper appHelper;

      public CotizacionController(
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
         IVendedorSerieProxy vendedorSerieProxy,
         IListaPrecioProveedorProxy listaPrecioProveedorProxy,
         IGrupoProveedorItemProxy grupoProveedorItemProxy,
         IMesContableProxy mesContableProxy,
         ITipoCambioDiaProxy tipoCambioDiaProxy,
         IMovimientoAlmacenSaldoProxy movimientoAlmacenSaldoProxy,
         ITipoDescuentoProxy tipoDescuentoProxy,
         IProductoDetalleProxy productoDetalleProxy,
         IListaPrecioVendedorProxy listaPrecioVendedorProxy,
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
         this.vendedorSerieProxy = vendedorSerieProxy;
         this.listaPrecioProveedorProxy = listaPrecioProveedorProxy;
         this.grupoProveedorItemProxy = grupoProveedorItemProxy;
         this.mesContableProxy = mesContableProxy;
         this.tipoCambioDiaProxy = tipoCambioDiaProxy;
         this.movimientoAlmacenSaldoProxy = movimientoAlmacenSaldoProxy;
         this.tipoDescuentoProxy = tipoDescuentoProxy;
         this.productoDetalleProxy = productoDetalleProxy;
         this.listaPrecioVendedorProxy = listaPrecioVendedorProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         CotizacionViewModel cotizacionViewModel = new();

         cargarListaEstadoCotizacion(cotizacionViewModel);

         MenuBarTagHelper menuBarTagHelper = new();
         cotizacionViewModel.MenuItemList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.CRUDAndPreview);

         cotizacionViewModel.MenuItemList.Add(new MenuItem() { Key = "consulta", Content = "Consulta de Precios" });

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
               cotizacionViewModel.Vendedor = vendedorDTO.ProveedorDTO.RazonSocial;
         }

         return PartialView(cotizacionViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         CotizacionViewModel model = JsonSerializer.Deserialize<CotizacionViewModel>(jsonModel);

         model.CotizacionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CotizacionDTO.OrderBy = "c.fecha desc,c.cotiza desc";

         model.CotizacionDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.CotizacionDTO.EsPedido = SiNoConstante.N;

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
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            VendedorNombre = f.VendedorDTO.ProveedorDTO.RazonSocial,
                            f.ListaPrecioId,
                            Moneda = f.MonedaDTO.Simbolo,
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
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody] CotizacionViewModel model)
      {
         Logger.InfoStart();

         CotizacionViewModel cotizacionViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true,
            AutorizacionPendiente = SiNoConstante.N
         };

         try
         {
            cargarDataParaEditar(cotizacionViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, cotizacionViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] CotizacionViewModel model)
      {

         Logger.InfoStart();

         CotizacionViewModel cotizacionViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(cotizacionViewModel, model.CotizacionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, cotizacionViewModel);
      }
      private void cargarDataParaEditar(CotizacionViewModel model, int id)
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

         model.CotizacionDTO = cotizacionDTO;

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.SingleSearch(new ParametroSistemaDTO() { 
            Codigo = ParametroSistemaConstante.VENTAS_PORCENTAJE_INCREMENTO_PU_SI_LA_CANTIDAD_ES_FRACCION });

         model.PorcentajeFraccionPU = parametroSistemaDTO.ValorNumerico;

         cargarListas(model);
         configurarServicioImpresion(model);

         WebHelper.SelectedListItemSetValue(model.MonedaList, cotizacionDTO.MonedaId);
         WebHelper.SelectedListItemSetValue(model.ListaPrecioList, cotizacionDTO.ListaPrecioId);
         WebHelper.SelectedListItemSetValue(model.TipoCondicionPagoList, cotizacionDTO.TipoCondicionPagoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Save, Disabled = (model.AutorizacionPendiente == SiNoConstante.S)  },
            new MenuItem() { MenuItemType = MenuItemType.Exit },
            new MenuItem() { Key="soli_auto", Content="Solicitar Autorizaciones", Hidden = (model.AutorizacionPendiente == SiNoConstante.S) },
            new MenuItem() { Key="estado_auto", Content="Ver Autorizaciones", Hidden = (model.AutorizacionPendiente == SiNoConstante.N) },
            new MenuItem() { Key="gen_pedido", Content="Generar Pedido" },
            new MenuItem() { Key="gen_compro", Content="Generar Comprobante" }
         };

         MenuBarTagHelper menuBarTagHelper = new();
         model.MenuProductoList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.CRUDAndSaveCancel);
         model.MenuProductoList.Add(new MenuItem() { Key = "verificar", Content = "Verificar Saldos" });

         model.MenuAnticipoList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.NewEdit);
         model.MenuAnticipoList.Add(new MenuItem() { MenuItemType = MenuItemType.Preview });
         model.MenuAnticipoList.Add(new MenuItem() { Key = "cerrar", Content = "Cerrar Anticipo" });
         model.MenuAnticipoList.Add(new MenuItem() { MenuItemType = MenuItemType.Delete });
         model.MenuAnticipoList.Add(new MenuItem() { Key = "extornar", Content = "Extornar" });
      }
      private string obtenerAutorizacionPendiente(CotizacionViewModel model, int id)
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
      private void cargarListaEstadoCotizacion(CotizacionViewModel model)
      {
         List<EstadoCotizacionDTO> estadoCotizacionDTOList = estadoCotizacionProxy.Search(new EstadoCotizacionDTO() { Estado = EstadoConstante.ACTIVO });
         model.EstadoCotizacionList = WebHelper.ToSelectListItem(estadoCotizacionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListas(CotizacionViewModel model)
      {
         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         if (model.CotizacionDTO.MonedaId == 0)
            model.ListaPrecioList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
         else
         {

            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.CotizacionDTO.MonedaId, model.CotizacionDTO.VendedorId);

            model.ListaPrecioList = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

            if (model.CotizacionDTO.ListaPrecioId == 0)
               if (listaPrecioDTOList.Count > 0)
               {
                  model.CotizacionDTO.ListaPrecioId = listaPrecioDTOList[0].Id;
                  model.CotizacionDTO.ImpuestoTasaDTO = listaPrecioDTOList[0].ImpuestoTasaDTO;
               }
         }

         List<TipoCondicionPagoDTO> tipoCondicionPagoDTOList = tipoCondicionPagoProxy.Search(new TipoCondicionPagoDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoCondicionPagoList = WebHelper.ToSelectListItem(tipoCondicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoDescuentoDTO> tipoDescuentoDTOList = tipoDescuentoProxy.Search(new TipoDescuentoDTO());
         model.TipoDescuentoString = string.Join(SeparadorConstante.PUNTO_COMA, tipoDescuentoDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Abreviatura));
      }
  
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
      public JsonResult ListarListaPrecio([FromBody] CotizacionViewModel model)
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

            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.CotizacionDTO.MonedaId, model.CotizacionDTO.VendedorId);

            decimal montoTipoCambio = decimal.Zero;
            if (monedaDTO.EsLocal == SiNoConstante.N)
            {
               TipoCambioDiaDTO tipoCambioDiaDTO = tipoCambioDiaProxy.SingleSearch(new TipoCambioDiaDTO()
               {
                  TipoCambioDTO = new TipoCambioDTO() { Codigo = TipoCambioConstante.VENTAS },
                  MonedaId = monedaDTO.Id,
                  Fecha = model.CotizacionDTO.Fecha,
                  Estado = EstadoConstante.ACTIVO
               });

               if (tipoCambioDiaDTO == null)
                  throw new Exception(SR.GetString("MG_tipo_cambio_dia_no_registrado", model.CotizacionDTO.Fecha.ToShortDateString()));

               montoTipoCambio = tipoCambioDiaDTO.MontoVenta;
            }

            resultResponse.Data = new
            {
               monedaDTO.EsLocal,
               MontoTipoCambio = montoTipoCambio,
               List = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.NombreMoneda, SelectListFirstOptionType.Select)
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
      public ActionResult Grabar([FromBody] CotizacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CotizacionDTO cotizacionDTO = cargarCotizacionDTO(model);

            if (cotizacionDTO.MonedaDTO.EsLocal == SiNoConstante.N)
            {
               if (cotizacionDTO.MontoTipoCambio == decimal.Zero)
                  throw new Exception(SR.GetString("GEN_tipo_cambio_monto_requerido"));
            }

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
                  //cotizacionProxy.UpdateAnticipo(cotizacionDTO);
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
      private CotizacionDTO cargarCotizacionDTO(CotizacionViewModel model)
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

               MonedaDTO monedaDTO = monedaProxy.GetById(new MonedaDTO() { MonedaId = model.CotizacionDTO.MonedaId });
               cotizacionDTO.MonedaDTO = monedaDTO;

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
               model.CotizacionDTO.Fecha = cotizacionDTO.Fecha;
               model.CotizacionDTO.MonedaDTO = cotizacionDTO.MonedaDTO;
               model.CotizacionDTO.ImpuestoTasaId = cotizacionDTO.ImpuestoTasaId;
               model.CotizacionDTO.MontoBruto = cotizacionDTO.MontoBruto;
               model.CotizacionDTO.MontoNeto = cotizacionDTO.MontoNeto;
               model.CotizacionDTO.MontoIgv = cotizacionDTO.MontoIgv;
               model.CotizacionDTO.MontoIsc = cotizacionDTO.MontoIsc;
               model.CotizacionDTO.MontoTotal = cotizacionDTO.MontoTotal;
               model.CotizacionDTO.EsPedido = cotizacionDTO.EsPedido;
               model.CotizacionDTO.EnviadoProduccion = cotizacionDTO.EnviadoProduccion;

               cotizacionDTO = model.CotizacionDTO;
            }

            if (string.IsNullOrEmpty(cotizacionDTO.EsPedido))
               cotizacionDTO.EsPedido = SiNoConstante.N;
            if (string.IsNullOrEmpty(cotizacionDTO.EnviadoProduccion))
               cotizacionDTO.EnviadoProduccion = SiNoConstante.N;

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
      public ActionResult Eliminar([FromBody] CotizacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.CotizacionDTO.DTOState = DTOState.Modified;

            CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(model.CotizacionDTO);

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

         return appHelper.ReportToPdf(reportData, string.Concat("CO", cotizacionDTO.Codigo));
      }
      private byte[] obtenerReporteData(string cotizacionId, string incluyePeso, int ListaPrecioId)
      {
         Dictionary<string, string> parameters = new()
         {
            { "cotizacionId", cotizacionId }
         };

         string reportName = string.Empty;

         switch (incluyePeso) { 
                case SiNoConstante.N: 
                    if(ListaPrecioId == 7){
                        reportName = "VT_CotizacionPedidoMB";
                    }
                    else{
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

      [HttpPost("PedidoCreatePopup")]
      public ActionResult PedidoCreatePopup([FromBody] CotizacionViewModel model)
      {

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<AlmacenDTO> almacenDTOList = vendedorSerieProxy.ListarAlmacenPorVendedor(new VendedorSerieDTO()
            {
               VendedorId = model.CotizacionDTO.VendedorId,
               DocumentoComercialSerieDTO = new DocumentoComercialSerieDTO()
               {
                  EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                  AlmacenIdNotNull = SiNoConstante.S
               },
               Estado = EstadoConstante.ACTIVO
            });

            model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

            if (almacenDTOList.Count > 0)
               WebHelper.SelectedListItemSetValue(model.AlmacenList, almacenDTOList[0].Id);

         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(model);
      }
      [HttpPost("GenerarPedido")]
      public ActionResult GenerarPedido([FromBody] CotizacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            DateTime fecha = model.CotizacionDTO.Fecha;
            CotizacionDTO cotizacionDTO = cargarCotizacionDTO(model);
            cotizacionDTO.Fecha = fecha;

            MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO()
            {
               Codigo = string.Format(StringFormatConstant.DATE_MONTH, DateTime.Today)
            });
            cotizacionDTO.MesContableId = mesContableDTO.Id;

            cotizacionProxy.GenerarPedido(cotizacionDTO);
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
      [HttpPost("GenerarComprobante")]
      public ActionResult GenerarComprobante([FromBody] CotizacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CotizacionDTO cotizacionDTO = cargarCotizacionDTO(model);
            MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO()
            {
               Codigo = string.Format(StringFormatConstant.DATE_MONTH, DateTime.Today)
            });
            cotizacionDTO.MesContableId = mesContableDTO.Id;

            int id = cotizacionProxy.GenerarComprobante(cotizacionDTO);

            resultResponse.Data = new
            {
               Id = id
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
      [HttpPost("ListarCotizacionProducto")]
      public ActionResult ListarCotizacionProducto(string jsonModel)
      {

         Logger.InfoStart();

         CotizacionViewModel model = JsonSerializer.Deserialize<CotizacionViewModel>(jsonModel);

         model.CotizacionProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CotizacionProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.CotizacionProductoDTO.NullRegistroComboId = SiNoConstante.S;

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

            List<ProductoDetalleDTO> productoDetalleDTOList = productoDetalleProxy.Search(new ProductoDetalleDTO() { ProductoId = id});

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
      public ActionResult VerificarSaldoCalcularMontos([FromBody] CotizacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            GrupoProductoDTO grupoProductoDTO = model.CotizacionProductoDTO.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO
               .GrupoProductoDTO;

            string sinSaldo = SiNoConstante.N;

            if (grupoProductoDTO.TipoProductoDTO.Codigo == TipoProductoConstante.PRODUCTO)
            {
               MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO = new();

               CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = model.CotizacionProductoDTO.CotizacionId });
               movimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId = cotizacionDTO.PeriodoEmpresaSedeId;

               MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO()
               {
                  Codigo = string.Format(StringFormatConstant.DATE_MONTH, DateTime.Today)
               });

               movimientoAlmacenSaldoDTO.ProductoId = model.CotizacionProductoDTO.ProductoId;
               movimientoAlmacenSaldoDTO.MesContableId = mesContableDTO.MesContableId;

               decimal saldoVenta = movimientoAlmacenSaldoProxy.ObtenerSaldoVenta(movimientoAlmacenSaldoDTO);

               if(saldoVenta < model.CotizacionProductoDTO.Cantidad)
                  sinSaldo = SiNoConstante.S;
            }

            CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(model.CalculadorVentaItemDTO);

            resultResponse.Data = new
            {
               SinSaldo = sinSaldo,
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
      [HttpPost("CalcularMontos")]
      public ActionResult CalcularMontos([FromBody] CotizacionViewModel model)
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
      [HttpPost("GrabarCotizacionProducto")]
      public ActionResult GrabarCotizacionProducto([FromBody] CotizacionViewModel model)
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
      private CotizacionProductoDTO cargarCotizacionProductoDTO(CotizacionViewModel model)
      {

         CotizacionProductoDTO cotizacionProductoDTO;
         try
         {

            cotizacionProductoDTO = model.CotizacionProductoDTO;

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
      public ActionResult EliminarCotizacionProducto([FromBody] CotizacionViewModel model)
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
      [HttpPost("ActualizarSinSaldo")]
      public ActionResult ActualizarSinSaldo([FromBody] CotizacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            List<CotizacionProductoDTO> cotizacionProductoDTOList = cotizacionProductoProxy.Search(new CotizacionProductoDTO()
            {
               CotizacionId = model.CotizacionProductoDTO.CotizacionId,
               ProductoDTO = new ProductoDTO() { FamiliaProductoDTO = new FamiliaProductoDTO(){ 
                  ClaseProductoDTO = new ClaseProductoDTO() { GrupoProductoDTO = new GrupoProductoDTO()
                  {
                     TipoProductoDTO = new TipoProductoDTO() { Codigo = TipoProductoConstante.PRODUCTO}
                  } }
               }},
               Estado = EstadoConstante.ACTIVO
            });

            foreach(CotizacionProductoDTO cotizacionProductoDTO in cotizacionProductoDTOList)
            {
               string sinSaldo = SiNoConstante.N;
               MovimientoAlmacenSaldoDTO movimientoAlmacenSaldoDTO = new();

               CotizacionDTO cotizacionDTO = cotizacionProxy.GetById(new CotizacionDTO() { CotizacionId = model.CotizacionProductoDTO.CotizacionId });
               movimientoAlmacenSaldoDTO.PeriodoEmpresaSedeId = cotizacionDTO.PeriodoEmpresaSedeId;

               MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO()
               {
                  Codigo = string.Format(StringFormatConstant.DATE_MONTH, DateTime.Today)
               });

               movimientoAlmacenSaldoDTO.ProductoId = cotizacionProductoDTO.ProductoId;
               movimientoAlmacenSaldoDTO.MesContableId = mesContableDTO.MesContableId;

               decimal saldoVenta = movimientoAlmacenSaldoProxy.ObtenerSaldoVenta(movimientoAlmacenSaldoDTO);

               if (saldoVenta < cotizacionProductoDTO.Cantidad)
                  sinSaldo = SiNoConstante.S;

               cotizacionProductoDTO.SinSaldo = sinSaldo;

               AuditDTO(cotizacionProductoDTO);

               cotizacionProductoProxy.ActualizarSinSaldo(cotizacionProductoDTO);
            }

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
      public ActionResult ExtornarAnticipoCliente([FromBody] CotizacionViewModel model)
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
      public ActionResult CerrarAnticipoCliente([FromBody] CotizacionViewModel model)
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
      public ActionResult AgregarAnticipoCliente([FromBody] CotizacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AnticipoClienteDTO anticipoClienteDTO = anticipoClienteProxy.GetById(new AnticipoClienteDTO() { 
               AnticipoClienteId = model.AnticipoClienteDTO.Id
            });

            AuditDTO(anticipoClienteDTO);
            anticipoClienteDTO.CotizacionId = model.AnticipoClienteDTO.CotizacionId;

            anticipoClienteProxy.Update(anticipoClienteDTO);
            resultResponse.Message = SR.GetString("VT_anticipo_agregado");
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
      public ActionResult EliminarAnticipoCliente([FromBody] CotizacionViewModel model)
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
      public IActionResult AutorizacionEditPopup([FromBody] CotizacionViewModel model)
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

         CotizacionViewModel model = JsonSerializer.Deserialize<CotizacionViewModel>(jsonModel);

         try
         {
            List<CotizacionAutorizacionDTO> cotizacionAutorizacionDTOList = null;
            if (model.AutorizacionPendiente == SiNoConstante.N)
            {
               List<TipoAutorizacionModuloDTO> tipoAutorizacionModuloDTOList = tipoAutorizacionModuloProxy.Search(new TipoAutorizacionModuloDTO()
               {
                  OpcionModuloDTO = new OpcionModuloDTO() { Codigo = TipoAutorizacionModuloConstante.COTIZACION }
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

         CotizacionViewModel model = JsonSerializer.Deserialize<CotizacionViewModel>(jsonModel);

         model.CotizacionProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CotizacionProductoDTO.OrderBy = "tp.tipo_producto,p.producto";
         model.CotizacionProductoDTO.ProductoDTO = new ProductoDTO()
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
      public ActionResult GrabarCotizacionAutorizacion([FromBody] CotizacionViewModel model)
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
      public IActionResult EstadoAutoEditPopup([FromBody] CotizacionViewModel model)
      {
         return PartialView(model);
      }
      [HttpPost("CancelarAutorizacion")]
      public ActionResult CancelarAutorizacion([FromBody] CotizacionViewModel model)
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
      private void configurarServicioImpresion(CotizacionViewModel cotizacionViewModel)
      {
         int empresaId = 0;
         if (!string.IsNullOrEmpty(cotizacionViewModel.ViewModelState.CID))
            empresaId = ObtenerEmpresaId(cotizacionViewModel.ViewModelState.CID);
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
            cotizacionViewModel.ServidorImpresion = vendedorDTO.ServidorImpresion;
         }

         string url = ObtenerDireccionServicioGeneracionFormatoImpresion();

         byte[] plainTextBytes = Encoding.UTF8.GetBytes(url);
         cotizacionViewModel.DataUrl = Convert.ToBase64String(plainTextBytes);
      }
      [HttpPost("VistaPreviaEditPopup")]
      public ActionResult VistaPreviaEditPopup([FromBody] CotizacionViewModel model)
      {
         return PartialView(model);
      }
   }
}