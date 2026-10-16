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

namespace ESD.ERP.Web.Areas.MG.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class AnticipoClienteController : AuthorizedController
   {
      private readonly IProveedorProxy proveedorProxy;
      private readonly IAnticipoClienteProxy anticipoClienteProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy;
      private readonly ITipoCargoAbonoProxy tipoCargoAbonoProxy;
      private readonly IMedioPagoProxy medioPagoProxy;
      private readonly IBancoProxy bancoProxy;
      private readonly ITipoOperacionAnticipoProxy tipoOperacionAnticipoProxy;
      private readonly IEstadoComprobanteProxy estadoComprobanteProxy;
      private readonly ITipoDocumentoComercialProxy tipoDocumentoComercialProxy;
      private readonly IVendedorSerieProxy vendedorSerieProxy;
      private readonly IVendedorProxy vendedorProxy;
      public AnticipoClienteController(
         IProveedorProxy proveedorProxy,
         IAnticipoClienteProxy anticipoClienteProxy,
         IMonedaProxy monedaProxy,
         ITipoDocumentoIdentidadProxy tipoDocumentoIdentidadProxy,
         ITipoCargoAbonoProxy tipoCargoAbonoProxy,
         IMedioPagoProxy medioPagoProxy,
         IBancoProxy bancoProxy,
         ITipoOperacionAnticipoProxy tipoOperacionAnticipoProxy,
         IEstadoComprobanteProxy estadoComprobanteProxy,
         ITipoDocumentoComercialProxy tipoDocumentoComercialProxy,
         IVendedorSerieProxy vendedorSerieProxy,
         IVendedorProxy vendedorProxy
         )
      {
         this.proveedorProxy = proveedorProxy;
         this.anticipoClienteProxy = anticipoClienteProxy;
         this.monedaProxy = monedaProxy;
         this.tipoDocumentoIdentidadProxy = tipoDocumentoIdentidadProxy;
         this.tipoCargoAbonoProxy = tipoCargoAbonoProxy;
         this.medioPagoProxy = medioPagoProxy;
         this.bancoProxy = bancoProxy;
         this.estadoComprobanteProxy = estadoComprobanteProxy;
         this.tipoOperacionAnticipoProxy = tipoOperacionAnticipoProxy;
         this.tipoDocumentoComercialProxy = tipoDocumentoComercialProxy;
         this.vendedorSerieProxy = vendedorSerieProxy;
         this.vendedorProxy = vendedorProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         AnticipoClienteViewModel anticipoClienteViewModel = new();

         cargarListas(anticipoClienteViewModel);

         anticipoClienteViewModel.MenuItemList = new();
         anticipoClienteViewModel.MenuItemList.Add(new MenuItem() { MenuItemType = MenuItemType.Edit });

         return PartialView(anticipoClienteViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         AnticipoClienteViewModel model = JsonSerializer.Deserialize<AnticipoClienteViewModel>(jsonModel);

         model.ProveedorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ProveedorDTO.OrderBy = "p.razon_social";

         model.ProveedorDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

         try
         {
            List<ProveedorDTO> proveedorDTOList = proveedorProxy.Search(model.ProveedorDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = proveedorDTOList.Count,
               rows = from f in proveedorDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ProveedorId,
                            f.Codigo,
                            TipoNaturalezaJuridica = f.TipoNaturalezaJuridicaDTO.Codigo,
                            TipoDocumentoIdentidadNombre = f.TipoDocumentoIdentidadDTO.Abreviatura,
                            f.NumeroDocumentoIdentidad,
                            f.NumeroRuc,
                            f.RazonSocial,
                            f.Direccion,
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
      public ActionResult Editar([FromBody] AnticipoClienteViewModel model)
      {

         Logger.InfoStart();

         AnticipoClienteViewModel anticipoClienteViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(anticipoClienteViewModel, model.AnticipoClienteDTO.ProveedorId);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, anticipoClienteViewModel);
      }
      private void cargarDataParaEditar(AnticipoClienteViewModel model, int id)
      {

         ProveedorDTO proveedorDTO = proveedorProxy.GetById(new ProveedorDTO() { ProveedorId = id });
         model.ProveedorDTO = proveedorDTO;

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
            model.VendedorId = vendedorDTO.Id;
         }

         configurarServicioImpresion(model);

         MenuBarTagHelper menuBarTagHelper = new();
         model.MenuItemList = menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.NewEdit);
         model.MenuItemList.Add(new MenuItem() { MenuItemType = MenuItemType.Preview });
         model.MenuItemList.Add(new MenuItem() { Key = "cerrar", Content = "Cerrar Anticipo" });
         model.MenuItemList.Add(new MenuItem() { Key = "dividir", Content = "Distribuir Anticipo" });
         model.MenuItemList.Add(new MenuItem() { Key = "extornar", Content = "Extornar" });

      }
      private void cargarListas(AnticipoClienteViewModel model)
      {

         TipoDocumentoIdentidadDTO tipoDocumentoIdentidadDTO = new() { Estado = EstadoConstante.ACTIVO };

         if(model.ProveedorDTO != null)
            if (model.ProveedorDTO.TipoNaturalezaJuridicaDTO.Codigo == TipoNaturalezaJuridicaConstante.JURIDICO)
               tipoDocumentoIdentidadDTO.Codigo = TipoDocumentoIdentidadConstante.RUC;
            else
               tipoDocumentoIdentidadDTO.NotTipoDocumentoIdentidadList = new List<string>() { TipoDocumentoIdentidadConstante.RUC };

         List<TipoDocumentoIdentidadDTO> tipoDocumentoIdentidadDTOList = tipoDocumentoIdentidadProxy.Search(tipoDocumentoIdentidadDTO);
         model.TipoDocumentoIdentidadList = WebHelper.ToSelectListItem(tipoDocumentoIdentidadDTOList, x => x.Id.ToString(), x => x.Abreviatura,
            SelectListFirstOptionType.Select);
      }
      [HttpGet("ObtenerMedioPago")]
      public JsonResult ObtenerMedioPago(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            MedioPagoDTO medioPagoDTO = medioPagoProxy.GetById(new MedioPagoDTO() { MedioPagoId = id });

            resultResponse.Data = new
            {
               medioPagoDTO.Codigo
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

         AnticipoClienteViewModel model = JsonSerializer.Deserialize<AnticipoClienteViewModel>(jsonModel);

         model.AnticipoClienteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.AnticipoClienteDTO.OrderBy = "ac.numero_opera desc,tca.tipo_car_abo,ac.fecha";
         model.AnticipoClienteDTO.ProveedorId = NumberUtil.WhenValueIsZero(model.AnticipoClienteDTO.ProveedorId);
         model.AnticipoClienteDTO.NullFechaBaja = SiNoConstante.S;

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
                            TipoCargoAbono = f.TipoCargoAbonoDTO.Codigo,
                            f.NumeroOperacion,
                            Fecha = string.Format(StringFormatConstant.DATE, f.Fecha),
                            MedioPago = f.MedioPagoDTO.Abreviatura,
                            Banco = f?.BancoDTO?.Nombre,
                            FechaReferencia = f.FechaReferencia == null ? string.Empty : string.Format(StringFormatConstant.DATE, f.FechaReferencia.Value),
                            f.NumeroReferencia,
                            f.MonedaId,
                            Moneda = f.MonedaDTO.Simbolo,
                            Monto = f.TipoCargoAbonoDTO.Codigo == TipoCargoAbonoConstante.ABONO ? f.Monto : decimal.Zero,
                            MontoAplicado = f.TipoOperacionAnticipoDTO.Codigo == TipoOperacionAnticipoConstante.ANTICIPO ? f.MontoAplicado : f.Monto,
                            MontoSaldo = f.TipoOperacionAnticipoDTO.Codigo == TipoOperacionAnticipoConstante.ANTICIPO ? f.MontoSaldo : decimal.Zero,
                            f.CotizacionId,
                            Pedido = f?.CotizacionDTO?.Codigo,
                            f.TipoDocumentoComercialId,
                            TipoDocumentoComercial = f?.TipoDocumentoComercialDTO?.Abreviatura,
                            f.Serie,
                            f.Observacion,
                            TipoOperacionAnticipo = f.TipoOperacionAnticipoDTO.Codigo,
                            TipoOperacionAnticipoNombre = f.TipoOperacionAnticipoDTO.Abreviatura,
                            f.Extornado,
                            SerieNumero = obtenerSerieNumero(f),
                            EstadoComprobante = f.EstadoComprobanteDTO.Codigo,
                            EstadoComprobanteNombre = f.EstadoComprobanteDTO.Nombre,
                            f.PermiteDividirMonto,
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
      private string obtenerSerieNumero(AnticipoClienteDTO anticipoClienteDTO)
      {
         if (anticipoClienteDTO.TipoOperacionAnticipoDTO.Codigo == TipoOperacionAnticipoConstante.ANTICIPO)
            return anticipoClienteDTO.ComprobanteDTO == null ? string.Empty : string.Concat(anticipoClienteDTO.ComprobanteDTO.Serie,
                            SeparadorConstante.GUION, anticipoClienteDTO.ComprobanteDTO.Numero);
         else
            return anticipoClienteDTO == null ? string.Empty :
               (anticipoClienteDTO.ComprobanteDTO == null ? string.Empty : string.Concat(anticipoClienteDTO.ComprobanteDTO.Serie,
               SeparadorConstante.GUION, anticipoClienteDTO.ComprobanteDTO.Numero));
      }
      [HttpPost("GrabarAnticipoCliente")]
      public ActionResult GrabarAnticipoCliente([FromBody] AnticipoClienteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AnticipoClienteDTO anticipoClienteDTO = cargarAnticipoClienteDTO(model);

            int newId = model.AnticipoClienteDTO.Id;
            string messageResourceKey = string.Empty;
            switch (anticipoClienteDTO.DTOState)
            {
               case DTOState.Added:
                  newId = anticipoClienteProxy.Insert(anticipoClienteDTO);
                  messageResourceKey = "VT_cta_cte_agregada";
                  break;

               case DTOState.Modified:
                  anticipoClienteProxy.Update(anticipoClienteDTO);
                  messageResourceKey = "VT_cta_cte_actualizada";
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
      private AnticipoClienteDTO cargarAnticipoClienteDTO(AnticipoClienteViewModel model)
      {

         AnticipoClienteDTO anticipoClienteDTO;
         try
         {

            if (model.AnticipoClienteDTO.DTOState == DTOState.Added)
            {
               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

               anticipoClienteDTO = model.AnticipoClienteDTO;
               anticipoClienteDTO.EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO;
               anticipoClienteDTO.EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId;

               TipoCargoAbonoDTO tipoCargoAbonoDTO = null;
               if (model.AnticipoClienteDTO.TipoCargoAbonoId == 0)
               {
                  tipoCargoAbonoDTO = tipoCargoAbonoProxy.ObtenerPorCodigo(new TipoCargoAbonoDTO()
                  {
                     Codigo = TipoCargoAbonoConstante.ABONO,
                     Estado = EstadoConstante.ACTIVO
                  });
                  model.AnticipoClienteDTO.TipoCargoAbonoId = tipoCargoAbonoDTO.Id;
               }
               else
                  tipoCargoAbonoDTO = tipoCargoAbonoProxy.GetById(new TipoCargoAbonoDTO() { 
                     TipoCargoAbonoId = model.AnticipoClienteDTO.TipoCargoAbonoId 
                  });
               anticipoClienteDTO.TipoCargoAbonoDTO = tipoCargoAbonoDTO;

               if (model.AnticipoClienteDTO.TipoOperacionAnticipoId == 0)
               {
                  TipoOperacionAnticipoDTO tipoOperacionAnticipoDTO = tipoOperacionAnticipoProxy.ObtenerPorCodigo(new TipoOperacionAnticipoDTO()
                  {
                     Codigo = TipoOperacionAnticipoConstante.ANTICIPO,
                     Estado = EstadoConstante.ACTIVO
                  });
                  model.AnticipoClienteDTO.TipoOperacionAnticipoId = tipoOperacionAnticipoDTO.Id;
               }

               EstadoComprobanteDTO estadoComprobanteDTO = estadoComprobanteProxy.ObtenerPorCodigo(new EstadoComprobanteDTO()
               {
                  Codigo = EstadoComprobanteConstante.PENDIENTE,
               });
               anticipoClienteDTO.EstadoComprobanteId = estadoComprobanteDTO.Id;
            }
            else
            {
               anticipoClienteDTO = anticipoClienteProxy.GetById(new AnticipoClienteDTO() { AnticipoClienteId = model.AnticipoClienteDTO.Id });

               model.AnticipoClienteDTO.TipoCargoAbonoId = anticipoClienteDTO.TipoCargoAbonoId;
               if(model.AnticipoClienteDTO.VendedorId == 0)
                  model.AnticipoClienteDTO.VendedorId = anticipoClienteDTO.VendedorId;

               model.AnticipoClienteDTO.TipoOperacionAnticipoId = anticipoClienteDTO.TipoOperacionAnticipoId;
               model.AnticipoClienteDTO.ComprobanteId = anticipoClienteDTO.ComprobanteId;
               model.AnticipoClienteDTO.AnticipoClientePadreId = anticipoClienteDTO.AnticipoClientePadreId;

               anticipoClienteDTO = model.AnticipoClienteDTO;
            }

            if (string.IsNullOrEmpty(anticipoClienteDTO.NumeroOperacion))
               anticipoClienteDTO.NumeroOperacion = string.Empty;
            if (string.IsNullOrEmpty(anticipoClienteDTO.NumeroReferencia))
               anticipoClienteDTO.NumeroReferencia = string.Empty;
            if (string.IsNullOrEmpty(anticipoClienteDTO.Extornado))
               anticipoClienteDTO.Extornado = SiNoConstante.N;
            if (string.IsNullOrEmpty(anticipoClienteDTO.Observacion))
               anticipoClienteDTO.Observacion = string.Empty;

            anticipoClienteDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(anticipoClienteDTO);
         }
         catch (Exception)
         {
            throw;
         }

         return anticipoClienteDTO;
      }
      [HttpPost("ExtornarAnticipoCliente")]
      public ActionResult ExtornarAnticipoCliente([FromBody] AnticipoClienteViewModel model)
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
      public ActionResult CerrarAnticipoCliente([FromBody] AnticipoClienteViewModel model)
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
      [HttpPost("DividirEditPopup")]
      public ActionResult DividirEditPopup([FromBody] AnticipoClienteViewModel model)
      {

         try
         {
            AnticipoClienteDTO anticipoClienteDTO = anticipoClienteProxy.GetById(new 
               AnticipoClienteDTO() { AnticipoClienteId = model.AnticipoClienteDTO.AnticipoClienteId });

            model.AnticipoClienteDTO = anticipoClienteDTO;
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(model);
      }
      [HttpPost("DividirAnticipoCliente")]
      public ActionResult DividirAnticipoCliente([FromBody] AnticipoClienteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.AnticipoClienteDTO);
            anticipoClienteProxy.DividirMonto(model.AnticipoClienteDTO);
            resultResponse.Message = SR.GetString("VT_cta_cte_actualizada");
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
      [HttpPost("CreateEditPopup")]
      public ActionResult CreateEditPopup([FromBody] AnticipoClienteViewModel model)
      {

         try
         {
            if (model.AnticipoClienteDTO.DTOState == DTOState.Added)
            {
               model.AnticipoClienteDTO.Fecha = DateTime.Today;
               model.AnticipoClienteDTO.MedioPagoDTO = new();
            }
            else
            {
               model.AnticipoClienteDTO = anticipoClienteProxy.GetById(new AnticipoClienteDTO() { AnticipoClienteId = model.AnticipoClienteDTO.Id });
            }

            cargarListasPopup(model);

            WebHelper.SelectedListItemSetValue(model.MonedaList, model.AnticipoClienteDTO.MonedaId);
            WebHelper.SelectedListItemSetValue(model.MedioPagoList, model.AnticipoClienteDTO.MedioPagoId);
            WebHelper.SelectedListItemSetValue(model.BancoList, model.AnticipoClienteDTO.BancoId);
            WebHelper.SelectedListItemSetValue(model.TipoDocumentoComercialList, model.AnticipoClienteDTO.TipoDocumentoComercialId);
            if(model.SerieList != null)
               WebHelper.SelectedListItemSetValue(model.SerieList, model.AnticipoClienteDTO.Serie);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(model);
      }
      private void cargarListasPopup(AnticipoClienteViewModel model)
      {

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { Estado = EstadoConstante.ACTIVO });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MedioPagoDTO> medioPagoDTOList = medioPagoProxy.Search(new MedioPagoDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO
         });
         model.MedioPagoList = WebHelper.ToSelectListItem(medioPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<BancoDTO> bancoDTOList = bancoProxy.Search(new BancoDTO() { Estado = EstadoConstante.ACTIVO });
         model.BancoList = WebHelper.ToSelectListItem(bancoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = tipoDocumentoComercialProxy.Search(new TipoDocumentoComercialDTO()
         {
            TipoDocumentoComercialList = new List<string>() { TipoDocumentoComercialConstante.FACTURA, TipoDocumentoComercialConstante.BOLETA_VENTA },
            Estado = EstadoConstante.ACTIVO
         });
         model.TipoDocumentoComercialList = WebHelper.ToSelectListItem(tipoDocumentoComercialDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         if (model.AnticipoClienteDTO.TipoDocumentoComercialId.HasValue)
         {
            List<VendedorSerieDTO> vendedorSerieDTOList = vendedorSerieProxy.Search(new VendedorSerieDTO()
            {
               VendedorId = model.AnticipoClienteDTO.VendedorId,
               DocumentoComercialSerieDTO = new DocumentoComercialSerieDTO()
               {
                  EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                  TipoDocumentoComercialId = model.AnticipoClienteDTO.TipoDocumentoComercialId.Value
               },
               Estado = EstadoConstante.ACTIVO
            });
            model.SerieList = WebHelper.ToSelectListItem(vendedorSerieDTOList, x => x.DocumentoComercialSerieDTO.Id.ToString(), x => x.DocumentoComercialSerieDTO.Serie,
            SelectListFirstOptionType.Empty);
         }
      }
      [HttpPost("ListPopup")]
      public ActionResult ListPopup([FromBody] AnticipoClienteViewModel model)
      {
         if (model == null)
            model = new AnticipoClienteViewModel();

         model.ModalEditId = GetModalEditPanelId();

         return PartialView(model);
      }
      private void configurarServicioImpresion(AnticipoClienteViewModel anticipoClienteViewModel)
      {
         int empresaId = 0;
         if (!string.IsNullOrEmpty(anticipoClienteViewModel.ViewModelState.CID))
            empresaId = ObtenerEmpresaId(anticipoClienteViewModel.ViewModelState.CID);
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
            anticipoClienteViewModel.ServidorImpresion = vendedorDTO.ServidorImpresion;
         }

         string url = ObtenerDireccionServicioGeneracionFormatoImpresion();

         byte[] plainTextBytes = Encoding.UTF8.GetBytes(url);
         anticipoClienteViewModel.DataUrl = Convert.ToBase64String(plainTextBytes);
      }
   }
}