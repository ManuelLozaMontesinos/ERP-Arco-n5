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
   public class RepoLiquiCajaController : AuthorizedController
   {
      private readonly ICajaProxy cajaProxy;
      private readonly AppHelper appHelper;

      public RepoLiquiCajaController(
         ICajaProxy cajaProxy,
         AppHelper appHelper
         )
      {
         this.cajaProxy = cajaProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReporteGenViewModel reporteGenViewModel = new()
         {
            Title = "Liquidación de Caja",
            Fecha = DateTime.Today
         };

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

         List<CajaDTO> cajaDTOList = cajaProxy.Search(new CajaDTO()
         {
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            Estado = EstadoConstante.ACTIVO,
         });

         reporteGenViewModel.SeleccionList = WebHelper.ToSelectListItem(cajaDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         return PartialView(reporteGenViewModel);
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string cajaId, string fecha)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid,cajaId, fecha);

         FileContentResult fileContentResult = null;
         switch (format)
         {
            case ReportFormatConstant.PDF:
               fileContentResult = appHelper.ReportToPdf("VT_LiquidacionCaja", parameters, "VT_LiquidacionCaja");
               break;

            case ReportFormatConstant.EXCEL:
            case ReportFormatConstant.EXCELDATA:
               fileContentResult = appHelper.ReportToExcelData("sp_rptvt_liquidacionCaja", parameters, "VT_LiquidacionCaja");
               break;
         }

         return fileContentResult;
      }
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string cajaId, string fecha)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "periodoEmpresaSedeId", periodoEmpresaSedeDTO.PeriodoEmpresaSedeId.ToString() },
            { "cajaId", cajaId }
         };

         DateTime date = Convert.ToDateTime(fecha);
         parameters.Add("fecha", string.Format(StringFormatConstant.DATE_ISO, date));

         return parameters;
      }
   }
}
