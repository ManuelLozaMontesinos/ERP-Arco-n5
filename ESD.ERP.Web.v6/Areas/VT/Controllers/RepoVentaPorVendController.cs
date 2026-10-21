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
   public class RepoVentaPorVendController : AuthorizedController
   {
      private readonly IMesContableProxy mesContableProxy;
      private readonly AppHelper appHelper;

      public RepoVentaPorVendController(
         IMesContableProxy mesContableProxy,
         AppHelper appHelper
         )
      { 
         this.mesContableProxy = mesContableProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReporteGenViewModel reporteGenViewModel = new()
         {
            Title = "Ventas por Vendedor",
            Fecha = DateTime.Today
         };

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

         List<MesContableDTO> mesContableDTOList = mesContableProxy.Search(new MesContableDTO()
         {
            OrderBy = "mc.mes_contable",
            NotMesContableList = new List<string>() { MesContableConstante.APERTURA, MesContableConstante.CIERRE },
            Estado = EstadoConstante.ACTIVO,
         });
         reporteGenViewModel.SeleccionList = WebHelper.ToSelectListItem(mesContableDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         return PartialView(reporteGenViewModel);
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string tipoReporte, string mesContableId, string fechaDesde, string fechaHasta, string excluirObsequio)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, tipoReporte, mesContableId, fechaDesde, fechaHasta, excluirObsequio);

         string reportName = string.Empty;
         string dataName = string.Empty;
         string downloadFileName = string.Empty;
         string store = string.Empty;

        if(excluirObsequio == SiNoConstante.S)
            store = "sp_rptvt_ventaPorVendedorSinObsequio";
        if(excluirObsequio == SiNoConstante.N)
            store = "sp_rptvt_ventaPorVendedor";


         switch (tipoReporte)
         {

            case DigitoConstante.UNO:
               reportName = "VT_VendedorVentaAnual";
               dataName = store;
               downloadFileName = reportName;
               break;
            case DigitoConstante.DOS:
               reportName = "VT_VendedorVentaMes";
               dataName = store;
               downloadFileName = reportName;
               break;
            case DigitoConstante.TRES:
               reportName = "VT_VentaXVendedor";
               dataName = store;
               downloadFileName = reportName;
               break;
            case DigitoConstante.CUATRO:
               reportName = "VT_VendedorVentaAnual";
               dataName = "sp_rptvt_estadisticaVentaXVendedor";
               downloadFileName = reportName;
               break;
            case DigitoConstante.CINCO:
               reportName = "VT_VendedorVentaMes";
               dataName = "sp_rptvt_estadisticaVentaXVendedor";
               downloadFileName = reportName;
               break;
         }

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
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string tipoReporte, string mesContableId, string fechaDesde, string fechaHasta, string excluirObsequio)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "periodoId", periodoEmpresaSedeDTO.PeriodoId.ToString() }
         };

         if (!string.IsNullOrEmpty(mesContableId))
         {
            MesContableDTO mesContableDTO = mesContableProxy.GetById(new MesContableDTO() { MesContableId = Convert.ToInt32(mesContableId) });
            parameters.Add("mes", mesContableDTO.Codigo);
         }
         else
            parameters.Add("mes", null);

         if (tipoReporte == DigitoConstante.UNO)
         {
            string fechaDesdeIso = string.Empty;
            if (!string.IsNullOrEmpty(fechaDesde))
            {
               DateTime date = Convert.ToDateTime(fechaDesde);
               fechaDesdeIso = string.Format(StringFormatConstant.DATE_ISO, date);
            }
            parameters.Add("fechaDesde", fechaDesdeIso);

            string fechaHastaIso = string.Empty;
            if (!string.IsNullOrEmpty(fechaHasta))
            {
               DateTime date = Convert.ToDateTime(fechaHasta);
               fechaHastaIso = string.Format(StringFormatConstant.DATE_ISO, date);
            }
            parameters.Add("fechaHasta", fechaHastaIso);
         }

         return parameters;
      }
   }
}
