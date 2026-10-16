using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.FE.Models;
using ESD.ERP.Web.Areas.FE.Service;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.FE.Controllers
{
   [Area("FE")]
   [Route("[controller]")]
   public class DocEmitidoController : AuthorizedController
   {
      private readonly IComprobanteProxy comprobanteProxy;
      private readonly ITipoDocumentoComercialProxy tipoDocumentoComercialProxy;
      private readonly IEstadoSunatProxy estadoSunatProxy;
      private readonly IFacturacionService facturacionService;
      private readonly AppHelper appHelper;

      public DocEmitidoController(
         IComprobanteProxy comprobanteProxy,
         ITipoDocumentoComercialProxy tipoDocumentoComercialProxy,
         IEstadoSunatProxy estadoSunatProxy,
         IFacturacionService facturacionService,
         AppHelper appHelper
         )
      {
         this.comprobanteProxy = comprobanteProxy;
         this.tipoDocumentoComercialProxy = tipoDocumentoComercialProxy;
         this.estadoSunatProxy = estadoSunatProxy;
         this.facturacionService = facturacionService;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         DocEmitidoViewModel docEmitidoViewModel = new()
         {
            Title= "Documentos Emitidos",
            FechaDesde = DateTime.Today,
            FechaHasta = DateTime.Today,

            MenuItemList = new List<MenuItem>
            {
               new MenuItem() { Key="enviar", Content="Enviar" },
               new MenuItem() { Key="genera_xml", Content="Generar XML"},
               new MenuItem() { Key="actualizar_cdr", Content="Actualizar CDR"},
               new MenuItem() { Key="descargar_cdr", Content="Descargar CDR SUNAT"},
               new MenuItem() { Key="descargar_xml", Content="Descargar XML"},
               new MenuItem() { Key="descargar_pdf", Content="Descargar PDF"}
            }
         };

         List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = tipoDocumentoComercialProxy.Search(new TipoDocumentoComercialDTO() 
         { 
            TipoDocumentoComercialList = new List<string>() { TipoDocumentoComercialConstante.FACTURA,
            TipoDocumentoComercialConstante.NOTA_CREDITO},
            Estado = EstadoConstante.ACTIVO 
         });
         docEmitidoViewModel.TipoDocumentoComercialList = WebHelper.ToSelectListItem(tipoDocumentoComercialDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         return PartialView(docEmitidoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         DocEmitidoViewModel model = JsonSerializer.Deserialize<DocEmitidoViewModel>(jsonModel);

         model.ComprobanteDTO.PeriodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = tipoDocumentoComercialProxy.Search(new TipoDocumentoComercialDTO()
         {
            TipoDocumentoComercialList = new List<string>() { TipoDocumentoComercialConstante.FACTURA,
               TipoDocumentoComercialConstante.NOTA_CREDITO},
            Estado = EstadoConstante.ACTIVO
         });

         if (model.ComprobanteDTO.TipoDocumentoComercialId == 0) {
            model.ComprobanteDTO.TipoDocumentoComercialList = tipoDocumentoComercialDTOList.Select(x => x.Codigo).ToList();
         }
         else
         {
            model.ComprobanteDTO.TipoDocumentoComercialList = tipoDocumentoComercialDTOList.Where(x => x.TipoDocumentoComercialId ==
            model.ComprobanteDTO.TipoDocumentoComercialId).Select(x => x.Codigo).ToList();
         }

         if (model.ComprobanteDTO.TipoDocumentoComercialDTO == null)
            model.ComprobanteDTO.TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO() {
               Codigo = TipoDocumentoComercialConstante.FACTURA
            };

         List<EstadoSunatDTO> estadoSunatDTOList = estadoSunatProxy.Search(new EstadoSunatDTO() { Estado = EstadoConstante.ACTIVO });

         if (string.IsNullOrEmpty(model.EstadoDocumento))
            model.ComprobanteDTO.EstadoSunatList = estadoSunatDTOList.Select(x => x.Codigo).ToList();
         if (model.EstadoDocumento == LetraConstante.P)
            model.ComprobanteDTO.EstadoSunatList = estadoSunatDTOList.Where(x => x.Codigo != EstadoSunatConstante.APROBADO).Select(x => x.Codigo).ToList();
         if (model.EstadoDocumento == LetraConstante.A)
            model.ComprobanteDTO.EstadoSunatList = estadoSunatDTOList.Where(x => x.Codigo == EstadoSunatConstante.APROBADO).Select(x => x.Codigo).ToList();

         try
         {
            List<ComprobanteDTO> comprobanteDTOList = comprobanteProxy.ListarDocumentoEmitido(model.ComprobanteDTO);

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
                            f.ComprobanteElectronicoId,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.Serie,
                            f.Numero,
                            f.TipoDocumentoComercialId,
                            TipoDocumentoComercial = f.TipoDocumentoComercialDTO.Codigo,
                            TipoDocumentoComercialNombre = f.TipoDocumentoComercialDTO.Nombre,
                            f.ProveedorDTO.NumeroRuc,
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.MontoTotal,
                            EstadoSunat = f.EstadoSunatDTO.Codigo,
                            EstadoSunatNombre = f.EstadoSunatDTO.Nombre
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
      private ParametroSistemaDTO obtenerParametroSistemaDTO()
      {
         List<ParametroSistemaDTO> parametroSistemaDTOList = appHelper
            .ObtenerGrupoParametroSistema(GrupoParametroSistemaConstante.FACTURACION_ELECTRONICA);

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaDTOList.
            FirstOrDefault(i => i.Codigo == ParametroSistemaConstante.FACTURACION_E_URL && i.Estado == EstadoConstante.ACTIVO);

         return parametroSistemaDTO;
      }
      [HttpPost("Enviar")]
      public ActionResult Enviar([FromBody] DocEmitidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            ParametroSistemaDTO parametroSistemaDTO = obtenerParametroSistemaDTO();

            if (parametroSistemaDTO != null)
            {
               resultResponse.Message = facturacionService.EnviarDocumento(parametroSistemaDTO.ValorCadena,
                  model.ComprobanteDTO.TipoDocumentoComercialDTO.Codigo, model.ComprobanteDTO.Id);
            }
            else
               resultResponse.Message = SR.GetString("FE_servicio_no_disponible");
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
      [HttpPost("GenerarDocumento")]
      public ActionResult GenerarDocumento([FromBody] DocEmitidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            ParametroSistemaDTO parametroSistemaDTO = obtenerParametroSistemaDTO();

            if (parametroSistemaDTO != null)
            {
               facturacionService.GenerarEnviarDocumento(parametroSistemaDTO.ValorCadena,
                  model.ComprobanteDTO.TipoDocumentoComercialDTO.Codigo, model.ComprobanteDTO.Id, SiNoConstante.N);

               resultResponse.Message = SR.GetString("FE_documento_generado");
            }
            else
               resultResponse.Message = SR.GetString("FE_servicio_no_disponible");
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
      [HttpPost("GenerarArchivo")]
      public ActionResult GenerarArchivo([FromBody] DocEmitidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            ParametroSistemaDTO parametroSistemaDTO = obtenerParametroSistemaDTO();

            if (parametroSistemaDTO != null)
            {
               resultResponse.Message = facturacionService.GenerarDocumento(parametroSistemaDTO.ValorCadena, 
                  model.ComprobanteDTO.TipoDocumentoComercialDTO.Codigo, model.ComprobanteDTO.Id);
            }
            else
               resultResponse.Message = SR.GetString("FE_servicio_no_disponible");
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
      [HttpPost("ActualizarCDR")]
      public ActionResult ActualizarCDR([FromBody] DocEmitidoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            ParametroSistemaDTO parametroSistemaDTO = obtenerParametroSistemaDTO();

            if (parametroSistemaDTO != null)
               resultResponse.Message = facturacionService.ActualizarCDR(parametroSistemaDTO.ValorCadena,
                  model.ComprobanteDTO.TipoDocumentoComercialDTO.Codigo,
                  model.ComprobanteDTO.Id);
            else
               resultResponse.Message = SR.GetString("FE_servicio_no_disponible");
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
