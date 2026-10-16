using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.RH.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.RH.Controllers
{
   [Area("RH")]
   [Route("[controller]")]
   public class PlanillaCalculoController : AuthorizedController
   {
      private readonly IPlanillaProxy planillaProxy;
      private readonly IMesProxy mesProxy;
      private readonly IPlanillaProcesoProxy planillaProcesoProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly AppHelper appHelper;
      public PlanillaCalculoController(
         IPlanillaProxy planillaProxy,
         IMesProxy mesProxy,
         IPlanillaProcesoProxy planillaProcesoProxy,
         IMonedaProxy monedaProxy,
         AppHelper appHelper
         )
      {
         this.planillaProxy = planillaProxy;
         this.mesProxy = mesProxy;
         this.planillaProcesoProxy = planillaProcesoProxy;
         this.monedaProxy = monedaProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         PlanillaCalculoViewModel planillaCalculoViewModel = new();

         cargarListas(planillaCalculoViewModel);

         planillaCalculoViewModel.MenuItemList = new List<MenuItem>() {
            new MenuItem() { Key = "calcular", Content="Calcular" },
            new MenuItem() { MenuItemType = MenuItemType.Preview}
         };

         return PartialView(planillaCalculoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PlanillaCalculoViewModel model = JsonSerializer.Deserialize<PlanillaCalculoViewModel>(jsonModel);

         model.PlanillaProcesoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PlanillaProcesoDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.PlanillaProcesoDTO.OrderBy = "m.nombre,p.nombre";

         try
         {
            List<PlanillaProcesoDTO> planillaProcesoDTOList = planillaProcesoProxy.Search(model.PlanillaProcesoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = planillaProcesoDTOList.Count,
               rows = from f in planillaProcesoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PlanillaProcesoId,
                            PlanillaNombre = f.PlanillaDTO.Nombre,
                            MesNombre = f.MesDTO.Nombre,
                            MonedaNombre = f.MonedaDTO.Nombre,
                            f.ProcesoAnual,
                            FechaDesde = f.FechaDesde.ToShortDateString(),
                            FechaHasta = f.FechaHasta.ToShortDateString(),
                            FechaProceso = f.FechaProceso == null ? string.Empty : f.FechaProceso.Value.ToShortDateString(),
                            EstadoCodigo = f.EstadoPlanillaProcesoDTO.Codigo,
                            EstadoNombre = f.EstadoPlanillaProcesoDTO.Nombre,
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
      private void cargarListas(PlanillaCalculoViewModel model)
      {

         List<PlanillaDTO> planillaDTOList = planillaProxy.Search(new PlanillaDTO()
         {
            EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
         });

         model.PlanillaList = WebHelper.ToSelectListItem(planillaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MesDTO> mesDTOList = mesProxy.Search(new MesDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.MesList = WebHelper.ToSelectListItem(mesDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

      }
      [HttpPost("Calcular")]
      public ActionResult Calcular([FromBody] PlanillaCalculoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            planillaProcesoProxy.Calcular(model.PlanillaProcesoDTO);

            resultResponse.Message = SR.GetString("RH_planilla_proceso_calculada");
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
      public FileContentResult ReportTo(string format, string cid)
      {
         byte[] reportData = obtenerReporteData("1");

         return appHelper.ReportToPdf(reportData, "ResumenPlanilla");
      }
      private byte[] obtenerReporteData(string planillaProcesoId)
      {
         Dictionary<string, string> parameters = new();
         parameters.Add("planillaProcesoId", planillaProcesoId);

         byte[] reportData = appHelper.ExecuteReport("RH_ResumenPlanilla", parameters, ReportFormatConstant.PDF);

         return reportData;
      }
   }
}
