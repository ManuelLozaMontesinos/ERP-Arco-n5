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
   public class CanjeSelectivoController : AuthorizedController
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
      private readonly IProveedorProxy proveedorProxy;
      private readonly IListaPrecioProveedorProxy listaPrecioProveedorProxy;
      private readonly IFacturacionService facturacionService;
      private readonly ITipoDescuentoProxy tipoDescuentoProxy;
      private readonly IEstadoDespachoProxy estadoDespachoProxy;
      private readonly AppHelper appHelper;

      public CanjeSelectivoController(
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
         IProveedorProxy proveedorProxy,
         IListaPrecioProveedorProxy listaPrecioProveedorProxy,
         IFacturacionService facturacionService,
         ITipoDescuentoProxy tipoDescuentoProxy,
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
         this.proveedorProxy = proveedorProxy;
         this.listaPrecioProveedorProxy = listaPrecioProveedorProxy;
         this.facturacionService = facturacionService;
         this.tipoDescuentoProxy = tipoDescuentoProxy;
         this.estadoDespachoProxy = estadoDespachoProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         CanjeSelectivoViewModel canjeSelectivoViewModel = new();

         cargarListaEstadoComprobante(canjeSelectivoViewModel);

         return PartialView(canjeSelectivoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         CanjeSelectivoViewModel model = JsonSerializer.Deserialize<CanjeSelectivoViewModel>(jsonModel);

         model.ComprobanteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteDTO.OrderBy = "c.fecha desc,c.numero_opera desc";

         model.ComprobanteDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.ComprobanteDTO.TipoComprobanteDTO = new TipoComprobanteDTO() { Codigo = TipoComprobanteConstante.CANJE_SELECTIVO }; 

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
      public ActionResult Crear([FromBody] CanjeSelectivoViewModel model)
      {
         Logger.InfoStart();

         CanjeSelectivoViewModel canjeSelectivoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(canjeSelectivoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, canjeSelectivoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] CanjeSelectivoViewModel model)
      {

         Logger.InfoStart();

         CanjeSelectivoViewModel canjeSelectivoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(canjeSelectivoViewModel, model.ComprobanteDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, canjeSelectivoViewModel);
      }
      private void cargarDataParaEditar(CanjeSelectivoViewModel model, int id)
      {

         ComprobanteDTO comprobanteDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            comprobanteDTO = new ComprobanteDTO() {
               Fecha = DateTime.Today,
               TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO(),
               VendedorDTO = new VendedorDTO() { ProveedorDTO = new ProveedorDTO() { } }
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
         }
         else
         {
            comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = id });

            model.ListaPrecioProveedorId = internalObtenerListaPrecioProveedorId(comprobanteDTO.ListaPrecioId,
               comprobanteDTO.ProveedorId);
         }

         model.ComprobanteDTO = comprobanteDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.TipoDocumentoComercialList, comprobanteDTO.TipoDocumentoComercialId);
         WebHelper.SelectedListItemSetValue(model.MonedaList, comprobanteDTO.MonedaId);
         WebHelper.SelectedListItemSetValue(model.ListaPrecioList, comprobanteDTO.ListaPrecioId);
         WebHelper.SelectedListItemSetValue(model.TipoCondicionPagoList, comprobanteDTO.TipoCondicionPagoId);
         WebHelper.SelectedListItemSetValue(model.MedioPagoList, comprobanteDTO.MedioPagoId);

         model.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Save },
            new MenuItem() { MenuItemType = MenuItemType.Exit },
            new MenuItem() { Key="emitir", Content="Emitir Comprobante" }
         };
      }
      private void cargarListaEstadoComprobante(CanjeSelectivoViewModel model)
      {
         List<EstadoComprobanteDTO> estadoComprobanteDTOList = estadoComprobanteProxy.Search(new EstadoComprobanteDTO() { Estado = EstadoConstante.ACTIVO });
         model.EstadoComprobanteList = WebHelper.ToSelectListItem(estadoComprobanteDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      private void cargarListas(CanjeSelectivoViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = vendedorSerieProxy.ListarTipoDocumentoComercialPorVendedor
            (new VendedorSerieDTO() { 
               VendedorId = model.ComprobanteDTO.VendedorId,
               DocumentoComercialSerieDTO = new DocumentoComercialSerieDTO() { 
                  EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                  TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO() { 
                     EsComprobantePago = SiNoConstante.S, EsInterno = SiNoConstante.N}
               },
               Estado = EstadoConstante.ACTIVO 
            });
         model.TipoDocumentoComercialList = WebHelper.ToSelectListItem(tipoDocumentoComercialDTOList, x => x.Id.ToString(), x => x.Abreviatura,
            SelectListFirstOptionType.Select);

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

            if (model.ComprobanteDTO.ListaPrecioId == 0)
               if (listaPrecioDTOList.Count > 0)
               {
                  model.ComprobanteDTO.ListaPrecioId = listaPrecioDTOList[0].Id;
                  model.ComprobanteDTO.ImpuestoTasaDTO = listaPrecioDTOList[0].ImpuestoTasaDTO;
               }
         }

         List<TipoCondicionPagoDTO> tipoCondicionPagoDTOList = tipoCondicionPagoProxy.Search(new TipoCondicionPagoDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoCondicionPagoList = WebHelper.ToSelectListItem(tipoCondicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MedioPagoDTO> medioPagoDTOList = medioPagoProxy.Search(new MedioPagoDTO() { 
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO 
         });
         model.MedioPagoList = WebHelper.ToSelectListItem(medioPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("ListarSerie")]
      public JsonResult ListarSerie([FromBody] CanjeSelectivoViewModel model)
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
      public JsonResult ListarListaPrecio([FromBody] CanjeSelectivoViewModel model)
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
                  model.ListaPrecioProveedorDTO.ProveedorId)
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] CanjeSelectivoViewModel model)
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
      private ComprobanteDTO cargarComprobanteDTO(CanjeSelectivoViewModel model)
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
                  Codigo = TipoComprobanteConstante.CANJE_SELECTIVO
               });
               comprobanteDTO.TipoComprobanteId = tipoComprobanteDTO.Id;

               TipoDetraccionDTO tipoDetraccionDTO = tipoDetraccionProxy.ObtenerPorCodigo(new TipoDetraccionDTO()
               {
                  Codigo = TipoDetraccionConstante.SIN_DETRACCION
               });
               comprobanteDTO.TipoDetraccionId = tipoDetraccionDTO.Id;

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

         if (comprobanteDTO.EstadoComprobanteDTO.Codigo == EstadoComprobanteConstante.EMITIDO)
            throw new Exception(SR.GetString("VT_comprobante_ya_esta_emitido"));

         if (comprobanteDTO.EstadoComprobanteDTO.Codigo == EstadoComprobanteConstante.ANULADO)
            throw new Exception(SR.GetString("VT_comprobante_ya_esta_anulado"));
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] CanjeSelectivoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ComprobanteDTO.DTOState = DTOState.Modified;

            AuditDTO(model.ComprobanteDTO);
            comprobanteProxy.EliminarCanje(model.ComprobanteDTO);

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

         ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = Convert.ToInt32(comprobanteId) });

         string formatoReporte = string.Empty;
         if (comprobanteDTO.TipoDocumentoComercialDTO.EsInterno == SiNoConstante.N)
            formatoReporte = comprobanteDTO.MontoDetraccion == decimal.Zero ? "VT_ComprobanteE" : "VT_InvoiceWithDetraction";
         else
            formatoReporte = "VT_InternoE";

         byte[] reportData = appHelper.ExecuteReport(formatoReporte, parameters, ReportFormatConstant.PDF);

         return reportData;
      }
      [HttpPost("Emitir")]
      public ActionResult Emitir([FromBody] CanjeSelectivoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ComprobanteDTO comprobanteDTO = cargarComprobanteDTO(model);

            AuditDTO(model.ComprobanteDTO);

            comprobanteProxy.EmitirCanje(model.ComprobanteDTO);

            comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = model.ComprobanteDTO.Id });

            generarDocumentoElectronico(comprobanteDTO);

            resultResponse.Message = SR.GetString("VT_comprobante_emitido", comprobanteDTO.SerieNumero);
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

         CanjeSelectivoViewModel model = JsonSerializer.Deserialize<CanjeSelectivoViewModel>(jsonModel);

         model.ComprobanteProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteProductoDTO.OrderBy = "tp.tipo_producto,p.producto";

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
                            f.SolicitaAutorizacionDescuento,
                            f.MontoBruto,
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

            resultResponse.Data = new
            {
               comprobanteDTO.MontoBruto,
               comprobanteDTO.MontoNeto,
               comprobanteDTO.MontoIgv,
               comprobanteDTO.MontoTotal
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
      public ActionResult CalcularMontos([FromBody] CanjeSelectivoViewModel model)
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
      [HttpPost("AplicarTasaPorMedioPago")]
      public ActionResult AplicarTasaPorMedioPago([FromBody] CanjeSelectivoViewModel model)
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
      public ActionResult GrabarComprobanteProducto([FromBody] CanjeSelectivoViewModel model)
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

               case DTOState.Modified:
                  comprobanteProductoProxy.ActualizarCanje(comprobanteProductoDTO);
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
      private ComprobanteProductoDTO cargarComprobanteProductoDTO(CanjeSelectivoViewModel model)
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

            comprobanteProductoDTO.PermiteDescuento = SiNoConstante.N;
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
      public ActionResult EliminarComprobanteProducto([FromBody] CanjeSelectivoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ComprobanteProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ComprobanteProductoDTO);

            ComprobanteProductoDTO comprobanteProductoDTO = comprobanteProductoProxy.GetById(model.ComprobanteProductoDTO);

            comprobanteProductoProxy.EliminarCanje(comprobanteProductoDTO);

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
      public ActionResult AgregarComprobanteProducto([FromBody] CanjeSelectivoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {          

            AuditDTO(model.ComprobanteProductoDTO);

            comprobanteProductoProxy.AgregarComprobanteProductoCanjeSelectivo(model.ComprobanteProductoDTO);

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
   }
}
