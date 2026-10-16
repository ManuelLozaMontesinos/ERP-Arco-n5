using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.CM.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.CM.Controllers
{
   [Area("CM")]
   [Route("[controller]")]
   public class ValorizaCompraController : AuthorizedController
   {
      private readonly IOrdenCompraComprobanteProxy ordenCompraComprobanteProxy;
      private readonly ICondicionPagoProxy condicionPagoProxy;
      private readonly IOrdenCompraProxy ordenCompraProxy;
      private readonly ITipoDocumentoComercialProxy tipoDocumentoComercialProxy;
      private readonly AppHelper appHelper;

      public ValorizaCompraController(
         IOrdenCompraComprobanteProxy ordenCompraComprobanteProxy,
         ICondicionPagoProxy condicionPagoProxy,
         IOrdenCompraProxy ordenCompraProxy,
         ITipoDocumentoComercialProxy tipoDocumentoComercialProxy,
         AppHelper appHelper
         )
      {
         this.ordenCompraComprobanteProxy = ordenCompraComprobanteProxy;
         this.condicionPagoProxy = condicionPagoProxy;
         this.ordenCompraProxy = ordenCompraProxy;
         this.tipoDocumentoComercialProxy = tipoDocumentoComercialProxy;
         this.appHelper = appHelper;
      }

      [Route("[action]")]
      public IActionResult Index()
      {
         ValorizaCompraViewModel valorizaCompraViewModel = new();

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

         List<CondicionPagoDTO> condicionPagoDTOList = condicionPagoProxy.Search(new CondicionPagoDTO() {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO 
         });
         valorizaCompraViewModel.CondicionPagoString = string.Concat(SeparadorConstante.DOS_PUNTOS, SelectListFirstOptionNameConstant.SELECT, SeparadorConstante.PUNTO_COMA) +
            string.Join(SeparadorConstante.PUNTO_COMA, condicionPagoDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

         List<TipoDocumentoComercialDTO> tipoDocumentoComercialDTOList = tipoDocumentoComercialProxy.Search(new TipoDocumentoComercialDTO()
         {
            EsInterno = SiNoConstante.N,
            Estado = EstadoConstante.ACTIVO
         });

         valorizaCompraViewModel.TipoDocumentoComercialString = string.Concat(SeparadorConstante.DOS_PUNTOS, SelectListFirstOptionNameConstant.SELECT, SeparadorConstante.PUNTO_COMA) +
            string.Join(SeparadorConstante.PUNTO_COMA, tipoDocumentoComercialDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

          valorizaCompraViewModel.CondicionPagoList = WebHelper.ToSelectListItem(condicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

            return PartialView(valorizaCompraViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ValorizaCompraViewModel model = JsonSerializer.Deserialize<ValorizaCompraViewModel>(jsonModel);

         model.OrdenCompraComprobanteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OrdenCompraComprobanteDTO.OrderBy = "occ.fecha desc,tdc.tipo_doc_com";

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         try
         {
            List<OrdenCompraComprobanteDTO> ordenCompraComprobanteDTOList = ordenCompraComprobanteProxy.Search(model.OrdenCompraComprobanteDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = ordenCompraComprobanteDTOList.Count,
               rows = from f in ordenCompraComprobanteDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.OrdenCompraComprobanteId,
                            f.OrdenCompraId,
                            f.OrdenCompraDTO.Codigo,
                            f.TipoDocumentoComercialId,
                            TipoDocumentoComercial = f.TipoDocumentoComercialDTO.Abreviatura,
                            f.Serie,
                            f.Numero,
                            Fecha = f.Fecha.ToShortDateString(),
                            FechaVcto = f.FechaVencimiento == null ? string.Empty : f.FechaVencimiento.Value.ToShortDateString(),
                            f.OrdenCompraDTO.ProveedorId,
                            f.OrdenCompraDTO.ProveedorDTO.RazonSocial,
                            Moneda = f.OrdenCompraDTO.MonedaDTO.Simbolo,
                            f.MontoBruto,
                            f.MontoImpuesto,
                            f.MontoTotal,
                            f.CondicionPagoId,
                            f.RegistroReferencialId,
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
      [HttpPost("CalcularMontoTotal")]
      public ActionResult CalcularMontoTotal([FromBody] ValorizaCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            CalculadorVentaItemDTO calculadorVentaItemDTO = calcularMonto(model);

            resultResponse.Data = new
            {
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
      private CalculadorVentaItemDTO calcularMonto(ValorizaCompraViewModel model)
      {
         OrdenCompraDTO ordenCompraDTO = ordenCompraProxy.GetById(new OrdenCompraDTO() { OrdenCompraId = model.OrdenCompraComprobanteDTO.OrdenCompraId });

         CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularMontos(new CalculadorVentaItemDTO()
         {
            PrecioUnitario = model.OrdenCompraComprobanteDTO.MontoTotal,
            Cantidad = 1,
            TipoIGV = TipoIgvConstante.GRAVADO,
            ImpuestoTasa = ordenCompraDTO.ImpuestoTasaDTO.Tasa
         });

         return calculadorVentaItemDTO;
      }
      [HttpPost("GrabarOrdenCompraComprobante")]
      public ActionResult GrabarOrdenCompraComprobante([FromBody] ValorizaCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            OrdenCompraComprobanteDTO ordenCompraComprobanteDTO = cargarOrdenCompraComprobanteDTO(model);

            CalculadorVentaItemDTO calculadorVentaItemDTO = calcularMonto(model);

            model.OrdenCompraComprobanteDTO.MontoBruto = calculadorVentaItemDTO.MontoBruto;
            model.OrdenCompraComprobanteDTO.MontoImpuesto = calculadorVentaItemDTO.MontoImpuesto;
            model.OrdenCompraComprobanteDTO.MontoTotal = calculadorVentaItemDTO.MontoTotal;

            int newId = model.OrdenCompraComprobanteDTO.Id;
            string messageResourceKey = string.Empty;
            switch (ordenCompraComprobanteDTO.DTOState)
            {
               case DTOState.Added:
                  newId = ordenCompraComprobanteProxy.Insert(ordenCompraComprobanteDTO);
                  messageResourceKey = "CM_orden_compra_comprobante_agregado";
                  break;

               case DTOState.Modified:
                  ordenCompraComprobanteProxy.Update(ordenCompraComprobanteDTO);
                  messageResourceKey = "CM_orden_compra_comprobante_actualizado";
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
      private OrdenCompraComprobanteDTO cargarOrdenCompraComprobanteDTO(ValorizaCompraViewModel model)
      {

         OrdenCompraComprobanteDTO ordenCompraComprobanteDTO;
         try
         {

            ordenCompraComprobanteDTO = model.OrdenCompraComprobanteDTO;

            AuditDTO(ordenCompraComprobanteDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return ordenCompraComprobanteDTO;
      }
      [HttpPost("EliminarOrdenCompraComprobante")]
      public ActionResult EliminarOrdenCompraComprobante([FromBody] ValorizaCompraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.OrdenCompraComprobanteDTO);

            OrdenCompraComprobanteDTO ordenCompraComprobanteDTO = ordenCompraComprobanteProxy.GetById(model.OrdenCompraComprobanteDTO);

            ordenCompraComprobanteProxy.Delete(ordenCompraComprobanteDTO);

            resultResponse.Message = SR.GetString("CM_orden_compra_comprobante_eliminado");
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
      public FileContentResult ReportTo(string format, string cid, string proveedorId, string fecha, string productoId)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, proveedorId, fecha);

         string reportName = "CM_Valorizacion";
         string downloadFileName = reportName;

         FileContentResult fileContentResult = null;
         switch (format)
         {
            case ReportFormatConstant.PDF:
               fileContentResult = appHelper.ReportToPdf(reportName, parameters, downloadFileName);
               break;

            case ReportFormatConstant.EXCEL:
               fileContentResult = appHelper.ReportToExcel(reportName, parameters, downloadFileName);
               break;
         }

         return fileContentResult;
      }
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string proveedorId, string fecha)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "empresaId", periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId.ToString() },
            { "proveedorId", proveedorId }
         };

         if (!string.IsNullOrEmpty(fecha))
         {
            DateTime date = Convert.ToDateTime(fecha);
            fecha = string.Format(StringFormatConstant.DATE_ISO, date);
            parameters.Add("fecha", fecha);
         }

         return parameters;
      }
   }
}