using ESD.Core;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class RepoRegVentasController : AuthorizedController
   {
      private readonly IMesProxy mesProxy;
      private readonly AppHelper appHelper;

      public RepoRegVentasController(
         IMesProxy mesProxy,
         AppHelper appHelper
         )
      {
         this.mesProxy = mesProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReporteGenViewModel reporteGenViewModel = new()
         {
            Title = "Registro de Ventas",
            Fecha = DateTime.Today
         };

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

         List<MesDTO> mesDTOList = mesProxy.Search(new MesDTO()
         {
            OrderBy = "m.mes",
            Estado = EstadoConstante.ACTIVO,
         });

         reporteGenViewModel.SeleccionList = WebHelper.ToSelectListItem(mesDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         return PartialView(reporteGenViewModel);
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string tipoReporte, string mesId, string fechaDesde, string fechaHasta, 
         string esOficial)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, mesId, fechaDesde, fechaHasta, esOficial);

         string reportName = string.Empty;
         string dataName = string.Empty;
         if (tipoReporte == SiNoConstante.S)
         {
            reportName = esOficial == SiNoConstante.S ? "VT_RegistroVentaOficial" : "VT_RegistroVentaGeneral";
            dataName = "sp_rptvt_registroVenta";
         }
         else
         {
            reportName = "VT_RegistroVentaDetalle";
            dataName = "sp_rptvt_registroVentaProducto";
         }

         string downloadFileName = "VT_RegistroVenta";

         FileContentResult fileContentResult = null;
         switch (format)
         {
            case ReportFormatConstant.PDF:
               fileContentResult = appHelper.ReportToPdf(reportName, parameters, downloadFileName);
               break;

            case ReportFormatConstant.EXCEL:
            case ReportFormatConstant.EXCELDATA:
               fileContentResult = appHelper.ReportToExcelData(dataName, parameters, downloadFileName);
               break;
         }

         return fileContentResult;
      }
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string mesId, string fechaDesde, string fechaHasta, string esOficial)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "periodoEmpresaSedeId", periodoEmpresaSedeDTO.PeriodoEmpresaSedeId.ToString() }
         };

         if (!string.IsNullOrEmpty(mesId))
         {
            MesDTO mesDTO = mesProxy.GetById(new MesDTO() { MesId = Convert.ToInt32(mesId) });
            mesId = mesDTO.Codigo;
         }
         parameters.Add("mes", mesId);

         if (!string.IsNullOrEmpty(fechaDesde))
         {
            DateTime date = Convert.ToDateTime(fechaDesde);
            fechaDesde = string.Format(StringFormatConstant.DATE_ISO, date);
         }
         else
            fechaDesde = string.Empty;
         parameters.Add("fechaDesde", fechaDesde);

         if (!string.IsNullOrEmpty(fechaHasta))
         {
            DateTime date = Convert.ToDateTime(fechaHasta);
            fechaHasta = string.Format(StringFormatConstant.DATE_ISO, date);
         }
         else
            fechaHasta = string.Empty;
         parameters.Add("fechaHasta", fechaHasta);

         if (esOficial == SiNoConstante.S) {
            parameters.Add("incluyeInterno", SiNoConstante.N);
            parameters.Add("esOficial", SiNoConstante.S);
         }
         else{
            parameters.Add("incluyeInterno", SiNoConstante.S);
            parameters.Add("esOficial", SiNoConstante.N);
         }

         return parameters;
      }
   }
}
