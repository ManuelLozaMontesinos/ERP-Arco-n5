using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.FE.Service;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
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
    public class NotaCreditoInternoController : AuthorizedController
    {
        private readonly IEstadoComprobanteProxy estadoComprobanteProxy;
        private readonly ITipoNotaCreditoProxy tipoNotaCreditoProxy;
        private readonly INotaCreditoProxy notaCreditoProxy;
        private readonly INotaCreditoProductoProxy notaCreditoProductoProxy;
        private readonly IVendedorProxy vendedorProxy;
        private readonly IVendedorSerieProxy vendedorSerieProxy;
        private readonly IDocumentoComercialSerieProxy documentoComercialSerieProxy;
        private readonly ITipoDocumentoComercialProxy tipoDocumentoComercialProxy;
        private readonly IFacturacionService facturacionService;
        private readonly IComprobanteProductoProxy comprobanteProductoProxy;
        private readonly AppHelper appHelper;

        public NotaCreditoInternoController(
           IEstadoComprobanteProxy estadoComprobanteProxy,
           ITipoNotaCreditoProxy tipoNotaCreditoProxy,
           INotaCreditoProxy notaCreditoProxy,
           INotaCreditoProductoProxy notaCreditoProductoProxy,
           IVendedorProxy vendedorProxy,
           IVendedorSerieProxy vendedorSerieProxy,
           IDocumentoComercialSerieProxy documentoComercialSerieProxy,
           ITipoDocumentoComercialProxy tipoDocumentoComercialProxy,
           IComprobanteProductoProxy comprobanteProductoProxy,
           IFacturacionService facturacionService,
           AppHelper appHelper
           )
        {
            this.estadoComprobanteProxy = estadoComprobanteProxy;
            this.tipoNotaCreditoProxy = tipoNotaCreditoProxy;
            this.notaCreditoProxy = notaCreditoProxy;
            this.notaCreditoProductoProxy = notaCreditoProductoProxy;
            this.vendedorProxy = vendedorProxy;
            this.vendedorSerieProxy = vendedorSerieProxy;
            this.documentoComercialSerieProxy = documentoComercialSerieProxy;
            this.tipoDocumentoComercialProxy = tipoDocumentoComercialProxy;
            this.facturacionService = facturacionService;
            this.comprobanteProductoProxy = comprobanteProductoProxy;
            this.appHelper = appHelper;
        }
        [Route("[action]")]
        public IActionResult Index()
        {
            NotaCreditoInternoViewModel notaCreditoInternoViewModel = new();

            cargarListaTipoNotaCredito(notaCreditoInternoViewModel);
            cargarListaEstadoComprobante(notaCreditoInternoViewModel);

            return PartialView(notaCreditoInternoViewModel);
        }
        [HttpPost("ListarBandeja")]
        public ActionResult ListarBandeja(string jsonModel)
        {

            Logger.InfoStart();

            NotaCreditoInternoViewModel model = JsonSerializer.Deserialize<NotaCreditoInternoViewModel>(jsonModel);

            model.NotaCreditoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.NotaCreditoDTO.OrderBy = "nc.fecha desc,nc.numero_opera desc";    

            model.NotaCreditoDTO.ComprobanteDTO.TipoDocumentoComercialId = 28; 

            model.NotaCreditoDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

            try
            {
                List<NotaCreditoDTO> notaCreditoDTOList = notaCreditoProxy.Search(model.NotaCreditoDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = notaCreditoDTOList.Count,
                    rows = from f in notaCreditoDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.NotaCreditoId,
                                   f.SerieNumero,
                                   Fecha = f.Fecha.ToShortDateString(),
                                   TipoNotaCreditoNombre = f.TipoNotaCreditoDTO.Nombre,
                                   f.ComprobanteDTO.TipoDocumentoSerieNumero,
                                   f.ComprobanteDTO.ProveedorDTO.RazonSocial,
                                   f.ComprobanteDTO.ProveedorDTO.NumeroDocumentoIdentidad,
                                   Moneda = f.ComprobanteDTO.MonedaDTO.Simbolo,
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
        public ActionResult Crear([FromBody] NotaCreditoInternoViewModel model)
        {
            Logger.InfoStart();

            NotaCreditoInternoViewModel notaCreditoInternoViewModel = new()
            {
                EditAction = ViewModelEditAction.New,
                IsNew = true,
            };

            try
            {
                cargarDataParaEditar(notaCreditoInternoViewModel, 0);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, notaCreditoInternoViewModel);
        }
        [HttpPost("Editar")]
        public ActionResult Editar([FromBody] NotaCreditoInternoViewModel model)
        {

            Logger.InfoStart();

            NotaCreditoInternoViewModel notaCreditoInternoViewModel = new()
            {
                EditAction = ViewModelEditAction.Edit
            };

            try
            {
                cargarDataParaEditar(notaCreditoInternoViewModel, model.NotaCreditoDTO.Id);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }

            Logger.InfoFinish();

            return PartialView(PartialViewNameConstant.CRUD, notaCreditoInternoViewModel);
        }
        private void cargarDataParaEditar(NotaCreditoInternoViewModel model, int id)
        {

            NotaCreditoDTO notaCreditoDTO;
            if (model.EditAction == ViewModelEditAction.New)
            {
                notaCreditoDTO = new NotaCreditoDTO()
                {
                    TipoNotaCreditoDTO = new TipoNotaCreditoDTO(),
                    Fecha = DateTime.Today,
                    ComprobanteDTO = new ComprobanteDTO()
                    {
                        TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO(),
                        MonedaDTO = new MonedaDTO(),
                        ProveedorDTO = new ProveedorDTO(),
                        TipoCondicionPagoDTO = new TipoCondicionPagoDTO(),
                        MedioPagoDTO = new MedioPagoDTO()
                    },
                    ImpuestoTasaDTO = new ImpuestoTasaDTO()
                };

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
                    notaCreditoDTO.VendedorDTO = vendedorDTO;
                    notaCreditoDTO.VendedorId = vendedorDTO.Id;
                }
                else
                    notaCreditoDTO.VendedorDTO = new VendedorDTO() { ProveedorDTO = new ProveedorDTO() { } };

                model.SerieList = WebHelper.ToSelectListItem(SelectListFirstOptionType.Blank);

                EstadoComprobanteDTO estadoComprobanteDTO = estadoComprobanteProxy.ObtenerPorCodigo(new EstadoComprobanteDTO() { Codigo = EstadoComprobanteConstante.PENDIENTE });
                notaCreditoDTO.EstadoComprobanteId = estadoComprobanteDTO.Id;
                notaCreditoDTO.EstadoComprobanteDTO = estadoComprobanteDTO;
            }
            else
            {
                notaCreditoDTO = notaCreditoProxy.GetById(new NotaCreditoDTO() { NotaCreditoId = id });
            }

            model.NotaCreditoDTO = notaCreditoDTO;

            cargarListas(model);

            WebHelper.SelectedListItemSetValue(model.TipoNotaCreditoList, notaCreditoDTO.TipoNotaCreditoId);

            MenuBarTagHelper menuBarTagHelper = new();
            model.MenuItemList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.SaveExit);

            model.MenuItemList.Add(new MenuItem() { Key = "cerrar", Content = "Cerrar Nota" });
        }
        private void cargarListaEstadoComprobante(NotaCreditoInternoViewModel model)
        {
            List<EstadoComprobanteDTO> estadoComprobanteDTOList = estadoComprobanteProxy.Search(new EstadoComprobanteDTO() { Estado = EstadoConstante.ACTIVO });
            model.EstadoComprobanteList = WebHelper.ToSelectListItem(estadoComprobanteDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
        }
        private void cargarListaTipoNotaCredito(NotaCreditoInternoViewModel model)
        {
            List<TipoNotaCreditoDTO> tipoNotaCreditoDTOList = tipoNotaCreditoProxy.Search(new TipoNotaCreditoDTO() { Estado = EstadoConstante.ACTIVO });
            model.TipoNotaCreditoList = WebHelper.ToSelectListItem(tipoNotaCreditoDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);
        }
        private void cargarListas(NotaCreditoInternoViewModel model)
        {
            cargarListaTipoNotaCredito(model);
        }
        [HttpPost("ListarSerie")]
        public JsonResult ListarSerie([FromBody] NotaCreditoInternoViewModel model)
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
                        TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO() { Codigo = TipoDocumentoComercialConstante.NOTA_CREDITO },
                        TipoDocumentoComercialRelacionadoId = model.ComprobanteDTO.TipoDocumentoComercialId
                    },
                    Estado = EstadoConstante.ACTIVO
                });

                string esContingencia = vendedorSerieDTOList.Count >= 1 ? vendedorSerieDTOList.FirstOrDefault().DocumentoComercialSerieDTO.EsContingencia
                   : string.Empty;

                resultResponse.Data = new
                {
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
        [HttpGet("ObtenerTipoNotaCredito")]
        public JsonResult ObtenerTipoNotaCredito(int id)
        {

            Logger.InfoStart();
            ResultResponse resultResponse = new();

            try
            {
                TipoNotaCreditoDTO tipoNotaCreditoDTO = tipoNotaCreditoProxy.GetById(new TipoNotaCreditoDTO() { TipoNotaCreditoId = id });

                resultResponse.Data = new
                {
                    tipoNotaCreditoDTO.AfectaAlmacen,
                    tipoNotaCreditoDTO.EsIngreso,
                    tipoNotaCreditoDTO.PermiteEditItem,
                    tipoNotaCreditoDTO.PermiteEditarCantidad,
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
        public ActionResult Grabar([FromBody] NotaCreditoInternoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                NotaCreditoDTO notaCreditoDTO = cargarNotaCreditoDTO(model);

                int newId = model.NotaCreditoDTO.Id;
                string messageResourceKey = string.Empty;
                switch (model.EditAction)
                {
                    case ViewModelEditAction.New:
                        newId = notaCreditoProxy.Insert(notaCreditoDTO);
                        messageResourceKey = "VT_nota_credito_agregada";
                        break;

                    case ViewModelEditAction.Edit:
                        notaCreditoProxy.Update(notaCreditoDTO);
                        messageResourceKey = "VT_nota_credito_actualizada";
                        break;
                }

                notaCreditoDTO = notaCreditoProxy.GetById(new NotaCreditoDTO() { NotaCreditoId = newId });

                resultResponse.Message = SR.GetString(messageResourceKey, notaCreditoDTO.SerieNumero);
                resultResponse.Data = new
                {
                    notaCreditoDTO.Id,
                    notaCreditoDTO.NumeroOperacion
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
        private NotaCreditoDTO cargarNotaCreditoDTO(NotaCreditoInternoViewModel model)
        {

            NotaCreditoDTO notaCreditoDTO;
            try
            {

                notaCreditoDTO = model.NotaCreditoDTO;

                if (model.EditAction == ViewModelEditAction.New)
                {
                    PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

                    notaCreditoDTO = model.NotaCreditoDTO;

                    notaCreditoDTO.PeriodoEmpresaSedeDTO = periodoEmpresaSedeDTO;
                    notaCreditoDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.PeriodoEmpresaSedeId;

                    EstadoComprobanteDTO estadoComprobanteDTO = estadoComprobanteProxy.ObtenerPorCodigo(new EstadoComprobanteDTO()
                    {
                        Codigo = EstadoComprobanteConstante.PENDIENTE
                    });
                    notaCreditoDTO.EstadoComprobanteDTO = estadoComprobanteDTO;
                    notaCreditoDTO.EstadoComprobanteId = estadoComprobanteDTO.Id;
                }
                else
                {
                    notaCreditoDTO = notaCreditoProxy.GetById(new NotaCreditoDTO() { NotaCreditoId = model.NotaCreditoDTO.Id });

                    model.NotaCreditoDTO.PeriodoEmpresaSedeDTO = notaCreditoDTO.PeriodoEmpresaSedeDTO;
                    model.NotaCreditoDTO.PeriodoEmpresaSedeId = notaCreditoDTO.PeriodoEmpresaSedeId;
                    model.NotaCreditoDTO.MontoBruto = notaCreditoDTO.MontoBruto;
                    model.NotaCreditoDTO.MontoNeto = notaCreditoDTO.MontoNeto;
                    model.NotaCreditoDTO.MontoIgv = notaCreditoDTO.MontoIgv;
                    model.NotaCreditoDTO.MontoIsc = notaCreditoDTO.MontoIsc;
                    model.NotaCreditoDTO.MontoTotal = notaCreditoDTO.MontoTotal;
                    model.NotaCreditoDTO.EsContingencia = notaCreditoDTO.EsContingencia;
                    model.NotaCreditoDTO.EstadoComprobanteDTO = notaCreditoDTO.EstadoComprobanteDTO;
                    model.NotaCreditoDTO.Emitido = notaCreditoDTO.Emitido;

                    notaCreditoDTO = model.NotaCreditoDTO;
                }

                if (string.IsNullOrEmpty(notaCreditoDTO.Emitido))
                    notaCreditoDTO.Emitido = SiNoConstante.N;

                if (string.IsNullOrEmpty(notaCreditoDTO.CodigoHash))
                    notaCreditoDTO.CodigoHash = string.Empty;

                notaCreditoDTO.Estado = EstadoConstante.ACTIVO;
                AuditDTO(notaCreditoDTO);

            }
            catch (Exception)
            {
                throw;
            }

            return notaCreditoDTO;
        }
        [HttpPost("Eliminar")]
        public ActionResult Eliminar([FromBody] NotaCreditoInternoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.NotaCreditoDTO.DTOState = DTOState.Modified;

                AuditDTO(model.NotaCreditoDTO);
                notaCreditoProxy.Delete(model.NotaCreditoDTO);

                NotaCreditoDTO notaCreditoDTO = notaCreditoProxy.GetById(model.NotaCreditoDTO);

                resultResponse.Message = SR.GetString("VT_nota_credito_anulada", notaCreditoDTO.SerieNumero);
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
        public FileContentResult ReportTo(string format, string cid, string notaCreditoId)
        {
            Dictionary<string, string> parameters = new()
         {
            { "notaCreditoId", notaCreditoId }
         };

            return appHelper.ReportToPdf("VT_NotaCreditoInt", parameters, "VT_NotaCreditoInt");
        }
        [HttpPost("Cerrar")]
        public ActionResult Cerrar([FromBody] NotaCreditoInternoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                NotaCreditoDTO notaCreditoDTO = cargarNotaCreditoDTO(model);

                notaCreditoProxy.Cerrar(notaCreditoDTO);

                notaCreditoDTO = notaCreditoProxy.GetById(new NotaCreditoDTO() { NotaCreditoId = model.NotaCreditoDTO.Id });

                resultResponse.Message = SR.GetString("VT_nota_credito_cerrada");
                resultResponse.Data = new
                {
                    notaCreditoDTO.Numero,
                    EstadoComprobante = notaCreditoDTO.EstadoComprobanteDTO.Codigo,
                    EstadoComprobanteNombre = notaCreditoDTO.EstadoComprobanteDTO.Nombre
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
        [HttpPost("ListarNotaCreditoProducto")]
        public ActionResult ListarNotaCreditoProducto(string jsonModel)
        {

            Logger.InfoStart();

            NotaCreditoInternoViewModel model = JsonSerializer.Deserialize<NotaCreditoInternoViewModel>(jsonModel);

            model.NotaCreditoProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
            model.NotaCreditoProductoDTO.OrderBy = "tp.tipo_producto,p.producto";

            try
            {
                List<NotaCreditoProductoDTO> notaCreditoProductoDTOList = notaCreditoProductoProxy.Search(model.NotaCreditoProductoDTO);

                int index = 1;
                var jsonData = new
                {
                    total = 1,
                    page = 1,
                    records = notaCreditoProductoDTOList.Count,
                    rows = from f in notaCreditoProductoDTOList.AsEnumerable()
                           select new
                           {
                               id = index++,
                               cell = new
                               {
                                   f.NotaCreditoProductoId,
                                   f.NotaCreditoId,
                                   f.ComprobanteProductoId,
                                   f.ProductoId,
                                   f.ComprobanteProductoDTO.ProductoDTO.Codigo,
                                   ProductoNombre = f.ComprobanteProductoDTO.ProductoDTO.Descripcion,
                                   TipoProducto = f.ComprobanteProductoDTO.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.TipoProductoDTO.Codigo,
                                   UnidadMedidaNombre = f.ComprobanteProductoDTO.UnidadMedidaDTO == null ? f.ComprobanteProductoDTO.ProductoDTO.UnidadMedidaDTO.Abreviatura :
                                   f.ComprobanteProductoDTO.UnidadMedidaDTO.Abreviatura,
                                   TipoIgv = f.ComprobanteProductoDTO.ProductoDTO.TipoIgvDTO.Codigo,
                                   f.ComprobanteProductoDTO.UnidadMedidaId,
                                   f.Descripcion,
                                   CantidadDocumento = f.ComprobanteProductoDTO.CantidadPresentacion == decimal.Zero ? f.ComprobanteProductoDTO.Cantidad : f.ComprobanteProductoDTO.CantidadPresentacion,
                                   f.Cantidad,
                                   f.ComprobanteProductoDTO.CantidadPresentacion,
                                   f.ComprobanteProductoDTO.CantidadEquivalente,
                                   CantidadVenta = f.ComprobanteProductoDTO.CantidadPresentacion == decimal.Zero ? f.Cantidad : f.ComprobanteProductoDTO.CantidadPresentacion,
                                   ValorUnitarioComprobante = f.ComprobanteProductoDTO.ValorUnitario,
                                   PrecioUnitarioComprobante = f.ComprobanteProductoDTO.PrecioUnitario,
                                   f.PrecioUnitario,
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
        [HttpGet("ObtenerTotales")]
        public JsonResult ObtenerTotales(int id)
        {

            Logger.InfoStart();
            ResultResponse resultResponse = new();

            try
            {
                NotaCreditoDTO notaCreditoDTO = notaCreditoProxy.GetById(new NotaCreditoDTO() { NotaCreditoId = id });
                if (notaCreditoDTO == null)
                    notaCreditoDTO = new NotaCreditoDTO();

                resultResponse.Data = new
                {
                    notaCreditoDTO.MontoBruto,
                    notaCreditoDTO.MontoNeto,
                    notaCreditoDTO.MontoIgv,
                    notaCreditoDTO.MontoTotal
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
        [HttpPost("VerificarSaldoCalcularMontos")]
        public ActionResult VerificarSaldoCalcularMontos([FromBody] NotaCreditoInternoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                decimal saldoNota = decimal.Zero;
                decimal cantidadAnterior = decimal.Zero;
                decimal cantidadDisponible = decimal.Zero;

                if (model.NotaCreditoProductoDTO.ComprobanteProductoId.HasValue)
                {
                    model.NotaCreditoProductoDTO.ComprobanteProductoDTO = comprobanteProductoProxy.GetById(new ComprobanteProductoDTO()
                    {
                        ComprobanteProductoId = model.NotaCreditoProductoDTO.ComprobanteProductoId.Value,
                    });
                    saldoNota = model.NotaCreditoProductoDTO.ComprobanteProductoDTO.Cantidad - model.NotaCreditoProductoDTO.ComprobanteProductoDTO.CantidadNotaCredito;
                }

                if (model.NotaCreditoProductoDTO.NotaCreditoProductoId > 0)
                {
                    NotaCreditoProductoDTO notaCreditoProductoDTO = notaCreditoProductoProxy.GetById(new NotaCreditoProductoDTO()
                    {
                        NotaCreditoProductoId = model.NotaCreditoProductoDTO.NotaCreditoProductoId
                    });

                    cantidadAnterior = notaCreditoProductoDTO.Cantidad;
                }

                cantidadDisponible = saldoNota + cantidadAnterior;

                if (cantidadDisponible < model.NotaCreditoProductoDTO.Cantidad)
                    throw new Exception(SR.GetString("VT_comprobante_nota_credito_cantidad_mayor_al_saldo", cantidadDisponible));

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
        [HttpPost("GrabarNotaCreditoProducto")]
        public ActionResult GrabarNotaCreditoProducto([FromBody] NotaCreditoInternoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                NotaCreditoProductoDTO notaCreditoProductoDTO = cargarNotaCreditoProductoDTO(model);

                int newId = model.NotaCreditoProductoDTO.Id;
                string messageResourceKey = string.Empty;
                switch (notaCreditoProductoDTO.DTOState)
                {
                    case DTOState.Added:
                        newId = notaCreditoProductoProxy.Insert(notaCreditoProductoDTO);
                        messageResourceKey = "VT_producto_agregado";
                        break;

                    case DTOState.Modified:
                        notaCreditoProductoProxy.Update(notaCreditoProductoDTO);
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
        private NotaCreditoProductoDTO cargarNotaCreditoProductoDTO(NotaCreditoInternoViewModel model)
        {

            NotaCreditoProductoDTO notaCreditoProductoDTO;
            try
            {

                if (model.NotaCreditoProductoDTO.DTOState == DTOState.Added)
                {
                    notaCreditoProductoDTO = model.NotaCreditoProductoDTO;
                }
                else
                {
                    notaCreditoProductoDTO = notaCreditoProductoProxy.GetById(new NotaCreditoProductoDTO() { NotaCreditoProductoId = model.NotaCreditoProductoDTO.Id });

                    notaCreditoProductoDTO = model.NotaCreditoProductoDTO;
                }

                AuditDTO(notaCreditoProductoDTO);
            }
            catch (Exception)
            {
                throw;
            }

            return notaCreditoProductoDTO;
        }
        [HttpPost("EliminarNotaCreditoProducto")]
        public ActionResult EliminarNotaCreditoProducto([FromBody] NotaCreditoInternoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                model.NotaCreditoProductoDTO.DTOState = DTOState.Modified;
                AuditDTO(model.NotaCreditoProductoDTO);

                NotaCreditoProductoDTO notaCreditoProductoDTO = notaCreditoProductoProxy.GetById(model.NotaCreditoProductoDTO);

                notaCreditoProductoProxy.Delete(notaCreditoProductoDTO);

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
        [HttpPost("AgregarComprobanteProducto")]
        public ActionResult AgregarComprobanteProducto([FromBody] NotaCreditoInternoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                AuditDTO(model.NotaCreditoProductoDTO);

                notaCreditoProductoProxy.AgregarComprobanteProducto(model.NotaCreditoProductoDTO);

                resultResponse.Message = SR.GetString("VT_comprobante_producto_multiple_seleccion_agregado");
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
