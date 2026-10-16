using ESD.Core;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.CM.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.CM.Controllers
{
   [Area("CM")]
   [Route("[controller]")]
   public class RepoValorizaComprobController : AuthorizedController
   {
      private readonly MesProxy mesProxy;
      private readonly AppHelper appHelper;

      public RepoValorizaComprobController(
         MesProxy mesProxy,
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
            Title = "Reporte Valorizacion por Mes"
         };

         List<MesDTO> mesDTOList = mesProxy.Search(new MesDTO(){ Estado = EstadoConstante.ACTIVO });
         reporteGenViewModel.SeleccionList= WebHelper.ToSelectListItem(mesDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

            return PartialView(reporteGenViewModel);
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string mesId, string proveedorId)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, mesId, proveedorId);

         string reportName = "CM_OrdenCompra";
         string dataName = "sp_rptcm_valorizaComprobante";
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

            case ReportFormatConstant.EXCELDATA:
               fileContentResult = appHelper.ReportToExcelData(dataName, parameters, downloadFileName);
               break;
         }

         return fileContentResult;
      }
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string mesId, string proveedorId)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "periodoId", periodoEmpresaSedeDTO.PeriodoDTO.PeriodoId.ToString() }
         };

        parameters.Add("mes", mesId);

         if (!string.IsNullOrEmpty(proveedorId))
            parameters.Add("proveedorId", proveedorId);
         else
            parameters.Add("proveedorId", null);

         return parameters;
      }
   }
}
