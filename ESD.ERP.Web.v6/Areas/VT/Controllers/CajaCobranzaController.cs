using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.FE.Service;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.Web.Services;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
    [Area("VT")]
    [Route("[controller]")]
    public class CajaCobranzaController : AuthorizedController
    {
        private readonly IEstadoComprobanteProxy estadoComprobanteProxy;
        private readonly IComprobanteProxy comprobanteProxy;
        private readonly ICajaPersonalProxy cajaPersonalProxy;
        private readonly IMedioPagoProxy medioPagoProxy;
        private readonly IComprobanteProductoProxy comprobanteProductoProxy;
        private readonly IMonedaProxy monedaProxy;
        private readonly IListaPrecioProxy listaPrecioProxy;
        private readonly IParametroSistemaProxy parametroSistemaProxy;
        private readonly ITipoCondicionPagoProxy tipoCondicionPagoProxy;
        private readonly IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy;
        private readonly IDocumentoComercialSerieProxy documentoComercialSerieProxy;
        private readonly IFacturacionService facturacionService;
        private readonly IAnticipoClienteProxy anticipoClienteProxy;
        private readonly INotaCreditoProxy notaCreditoProxy;
        private readonly ITipoDocumentoComercialProxy tipoDocumentoComercialProxy;
        private readonly IImpuestoTasaProxy impuestoTasaProxy;
        private readonly ITipoComprobanteProxy tipoComprobanteProxy;
        private readonly ITipoDetraccionProxy tipoDetraccionProxy;
        private readonly IProductoProxy productoProxy;
        private readonly ITipoAfectacionIgvProxy tipoAfectacionIgvProxy;
        private readonly IAlmacenProxy almacenProxy;
        private readonly INotifyService notifiyService;
        private readonly ITipoDescuentoProxy tipoDescuentoProxy;
        private readonly AppHelper appHelper;

        public CajaCobranzaController(
           IEstadoComprobanteProxy estadoComprobanteProxy,
           IComprobanteProxy comprobanteProxy,
           ICajaPersonalProxy cajaPersonalProxy,
           IMedioPagoProxy medioPagoProxy,
           IComprobanteProductoProxy comprobanteProductoProxy,
           IMonedaProxy monedaProxy,
           IListaPrecioProxy listaPrecioProxy,
           IParametroSistemaProxy parametroSistemaProxy,
           ITipoCondicionPagoProxy tipoCondicionPagoProxy,
           IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy,
           IDocumentoComercialSerieProxy documentoComercialSerieProxy,
           IFacturacionService facturacionService,
           IAnticipoClienteProxy anticipoClienteProxy,
           INotaCreditoProxy notaCreditoProxy,
           ITipoDocumentoComercialProxy tipoDocumentoComercialProxy,
           IImpuestoTasaProxy impuestoTasaProxy,
           ITipoComprobanteProxy tipoComprobanteProxy,
           ITipoDetraccionProxy tipoDetraccionProxy,
           IProductoProxy productoProxy,
           ITipoAfectacionIgvProxy tipoAfectacionIgvProxy,
           IAlmacenProxy almacenProxy,
           INotifyService notifiyService,
           ITipoDescuentoProxy tipoDescuentoProxy,
           AppHelper appHelper
           )
        {
            this.estadoComprobanteProxy = estadoComprobanteProxy;
            this.comprobanteProxy = comprobanteProxy;
            this.cajaPersonalProxy = cajaPersonalProxy;
            this.medioPagoProxy = medioPagoProxy;
            this.comprobanteProductoProxy = comprobanteProductoProxy;
            this.monedaProxy = monedaProxy;
            this.listaPrecioProxy = listaPrecioProxy;
            this.parametroSistemaProxy = parametroSistemaProxy;
            this.tipoCondicionPagoProxy = tipoCondicionPagoProxy;
            this.periodoEmpresaSedeProxy = periodoEmpresaSedeProxy;
            this.documentoComercialSerieProxy = documentoComercialSerieProxy;
            this.facturacionService = facturacionService;
            this.anticipoClienteProxy = anticipoClienteProxy;
            this.notaCreditoProxy = notaCreditoProxy;
            this.tipoDocumentoComercialProxy = tipoDocumentoComercialProxy;
            this.impuestoTasaProxy = impuestoTasaProxy;
            this.tipoComprobanteProxy = tipoComprobanteProxy;
            this.tipoDetraccionProxy = tipoDetraccionProxy;
            this.productoProxy = productoProxy;
            this.tipoAfectacionIgvProxy = tipoAfectacionIgvProxy;
            this.almacenProxy = almacenProxy;
            this.notifiyService = notifiyService;
            this.tipoDescuentoProxy = tipoDescuentoProxy;
            this.appHelper = appHelper;
        }
        [Route("[action]")]
        public IActionResult Index()
        {
            try
            {
                CajaCobranzaViewModel cajaCobranzaViewModel = new()
                {
                    ComprobanteDTO = new ComprobanteDTO
                    {
                        FechaDesde = DateTime.Today,
                        FechaHasta = DateTime.Today
                    }
                };

                cargarListaEstadoComprobante(cajaCobranzaViewModel);

                int periodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId();

                PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = periodoEmpresaSedeProxy.GetById(new PeriodoEmpresaSedeDTO()
                {
                    PeriodoEmpresaSedeId = periodoEmpresaSedeId
                });

                CajaPersonalDTO cajaPersonalDTO = cajaPersonalProxy.SingleSearch(new CajaPersonalDTO()
                {
                    CajaDTO = new CajaDTO() { EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId },
                    PersonalDTO = new PersonalDTO() { ProveedorDTO = new ProveedorDTO() { UsuarioId = ObtenerUsuarioId() } },
                    Estado = EstadoConstante.ACTIVO
                });

                if (cajaPersonalDTO != null)
                    cajaCobranzaViewModel.ComprobanteDTO.CajaId = cajaPersonalDTO.CajaId;

                cajaCobranzaViewModel.MenuItemList = new List<MenuItem>
            {
               new MenuItem() { MenuItemType = MenuItemType.Edit },
               new MenuItem() { Key = "emitir", Content = "Emitir", Disabled = true},
               new MenuItem() { MenuItemType = MenuItemType.Delete },
               new MenuItem() { MenuItemType = MenuItemType.Preview }
            };

                cargarGridModel(cajaCobranzaViewModel);

                return PartialView(cajaCobranzaViewModel);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }
        [HttpPost("ListarBandeja")]
        public ActionResult ListarBandeja(string jsonModel)
        {

            Logger.InfoStart();

            CajaCobranzaViewModel model = JsonSerializer.Deserialize<CajaCobranzaViewModel>(jsonModel);

            model.ComprobanteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.ComprobanteDTO.OrderBy = "Fecha desc,Numero desc";

            model.ComprobanteDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
            if (model.ComprobanteDTO.EstadoComprobanteId == 0)
            {
                List<string> list = new()
            {
               EstadoComprobanteConstante.CERRADO,
               EstadoComprobanteConstante.EMITIDO,
               EstadoComprobanteConstante.ANULADO
            };

                model.ComprobanteDTO.EstadoComprobanteCodigoList = list;
            }

            try
            {
                string result = comprobanteProxy.ListarCobranzaGrid(model.ComprobanteDTO);

                object jsonData = JsonSerializer.Deserialize(result, typeof(object));

                Logger.InfoFinish();
                return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }
        private void cargarGridModel(CajaCobranzaViewModel model)
        {
            string colName = ",,T/D,Serie-Numero,Fecha,Cliente,#Doc,,#Pedido,Vendedor,Mon.,Total,,Estado,,Estado SUNAT,,";

            string colModel = "["
               + "{\"name\":\"ComprobanteId\",\"hidden\":true},"
               + "{\"name\":\"TipoDocumentoComercial\",\"hidden\":true},"
               + "{\"name\":\"TipoDocumentoComercialNombre\",\"width\":\"60\"},"
               + "{\"name\":\"SerieNumero\",\"width\":\"100\"},"
               + "{\"name\":\"Fecha\",\"width\":\"70\",\"align\":\"right\",\"sorttype\":\"date\",\"datefmt\":\"" + JavaScriptConstant.GRID_DATE_FORMAT + "\"},"
               + "{\"name\":\"ProveedorNombre\",\"width\":\"300\"},"
               + "{\"name\":\"NumeroDocumentoIdentidad\",\"width\":\"100\"},"
               + "{\"name\":\"CotizacionId\",\"hidden\":true},"
               + "{\"name\":\"Pedido\",\"width\":\"70\"},"
               + "{\"name\":\"VendedorNombre\",\"width\":\"250\"},"
               + "{\"name\":\"MonedaSimbolo\",\"width\":\"40\"},"
               + "{\"name\":\"MontoTotal\",\"width\":\"100\",\"align\":\"right\",\"formatter\":\"number\",\"sorttype\":\"number\"},"
               + "{\"name\":\"EstadoComprobante\",\"hidden\":true},"
               + "{\"name\":\"EstadoComprobanteNombre\",\"width\":\"120\"},"
               + "{\"name\":\"EstadoSunat\",\"hidden\":true},"
               + "{\"name\":\"EstadoSunatNombre\",\"width\":\"120\"},"
               + "{\"name\":\"CajaId\",\"hidden\":true},"
               + "{\"name\":\"Estado\",\"hidden\":true}";
            colModel += "]";

            model.GridColNames = colName;
            model.GridColModel = colModel;
        }
        [HttpPost("Editar")]
        public ActionResult Editar([FromBody] CajaCobranzaViewModel model)
        {

            Logger.InfoStart();

            CajaCobranzaViewModel cajaCobranzaViewModel = new()
            {
                EditAction = ViewModelEditAction.Edit
            };

            try
            {
                cargarDataParaEditar(cajaCobranzaViewModel, model.ComprobanteDTO.Id);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, cajaCobranzaViewModel);
        }
        private void cargarListaEstadoComprobante(CajaCobranzaViewModel model)
        {
            List<string> list = new()
         {
            EstadoComprobanteConstante.CERRADO,
            EstadoComprobanteConstante.EMITIDO,
            EstadoComprobanteConstante.ANULADO
         };

            List<EstadoComprobanteDTO> estadoComprobanteDTOList = estadoComprobanteProxy.Search(new EstadoComprobanteDTO()
            {
                CodigoList = list,
                Estado = EstadoConstante.ACTIVO
            });
            model.EstadoComprobanteList = WebHelper.ToSelectListItem(estadoComprobanteDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

            EstadoComprobanteDTO estadoComprobanteDTO = estadoComprobanteProxy.ObtenerPorCodigo(new EstadoComprobanteDTO()
            {
                Codigo = EstadoComprobanteConstante.CERRADO
            });

            WebHelper.SelectedListItemSetValue(model.EstadoComprobanteList, estadoComprobanteDTO.Id);
        }
        private void cargarDataParaEditar(CajaCobranzaViewModel model, int id)
        {

            ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = id });
            model.ComprobanteDTO = comprobanteDTO;

            ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.SingleSearch(new ParametroSistemaDTO()
            {
                Codigo = ParametroSistemaConstante.VENTAS_PORCENTAJE_INCREMENTO_PU_SI_LA_CANTIDAD_ES_FRACCION
            });

            model.PorcentajeFraccionPU = parametroSistemaDTO.ValorNumerico;

            cargarListas(model);

            CajaPersonalDTO cajaPersonalDTO = cajaPersonalProxy.SingleSearch(new CajaPersonalDTO()
            {
                CajaDTO = new CajaDTO() { EmpresaSedeId = ObtenerEmpresaId() },
                PersonalDTO = new PersonalDTO() { ProveedorDTO = new ProveedorDTO() { UsuarioId = ObtenerUsuarioId() } },
                Estado = EstadoConstante.ACTIVO
            });

            if (cajaPersonalDTO != null)
                model.ComprobanteDTO.CajaId = cajaPersonalDTO.CajaId;

            WebHelper.SelectedListItemSetValue(model.MonedaList, comprobanteDTO.MonedaId);
            WebHelper.SelectedListItemSetValue(model.ListaPrecioList, comprobanteDTO.ListaPrecioId);
            WebHelper.SelectedListItemSetValue(model.TipoCondicionPagoList, comprobanteDTO.TipoCondicionPagoId);
            WebHelper.SelectedListItemSetValue(model.MedioPagoList, comprobanteDTO.MedioPagoId);

            model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key = "emitir", Content = "Emitir"},
            new MenuItem() { MenuItemType = MenuItemType.Exit}
         };
        }
        private void cargarListas(CajaCobranzaViewModel model)
        {

            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
            model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

            if (model.ComprobanteDTO.MonedaId == 0)
                model.ListaPrecioList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Select);
            else
            {
                List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.ComprobanteDTO.MonedaId);

                model.ListaPrecioList = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.Nombre,
                   SelectListFirstOptionType.Select);
            }

            List<TipoCondicionPagoDTO> tipoCondicionPagoDTOList = tipoCondicionPagoProxy.Search(new TipoCondicionPagoDTO() { Estado = EstadoConstante.ACTIVO });
            model.TipoCondicionPagoList = WebHelper.ToSelectListItem(tipoCondicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

            List<MedioPagoDTO> medioPagoDTOList = medioPagoProxy.Search(new MedioPagoDTO()
            {
                EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
                Estado = EstadoConstante.ACTIVO
            });
            model.MedioPagoList = WebHelper.ToSelectListItem(medioPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
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
        [HttpPost("Eliminar")]
        public ActionResult Eliminar([FromBody] CajaCobranzaViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.ComprobanteDTO.DTOState = DTOState.Modified;

                ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(model.ComprobanteDTO);

                EstadoComprobanteDTO estadoComprobanteDTO = estadoComprobanteProxy.ObtenerPorCodigo(new EstadoComprobanteDTO()
                {
                    Codigo = EstadoComprobanteConstante.ANULADO
                });

                comprobanteDTO.EstadoComprobanteId = estadoComprobanteDTO.Id;
                AuditDTO(comprobanteDTO);

                comprobanteProxy.Delete(comprobanteDTO);

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
        [HttpPost("EmitirEditPopup")]
        public ActionResult EmitirEditPopup([FromBody] CajaCobranzaViewModel model)
        {

            try
            {
                ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = model.ComprobanteDTO.Id });

                model.ComprobanteDTO = comprobanteDTO;

                List<MedioPagoDTO> medioPagoDTOList = medioPagoProxy.Search(new MedioPagoDTO()
                {
                    EmpresaId = comprobanteDTO.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
                    Estado = EstadoConstante.ACTIVO
                });
                model.MedioPagoList = WebHelper.ToSelectListItem(medioPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
                   SelectListFirstOptionType.Select);

                if (medioPagoDTOList.Count > 0)
                    WebHelper.SelectedListItemSetValue(model.MedioPagoList, comprobanteDTO.MedioPagoId);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            return PartialView(model);
        }
        [HttpPost("Emitir")]
        public ActionResult Emitir([FromBody] CajaCobranzaViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                AuditDTO(model.ComprobanteDTO);

                comprobanteProxy.Emitir(model.ComprobanteDTO);

                ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = model.ComprobanteDTO.Id });

                generarDocumentoElectronico(comprobanteDTO);

                notifiyService.SendNotification(new NotificacionPushDTO()
                {
                    UsuarioId = comprobanteDTO.VendedorDTO.ProveedorDTO.UsuarioId.Value,
                    Titulo = SR.GetString("VT_comprobante_cobranza_notificacion_titulo"),
                    Descripcion = SR.GetString("VT_comprobante_cobranza_notificacion_descripcion", comprobanteDTO.SerieNumero,
                      comprobanteDTO.ProveedorDTO.RazonSocial)
                });

                resultResponse.Message = SR.GetString("VT_comprobante_emitido", comprobanteDTO.SerieNumero);
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

            CajaCobranzaViewModel model = JsonSerializer.Deserialize<CajaCobranzaViewModel>(jsonModel);

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
        public ActionResult CalcularMontos([FromBody] CajaCobranzaViewModel model)
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
        private void generarDocumentoElectronico(ComprobanteDTO comprobanteDTO)
        {
            if (comprobanteDTO.TipoDocumentoComercialDTO.EsInterno == SiNoConstante.S)
                return;

            if (comprobanteDTO.TipoDocumentoComercialDTO.EsElectronico == SiNoConstante.N)
                return;

            DocumentoComercialSerieDTO documentoComercialSerieDTO = documentoComercialSerieProxy.SingleSearch(new DocumentoComercialSerieDTO()
            {
                EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = comprobanteDTO.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
                TipoDocumentoComercialId = comprobanteDTO.TipoDocumentoComercialId,
                Serie = comprobanteDTO.Serie
            });

            List<ParametroSistemaDTO> parametroSistemaDTOList = appHelper
               .ObtenerGrupoParametroSistema(GrupoParametroSistemaConstante.FACTURACION_ELECTRONICA);

            ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.
               FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.FACTURACION_E_URL && i.Estado == EstadoConstante.ACTIVO);

            if (parametroSistemaDTO == null)
                return;

            facturacionService.GenerarEnviarDocumento(parametroSistemaDTO.ValorCadena, comprobanteDTO.TipoDocumentoComercialDTO.Codigo,
               comprobanteDTO.Id, documentoComercialSerieDTO.EnvioEnLinea);

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

            ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = Convert.ToInt32(comprobanteId) });

            string formatoReporte = string.Empty;

            if (comprobanteDTO.TipoDocumentoComercialDTO.EsInterno == SiNoConstante.N)
               formatoReporte = comprobanteDTO.MontoDetraccion == decimal.Zero ? "VT_ComprobanteE" : "VT_InvoiceWithDetraction";
            else
               formatoReporte = "VT_InternoE";

            byte[] reportData = appHelper.ExecuteReport(formatoReporte, parameters, ReportFormatConstant.PDF);

            return reportData;
        }
        [HttpGet("ReporteEmitido")]
        public FileContentResult ReporteEmitido(string format, string cid, string comprobanteId,
           string copia1, string copia2, string copia3)
        {
            byte[] reportData = obtenerReporteData(comprobanteId);

            List<MemoryStream> msList = new()
         {
            new MemoryStream(reportData)
         };

            if (copia2 == SiNoConstante.S)
                msList.Add(new MemoryStream(reportData));

            if (copia3 == SiNoConstante.S)
                msList.Add(new MemoryStream(reportData));

            List<PdfWaterMark> wmList = new()
         {
            new PdfWaterMark()
         };

            if (copia2 == SiNoConstante.S)
                wmList.Add(new PdfWaterMark() { WaterMark = "CONTROL ADMINISTRATIVO", Position = 20 });

            if (copia3 == SiNoConstante.S)
                wmList.Add(new PdfWaterMark() { WaterMark = "DESPACHO", Position = 20 });

            byte[] mergePdf = appHelper.MergePDFFiles(msList.ToArray(), wmList.ToArray());

            return appHelper.ReportToPdf(mergePdf, "VT_ComprobanteE");
        }
        [HttpPost("EmitirAnticipoEditPopup")]
        public ActionResult EmitirAnticipoEditPopup([FromBody] CajaCobranzaViewModel model)
        {
            try
            {
                model.AnticipoClienteDTO = anticipoClienteProxy.GetById(new AnticipoClienteDTO() { AnticipoClienteId = model.AnticipoClienteDTO.Id });

                List<MedioPagoDTO> medioPagoDTOList = medioPagoProxy.Search(new MedioPagoDTO()
                {
                    EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
                    Estado = EstadoConstante.ACTIVO
                });
                model.MedioPagoList = WebHelper.ToSelectListItem(medioPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
                   SelectListFirstOptionType.Select);
                WebHelper.SelectedListItemSetValue(model.MedioPagoList, model.AnticipoClienteDTO.MedioPagoId);

                List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = tipoDocumentoComercialProxy.Search(new TipoDocumentoComercialDTO()
                {
                    TipoDocumentoComercialList = new List<string>() { TipoDocumentoComercialConstante.FACTURA, TipoDocumentoComercialConstante.BOLETA_VENTA },
                    Estado = EstadoConstante.ACTIVO
                });
                model.TipoDocumentoComercialList = WebHelper.ToSelectListItem(tipoDocumentoComercialDTOList, x => x.Id.ToString(), x => x.Nombre,
                   SelectListFirstOptionType.Blank);
                WebHelper.SelectedListItemSetValue(model.TipoDocumentoComercialList, model.AnticipoClienteDTO.TipoDocumentoComercialId);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            return PartialView(model);
        }
        [HttpPost("EmitirAnticipoCliente")]
        public ActionResult EmitirAnticipoCliente([FromBody] CajaCobranzaViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {

                AuditDTO(model.AnticipoClienteDTO);

                AnticipoClienteDTO anticipoClienteDTO = anticipoClienteProxy.GetById(new AnticipoClienteDTO() { AnticipoClienteId = model.AnticipoClienteDTO.Id });

                ComprobanteDTO comprobanteDTO = cargarComprobanteAnticipoDTO(model, anticipoClienteDTO);
                model.AnticipoClienteDTO.ComprobanteDTO = comprobanteDTO;

                anticipoClienteProxy.Emitir(model.AnticipoClienteDTO);

                anticipoClienteDTO = anticipoClienteProxy.GetById(new AnticipoClienteDTO() { AnticipoClienteId = model.AnticipoClienteDTO.Id });
                if (model.AnticipoClienteDTO.TipoDocumentoComercialId.HasValue)
                {
                    comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = anticipoClienteDTO.ComprobanteId.Value });

                    generarDocumentoElectronico(comprobanteDTO);
                }

                notifiyService.SendNotification(new NotificacionPushDTO()
                {
                    UsuarioId = anticipoClienteDTO.VendedorDTO.ProveedorDTO.UsuarioId.Value,
                    Titulo = SR.GetString("VT_anticipo_cobranza_notificacion_titulo"),
                    Descripcion = SR.GetString("VT_anticipo_cobranza_notificacion_descripcion", anticipoClienteDTO.NumeroOperacion,
                      anticipoClienteDTO.ProveedorDTO.RazonSocial)
                });

                resultResponse.Message = SR.GetString("VT_anticipo_emitido", anticipoClienteDTO.NumeroOperacion);
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
        private ComprobanteDTO cargarComprobanteAnticipoDTO(CajaCobranzaViewModel model, AnticipoClienteDTO anticipoClienteDTO)
        {

            if (!anticipoClienteDTO.TipoDocumentoComercialId.HasValue)
                return null;

            ComprobanteDTO comprobanteDTO = new();

            try
            {
                PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

                comprobanteDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;
                comprobanteDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;
                comprobanteDTO.TipoDocumentoComercialId = anticipoClienteDTO.TipoDocumentoComercialId.Value;
                comprobanteDTO.Serie = anticipoClienteDTO.Serie;
                comprobanteDTO.Numero = string.Empty;
                comprobanteDTO.Fecha = anticipoClienteDTO.Fecha;
                comprobanteDTO.CotizacionId = anticipoClienteDTO.CotizacionId;
                comprobanteDTO.ProveedorId = anticipoClienteDTO.ProveedorId;
                comprobanteDTO.NombreAlterno = string.Empty;
                comprobanteDTO.VendedorId = anticipoClienteDTO.VendedorId;
                comprobanteDTO.MonedaId = anticipoClienteDTO.MonedaId;

                ListaPrecioDTO listaPrecioDTO = listaPrecioProxy.SingleSearch(new ListaPrecioDTO()
                {
                    EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
                    EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                    MonedaId = anticipoClienteDTO.MonedaId,
                    Estado = EstadoConstante.ACTIVO
                });
                comprobanteDTO.ListaPrecioId = listaPrecioDTO.Id;

                TipoCondicionPagoDTO tipoCondicionPagoDTO = tipoCondicionPagoProxy.ObtenerPorCodigo(new TipoCondicionPagoDTO()
                {
                    Codigo = TipoCondicionPagoConstante.CONTADO
                });
                comprobanteDTO.TipoCondicionPagoId = tipoCondicionPagoDTO.Id;

                AlmacenDTO almacenDTO = almacenProxy.SingleSearch(new AlmacenDTO()
                {
                    EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
                    EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                    EsVenta = SiNoConstante.S,
                    Estado = EstadoConstante.ACTIVO
                });
                comprobanteDTO.AlmacenId = almacenDTO.Id;

                comprobanteDTO.Observacion = string.Empty;
                comprobanteDTO.MedioPagoId = anticipoClienteDTO.MedioPagoId;

                ImpuestoTasaDTO impuestoTasaDTO = impuestoTasaProxy.SingleSearch(new ImpuestoTasaDTO()
                {
                    TipoImpuestoDTO = new TipoImpuestoDTO() { Codigo = TipoImpuestoConstante.IGV },
                    Periodo = periodoEmpresaSedeDTO.PeriodoDTO.Codigo
                });
                comprobanteDTO.ImpuestoTasaId = impuestoTasaDTO.Id;

                comprobanteDTO.NumeroGuiaRemision = string.Empty;
                comprobanteDTO.Canjeado = SiNoConstante.N;

                TipoComprobanteDTO tipoComprobanteDTO = tipoComprobanteProxy.ObtenerPorCodigo(new TipoComprobanteDTO()
                {
                    Codigo = TipoComprobanteConstante.ANTICIPO
                });
                comprobanteDTO.TipoComprobanteId = tipoComprobanteDTO.Id;

                TipoDetraccionDTO tipoDetraccionDTO = tipoDetraccionProxy.ObtenerPorCodigo(new TipoDetraccionDTO()
                {
                    Codigo = TipoDetraccionConstante.SIN_DETRACCION
                });
                comprobanteDTO.TipoDetraccionId = tipoDetraccionDTO.Id;

                comprobanteDTO.NumeroOperacionPago = string.Empty;
                comprobanteDTO.EsContingencia = SiNoConstante.N;
                comprobanteDTO.Emitido = SiNoConstante.N;
                comprobanteDTO.CodigoHash = string.Empty;
                comprobanteDTO.Bancarizado = SiNoConstante.N;
                comprobanteDTO.ObservacionDespacho = string.Empty;
                comprobanteDTO.EntregaDespacho = string.Empty;
                comprobanteDTO.Estado = EstadoConstante.ACTIVO;

                EstadoComprobanteDTO estadoComprobanteDTO = estadoComprobanteProxy.ObtenerPorCodigo(new EstadoComprobanteDTO()
                {
                    Codigo = EstadoComprobanteConstante.PENDIENTE
                });
                comprobanteDTO.EstadoComprobanteId = estadoComprobanteDTO.Id;

                AuditDTO(comprobanteDTO);

                ProductoDTO productoDTO = productoProxy.SingleSearch(new ProductoDTO()
                {
                    Codigo = ProductoConstante.ANTICIPO,
                    EsOperacion = SiNoConstante.S,
                    Estado = EstadoConstante.ACTIVO
                });

                TipoDescuentoDTO tipoDescuentoDTO = tipoDescuentoProxy.ObtenerPorCodigo(new TipoDescuentoDTO() { Codigo = TipoDescuentoConstante.SIN_DESCUENTO });

                ComprobanteProductoDTO comprobanteProductoDTO = new()
                {
                    ProductoDTO = productoDTO,
                    ProductoId = productoDTO.ProductoId,
                    Cantidad = decimal.One,
                    CantidadBloqueada = SiNoConstante.N,
                    IncluyeDescuentoListaPrecio = SiNoConstante.N,
                    SolicitaAutorizacionDescuento = SiNoConstante.N,
                    PermiteDescuento = SiNoConstante.N,
                    TipoDescuentoId = tipoDescuentoDTO.TipoDescuentoId,
                    TipoDetraccionId = tipoDetraccionDTO.Id,
                    ObservacionDespacho = string.Empty,
                    EntregaDespacho = string.Empty,
                    Estado = EstadoConstante.ACTIVO
                };

                comprobanteProductoDTO.Descripcion = anticipoClienteDTO.Observacion;

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

                CalculadorVentaItemDTO calculadorVentaItem = new()
                {
                    TipoIGV = tipoIgv,
                    Cantidad = decimal.One,
                    PrecioUnitario = anticipoClienteDTO.Monto,
                    ImpuestoTasa = impuestoTasaDTO.Tasa,
                };

                calculadorVentaItem = CalculadorVenta.CalcularMontos(calculadorVentaItem);

                comprobanteProductoDTO.PrecioUnitario = calculadorVentaItem.PrecioUnitario;
                comprobanteProductoDTO.ValorUnitario = calculadorVentaItem.ValorUnitario;
                comprobanteProductoDTO.MontoBruto = calculadorVentaItem.MontoBruto;
                comprobanteProductoDTO.MontoNeto = calculadorVentaItem.MontoNeto;
                comprobanteProductoDTO.MontoIgv = calculadorVentaItem.MontoImpuesto;
                comprobanteProductoDTO.MontoTotal = calculadorVentaItem.MontoTotal;

                comprobanteDTO.MontoBruto = calculadorVentaItem.MontoBruto;
                comprobanteDTO.MontoNeto = calculadorVentaItem.MontoNeto;
                comprobanteDTO.MontoIgv = calculadorVentaItem.MontoImpuesto;
                comprobanteDTO.MontoTotal = calculadorVentaItem.MontoTotal;

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
        [HttpPost("EmitirNCEditPopup")]
        public ActionResult EmitirNCEditPopup([FromBody] CajaCobranzaViewModel model)
        {
            return PartialView(model);
        }
        [HttpPost("EmitirNotaCredito")]
        public ActionResult EmitirNotaCredito([FromBody] CajaCobranzaViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                AuditDTO(model.NotaCreditoDTO);

                notaCreditoProxy.Emitir(model.NotaCreditoDTO);

                NotaCreditoDTO notaCreditoDTO = notaCreditoProxy.GetById(new NotaCreditoDTO() { NotaCreditoId = model.NotaCreditoDTO.Id });

                generarDocumentoElectronico(notaCreditoDTO);

                resultResponse.Message = SR.GetString("VT_nota_credito_emitida", notaCreditoDTO.SerieNumero);
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
        [HttpPost("EmitirNCInternoEditPopup")]
        public ActionResult EmitirNCInternoEditPopup([FromBody] CajaCobranzaViewModel model)
        {
            return PartialView(model);
        }
        [HttpPost("EmitirNotaCreditoInterno")]
        public ActionResult EmitirNotaCreditoInterno([FromBody] CajaCobranzaViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                AuditDTO(model.NotaCreditoDTO);

                notaCreditoProxy.EmitirInterno(model.NotaCreditoDTO);

                NotaCreditoDTO notaCreditoDTO = notaCreditoProxy.GetById(new NotaCreditoDTO() { NotaCreditoId = model.NotaCreditoDTO.Id });

                generarDocumentoElectronico(notaCreditoDTO);

                resultResponse.Message = SR.GetString("VT_nota_credito_emitida", notaCreditoDTO.SerieNumero);
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
        private void generarDocumentoElectronico(NotaCreditoDTO notaCreditoDTO)
        {
            TipoDocumentoComercialDTO tipoDocumentoComercialDTO = tipoDocumentoComercialProxy.ObtenerPorCodigo(new TipoDocumentoComercialDTO()
            {
                Codigo = TipoDocumentoComercialConstante.NOTA_CREDITO
            });

            if (tipoDocumentoComercialDTO.EsInterno == SiNoConstante.S)
                return;

            if (tipoDocumentoComercialDTO.EsElectronico == SiNoConstante.N)
                return;

            DocumentoComercialSerieDTO documentoComercialSerieDTO = documentoComercialSerieProxy.SingleSearch(new DocumentoComercialSerieDTO()
            {
                EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = notaCreditoDTO.PeriodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
                TipoDocumentoComercialId = tipoDocumentoComercialDTO.TipoDocumentoComercialId,
                Serie = notaCreditoDTO.Serie
            });

            List<ParametroSistemaDTO> parametroSistemaDTOList = appHelper
               .ObtenerGrupoParametroSistema(GrupoParametroSistemaConstante.FACTURACION_ELECTRONICA);

            ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.
               FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.FACTURACION_E_URL && i.Estado == EstadoConstante.ACTIVO);

            if (parametroSistemaDTO == null)
                return;

            facturacionService.GenerarEnviarDocumento(parametroSistemaDTO.ValorCadena, tipoDocumentoComercialDTO.Codigo,
               notaCreditoDTO.Id, documentoComercialSerieDTO.EnvioEnLinea);

        }
    }
}
