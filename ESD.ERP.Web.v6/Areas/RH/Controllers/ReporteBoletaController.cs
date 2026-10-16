using ESD.Core;
using ESD.Core.Web;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.RH.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Controllers
{
   [Area("RH")]
   [Route("[controller]")]
   [CheckAndHandleFileResult]
   public class ReporteBoletaController : AuthorizedController
   {
      private readonly IPlanillaProcesoProxy planillaProcesoProxy;
      private readonly AppHelper appHelper;

      public ReporteBoletaController(
          IPlanillaProcesoProxy planillaProcesoProxy,
          AppHelper appHelper
         )
      {
         this.planillaProcesoProxy = planillaProcesoProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReporteBoletaViewModel reporteBoletaViewModel = new();

         int periodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId();

         List<PlanillaProcesoDTO> planillaProcesoDTOList = planillaProcesoProxy.Search(new PlanillaProcesoDTO() 
         { PeriodoEmpresaSedeId = periodoEmpresaSedeId });

         reporteBoletaViewModel.PlanillaProcesoList = WebHelper.ToSelectListItem(planillaProcesoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         return PartialView(reporteBoletaViewModel);
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string planillaProcesoId, string personalId)
      {
         byte[] reportData = obtenerReporteData(planillaProcesoId, personalId);

         return appHelper.ReportToPdf(reportData, "RH_Boleta");
      }
      private byte[] obtenerReporteData(string planillaProcesoId, string personalId)
      {
         Dictionary<string, string> parameters = new()
         {
            { "planillaProcesoId", planillaProcesoId }
         };

         if (string.IsNullOrEmpty(personalId))
            parameters.Add("personalId", DigitoConstante.CERO);
         else
            parameters.Add("personalId", personalId);

         byte[] reportData = appHelper.ExecuteReport("RH_Boleta", parameters, ReportFormatConstant.PDF);

         return reportData;
      }
   }
}