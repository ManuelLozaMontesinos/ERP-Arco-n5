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
   [Route("[controller]")]
   public class ComproAnticipoController : AuthorizedController
   {
      private readonly IEstadoComprobanteProxy estadoComprobanteProxy;
      private readonly IComprobanteProxy comprobanteProxy;
      private readonly IComprobanteProductoProxy comprobanteProductoProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IListaPrecioProxy listaPrecioProxy;
      private readonly IVendedorProxy vendedorProxy;
      private readonly IImpuestoTasaProxy impuestoTasaProxy;
      private readonly ITipoAfectacionIgvProxy tipoAfectacionIgvProxy;
      private readonly IVendedorSerieProxy vendedorSerieProxy;
      private readonly ITipoCondicionPagoProxy tipoCondicionPagoProxy;
      private readonly IMedioPagoProxy medioPagoProxy;
      private readonly IDocumentoComercialSerieProxy documentoComercialSerieProxy;
      private readonly ITipoComprobanteProxy tipoComprobanteProxy;
      private readonly ITipoDetraccionProxy tipoDetraccionProxy;
      private readonly IProductoProxy productoProxy;
      private readonly IProveedorProxy proveedorProxy;
      private readonly ITipoCambioDiaProxy tipoCambioDiaProxy;
      private readonly IEstadoDespachoProxy estadoDespachoProxy;
      private readonly AppHelper appHelper;

      public ComproAnticipoController(
         IEstadoComprobanteProxy estadoComprobanteProxy,
         IComprobanteProxy comprobanteProxy,
         IComprobanteProductoProxy comprobanteProductoProxy,
         IMonedaProxy monedaProxy,
         IListaPrecioProxy listaPrecioProxy,
         IVendedorProxy vendedorProxy,
         IImpuestoTasaProxy impuestoTasaProxy,
         ITipoAfectacionIgvProxy tipoAfectacionIgvProxy,
         IVendedorSerieProxy vendedorSerieProxy,
         ITipoCondicionPagoProxy tipoCondicionPagoProxy,
         IMedioPagoProxy medioPagoProxy,
         IDocumentoComercialSerieProxy documentoComercialSerieProxy,
         ITipoComprobanteProxy tipoComprobanteProxy,
         ITipoDetraccionProxy tipoDetraccionProxy,
         IProductoProxy productoProxy,
         IProveedorProxy proveedorProxy,
         ITipoCambioDiaProxy tipoCambioDiaProxy,
         IEstadoDespachoProxy estadoDespachoProxy,
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
         this.vendedorSerieProxy = vendedorSerieProxy;
         this.tipoCondicionPagoProxy = tipoCondicionPagoProxy;
         this.medioPagoProxy = medioPagoProxy;
         this.documentoComercialSerieProxy = documentoComercialSerieProxy;
         this.tipoComprobanteProxy = tipoComprobanteProxy;
         this.tipoDetraccionProxy = tipoDetraccionProxy;
         this.productoProxy = productoProxy;
         this.proveedorProxy = proveedorProxy;
         this.tipoCambioDiaProxy = tipoCambioDiaProxy;
         this.estadoDespachoProxy = estadoDespachoProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ComproAnticipoViewModel comproAnticipoViewModel = new();

         cargarListaEstadoComprobante(comproAnticipoViewModel);

         VendedorDTO vendedorDTO = vendedorProxy.SingleSearch(new VendedorDTO()
         {
            ProveedorDTO = new ProveedorDTO()
            {
               EmpresaId = ObtenerEmpresaId(),
               UsuarioId = ObtenerUsuarioId()
            }
         });
         if (vendedorDTO != null)
            comproAnticipoViewModel.Vendedor = vendedorDTO.ProveedorDTO.RazonSocial;

         return PartialView(comproAnticipoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ComproAnticipoViewModel model = JsonSerializer.Deserialize<ComproAnticipoViewModel>(jsonModel);

         model.ComprobanteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteDTO.OrderBy = "c.fecha desc,c.numero_opera desc";

         model.ComprobanteDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.ComprobanteDTO.TipoComprobanteDTO = new()
         {
            Codigo = TipoComprobanteConstante.ANTICIPO
         };

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
                            Fecha = f.Fecha.ToShortDateString(),
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            VendedorNombre = f.VendedorDTO.ProveedorDTO.RazonSocial,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.MontoTotal,
                            EstadoComprobante = f.EstadoComprobanteDTO.Codigo,
                            EstadoComprobanteNombre = f.EstadoComprobanteDTO.Nombre,
                            EstadoSunat = f.EstadoSunatDTO != null ? f.EstadoSunatDTO.Codigo : string.Empty,
                            EstadoSunatNombre = f.EstadoSunatDTO != null ? f.EstadoSunatDTO.Nombre : string.Empty,
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
      public ActionResult Crear([FromBody] ComproAnticipoViewModel model)
      {
         Logger.InfoStart();

         ComproAnticipoViewModel comproAnticipoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(comproAnticipoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, comproAnticipoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ComproAnticipoViewModel model)
      {

         Logger.InfoStart();

         ComproAnticipoViewModel comproAnticipoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(comproAnticipoViewModel, model.ComprobanteDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, comproAnticipoViewModel);
      }
      private void cargarDataParaEditar(ComproAnticipoViewModel model, int id)
      {

         ComprobanteDTO comprobanteDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            comprobanteDTO = new ComprobanteDTO()
            {
               Fecha = DateTime.Today,
               TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO(),
               VendedorDTO = new VendedorDTO() { ProveedorDTO = new ProveedorDTO() { } },
            };

            ProveedorDTO proveedorDTO = proveedorProxy.SingleSearch(new ProveedorDTO()
            {
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
               Codigo = ProveedorConstante.GENERICO
            });

            comprobanteDTO.ProveedorDTO = proveedorDTO;
            comprobanteDTO.ProveedorId = proveedorDTO.Id;

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
               comprobanteDTO.VendedorDTO = vendedorDTO;
               comprobanteDTO.VendedorId = vendedorDTO.Id;
            }

            MonedaDTO monedaDTO = monedaProxy.ObtenerPorCodigo(new MonedaDTO() { Codigo = MonedaConstante.DEFAULT });
            comprobanteDTO.MonedaId = monedaDTO.Id;
            comprobanteDTO.MonedaDTO = monedaDTO;

            TipoCondicionPagoDTO tipoCondicionPagoDTO = tipoCondicionPagoProxy.ObtenerPorCodigo(new TipoCondicionPagoDTO()
            {
               Codigo = TipoCondicionPagoConstante.CONTADO
            });
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
         }
         else
         {
            comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = id });

            ComprobanteProductoDTO comprobanteProductoDTO = comprobanteProductoProxy.SingleSearch(new ComprobanteProductoDTO()
            {
               ComprobanteId = id,
               Estado = EstadoConstante.ACTIVO
            });
            model.Descripcion = comprobanteProductoDTO.Descripcion;
         }

         model.ComprobanteDTO = comprobanteDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.TipoDocumentoComercialList, comprobanteDTO.TipoDocumentoComercialId);
         WebHelper.SelectedListItemSetValue(model.MonedaList, comprobanteDTO.MonedaId);
         WebHelper.SelectedListItemSetValue(model.MedioPagoList, comprobanteDTO.MedioPagoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { MenuItemType = MenuItemType.Exit },
            new MenuItem() { Key="cerrar", Content="Cerrar Comprobante" }
         };
      }
      private void cargarListaEstadoComprobante(ComproAnticipoViewModel model)
      {
         List<EstadoComprobanteDTO> estadoComprobanteDTOList = estadoComprobanteProxy.Search(new EstadoComprobanteDTO() { Estado = EstadoConstante.ACTIVO });
         model.EstadoComprobanteList = WebHelper.ToSelectListItem(estadoComprobanteDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListas(ComproAnticipoViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = vendedorSerieProxy.ListarTipoDocumentoComercialPorVendedor
            (new VendedorSerieDTO()
            {
               VendedorId = model.ComprobanteDTO.VendedorId,
               DocumentoComercialSerieDTO = new DocumentoComercialSerieDTO()
               {
                  EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                  TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO() { 
                     EsInterno = SiNoConstante.N,
                     EsComprobantePago = SiNoConstante.S 
                  }
               },
               Estado = EstadoConstante.ACTIVO
            });
         model.TipoDocumentoComercialList = WebHelper.ToSelectListItem(tipoDocumentoComercialDTOList, x => x.Id.ToString(), x => x.Abreviatura,
            SelectListFirstOptionType.Select);

         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         if (model.ComprobanteDTO.MonedaId != 0)
         {
            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.ComprobanteDTO.MonedaId);

            if (model.ComprobanteDTO.ListaPrecioId == 0)
               if (listaPrecioDTOList.Count > 0)
               {
                  model.ComprobanteDTO.ListaPrecioId = listaPrecioDTOList[0].Id;
                  model.ComprobanteDTO.ImpuestoTasaDTO = listaPrecioDTOList[0].ImpuestoTasaDTO;
               }
         }

         List<MedioPagoDTO> medioPagoDTOList = medioPagoProxy.Search(new MedioPagoDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO
         });
         model.MedioPagoList = WebHelper.ToSelectListItem(medioPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("ListarSerie")]
      public JsonResult ListarSerie([FromBody] ComproAnticipoViewModel model)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<VendedorSerieDTO> vendedorSerieDTOList = vendedorSerieProxy.Search(new VendedorSerieDTO()
            {
               VendedorId = model.ComprobanteDTO.VendedorId,
               DocumentoComercialSerieDTO = new DocumentoComercialSerieDTO()
               {
                  EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                  TipoDocumentoComercialId = model.ComprobanteDTO.TipoDocumentoComercialId
               },
               Estado = EstadoConstante.ACTIVO
            });

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
               documentoComercialSerieDTO.EsContingencia
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
      public JsonResult ListarListaPrecio([FromBody] ComproAnticipoViewModel model)
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

            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.ComprobanteDTO.MonedaId);

            decimal montoTipoCambio = decimal.Zero;
            if (monedaDTO.EsLocal == SiNoConstante.N)
            {
               TipoCambioDiaDTO tipoCambioDiaDTO = tipoCambioDiaProxy.SingleSearch(new TipoCambioDiaDTO()
               {
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
               listaPrecioDTOList.FirstOrDefault().ListaPrecioId,
               listaPrecioDTOList.FirstOrDefault().ImpuestoTasaDTO.Tasa
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] ComproAnticipoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ComprobanteDTO comprobanteDTO = cargarComprobanteDTO(model);

            validarComprobanteDisponible(comprobanteDTO, 0);

            int newId = model.ComprobanteDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = comprobanteProxy.AgregarComprobanteAnticipo(comprobanteDTO);
                  messageResourceKey = "VT_comprobante_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  comprobanteProxy.ActualizarComprobanteAnticipo(comprobanteDTO);
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
      private ComprobanteDTO cargarComprobanteDTO(ComproAnticipoViewModel model)
      {

         ComprobanteDTO comprobanteDTO;
         try
         {

            comprobanteDTO = model.ComprobanteDTO;
            ComprobanteProductoDTO comprobanteProductoDTO = null;

            if (model.EditAction == ViewModelEditAction.New)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

               comprobanteDTO = model.ComprobanteDTO;

               comprobanteDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;
               comprobanteDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;
               comprobanteDTO.MontoBruto = model.ComprobanteDTO.MontoNeto;

               ImpuestoTasaDTO impuestoTasaDTO = impuestoTasaProxy.SingleSearch(new ImpuestoTasaDTO()
               {
                  TipoImpuestoDTO = new TipoImpuestoDTO() { Codigo = TipoImpuestoConstante.IGV },
                  Periodo = periodoEmpresaSedeDTO.PeriodoDTO.Codigo
               });
               comprobanteDTO.ImpuestoTasaId = impuestoTasaDTO.Id;

               TipoComprobanteDTO tipoComprobanteDTO = tipoComprobanteProxy.ObtenerPorCodigo(new TipoComprobanteDTO()
               {
                  Codigo = TipoComprobanteConstante.ANTICIPO
               });
               comprobanteDTO.TipoComprobanteId = tipoComprobanteDTO.Id;

               TipoCondicionPagoDTO tipoCondicionPagoDTO = tipoCondicionPagoProxy.ObtenerPorCodigo(new TipoCondicionPagoDTO() {
                  Codigo = TipoCondicionPagoConstante.CONTADO
               });
               comprobanteDTO.TipoCondicionPagoId = tipoCondicionPagoDTO.Id;

               TipoDetraccionDTO tipoDetraccionDTO = tipoDetraccionProxy.ObtenerPorCodigo(new TipoDetraccionDTO()
               {
                  Codigo = TipoDetraccionConstante.SIN_DETRACCION
               });
               comprobanteDTO.TipoDetraccionId = tipoDetraccionDTO.Id;

               MonedaDTO monedaDTO = monedaProxy.GetById(new MonedaDTO() { MonedaId = model.ComprobanteDTO.MonedaId });
               comprobanteDTO.MonedaDTO = monedaDTO;

               EstadoComprobanteDTO estadoComprobanteDTO = estadoComprobanteProxy.ObtenerPorCodigo(new EstadoComprobanteDTO()
               {
                  Codigo = EstadoComprobanteConstante.PENDIENTE
               });
               comprobanteDTO.EstadoComprobanteDTO = estadoComprobanteDTO;
               comprobanteDTO.EstadoComprobanteId = estadoComprobanteDTO.Id;

               EstadoDespachoDTO estadoDespachoDTO = estadoDespachoProxy.ObtenerPorCodigo(new EstadoDespachoDTO()
               {
                  Codigo = EstadoDespachoConstante.PENDIENTE
               });
               comprobanteDTO.EstadoDespachoId = estadoDespachoDTO.Id;

               ProductoDTO productoDTO = productoProxy.SingleSearch(new ProductoDTO() { 
                  Codigo = ProductoConstante.ANTICIPO,
                  EsOperacion = SiNoConstante.S,
                  Estado = EstadoConstante.ACTIVO
               });

               comprobanteProductoDTO = new ComprobanteProductoDTO()
               {
                  ProductoDTO = productoDTO,
                  ProductoId = productoDTO.ProductoId,
                  Cantidad = decimal.One,
                  CantidadBloqueada = SiNoConstante.N,
                  IncluyeDescuentoListaPrecio = SiNoConstante.N,
                  SolicitaAutorizacionDescuento = SiNoConstante.N,
                  TipoDetraccionId = tipoDetraccionDTO.Id,
                  ObservacionDespacho = string.Empty,
                  EntregaDespacho = string.Empty,
                  Estado = EstadoConstante.ACTIVO
               };
            }
            else
            {
               comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = model.ComprobanteDTO.Id });
               comprobanteDTO.MontoBruto = model.ComprobanteDTO.MontoNeto;
               comprobanteDTO.MontoNeto = model.ComprobanteDTO.MontoNeto;
               comprobanteDTO.MontoIgv = model.ComprobanteDTO.MontoIgv;
               comprobanteDTO.MontoBruto = model.ComprobanteDTO.MontoTotal;

               comprobanteProductoDTO = comprobanteProductoProxy.SingleSearch(new ComprobanteProductoDTO() { 
                  ComprobanteId = model.ComprobanteDTO.Id,
                  Estado = EstadoConstante.ACTIVO
               });
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
            if (string.IsNullOrEmpty(comprobanteDTO.Observacion))
               comprobanteDTO.Observacion = string.Empty;
            if (string.IsNullOrEmpty(comprobanteDTO.ObservacionDespacho))
               comprobanteDTO.ObservacionDespacho = string.Empty;
            if (string.IsNullOrEmpty(comprobanteDTO.EntregaDespacho))
               comprobanteDTO.EntregaDespacho = string.Empty;

            comprobanteDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(comprobanteDTO);

            comprobanteProductoDTO.Descripcion = model.Descripcion;

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
            comprobanteProductoDTO.PrecioUnitario = model.ComprobanteDTO.MontoTotal;
            comprobanteProductoDTO.ValorUnitario = model.ComprobanteDTO.MontoTotal - model.ComprobanteDTO.MontoIgv;
            comprobanteProductoDTO.MontoBruto = model.ComprobanteDTO.MontoNeto;
            comprobanteProductoDTO.MontoNeto = model.ComprobanteDTO.MontoNeto;
            comprobanteProductoDTO.MontoIgv = model.ComprobanteDTO.MontoIgv;
            comprobanteProductoDTO.MontoTotal = model.ComprobanteDTO.MontoTotal;

            AuditDTO(comprobanteProductoDTO);

            comprobanteDTO.ComprobanteProductoDTOList = new List<ComprobanteProductoDTO>
            {
               comprobanteProductoDTO
            };

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
            if (comprobanteDTO.MontoTipoCambio == decimal.Zero)
               throw new Exception(SR.GetString("GEN_tipo_cambio_monto_requerido"));
         }

         if (comprobanteDTO.EstadoComprobanteDTO.Codigo == EstadoComprobanteConstante.EMITIDO)
            throw new Exception(SR.GetString("VT_comprobante_ya_esta_emitido"));

         if (comprobanteDTO.EstadoComprobanteDTO.Codigo == EstadoComprobanteConstante.ANULADO)
            throw new Exception(SR.GetString("VT_comprobante_ya_esta_anulado"));
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] ComproAnticipoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ComprobanteDTO.DTOState = DTOState.Modified;

            ComprobanteProductoDTO comprobanteProductoDTO = comprobanteProductoProxy.SingleSearch(new ComprobanteProductoDTO()
            {
               ComprobanteId = model.ComprobanteDTO.Id,
               Estado = EstadoConstante.ACTIVO
            });
            model.ComprobanteDTO.ComprobanteProductoDTOList = new List<ComprobanteProductoDTO>
            {
               comprobanteProductoDTO
            };

            AuditDTO(model.ComprobanteDTO);
            comprobanteProxy.AnularComprobanteAnticipo(model.ComprobanteDTO);

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
      public FileContentResult ReportTo(string format, string cid, string comprobanteId)
      {
         byte[] reportData = obtenerReporteData(comprobanteId);

         return appHelper.ReportToPdf(reportData, "VT_ComprobanteE");
      }
      private byte[] obtenerReporteData(string comprobanteId)
      {
         Dictionary<string, string> parameters = new()
         {
            { "comprobanteId", comprobanteId }
         };

         string formatoReporte = "VT_ComprobanteE";

         byte[] reportData = appHelper.ExecuteReport(formatoReporte, parameters, ReportFormatConstant.PDF);

         return reportData;
      }
      [HttpPost("Cerrar")]
      public ActionResult Cerrar([FromBody] ComproAnticipoViewModel model)
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
      [HttpPost("CalcularMontos")]
      public ActionResult CalcularMontos([FromBody] ComproAnticipoViewModel model)
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

      //[HttpPost("AplicarTasaPorMedioPago")]
      //public ActionResult AplicarTasaPorMedioPago([FromBody] ComproAnticipoViewModel model)
      //{
      //   Logger.InfoStart();

      //   ResultResponse resultResponse = new();

      //   try
      //   {
      //      validarComprobanteDisponible(null, model.ComprobanteDTO.ComprobanteId);

      //      comprobanteProxy.AplicarTasaPorMedioPago(model.ComprobanteDTO);
      //   }
      //   catch (Exception e)
      //   {
      //      Logger.Error(e);
      //      resultResponse.Message = e.Message;
      //      resultResponse.Success = false;
      //   }

      //   Logger.InfoFinish();

      //   return Json(resultResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
      //}
   }
}
