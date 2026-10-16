using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.RH.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.RH.Controllers
{
   [Area("RH")]
   [Route("[controller]")]
   [CheckAndHandleFileResult]
   public class PlanillaRegulaController : AuthorizedController
   {
      private readonly IPlanillaProxy planillaProxy;
      private readonly IMesProxy mesProxy;
      private readonly IPlanillaProcesoProxy planillaProcesoProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IPlanillaProcesoRegularizacionProxy planillaProcesoRegularizacionProxy;
      public PlanillaRegulaController(
         IPlanillaProxy planillaProxy,
         IMesProxy mesProxy,
         IPlanillaProcesoProxy planillaProcesoProxy,
         IMonedaProxy monedaProxy,
         IPlanillaProcesoRegularizacionProxy planillaProcesoRegularizacionProxy
         )
      {
         this.planillaProxy = planillaProxy;
         this.mesProxy = mesProxy;
         this.planillaProcesoProxy = planillaProcesoProxy;
         this.monedaProxy = monedaProxy;
         this.planillaProcesoRegularizacionProxy = planillaProcesoRegularizacionProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         PlanillaRegulaViewModel planillaHoraViewModel = new();

         cargarListas(planillaHoraViewModel);

         planillaHoraViewModel.MenuItemList = new List<MenuItem>() {
            new MenuItem() { MenuItemType = MenuItemType.Edit}
         };

         return PartialView(planillaHoraViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PlanillaRegulaViewModel model = JsonSerializer.Deserialize<PlanillaRegulaViewModel>(jsonModel);

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
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] PlanillaRegulaViewModel model)
      {

         Logger.InfoStart();

         PlanillaRegulaViewModel planillaHoraViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(planillaHoraViewModel, model.PlanillaProcesoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, planillaHoraViewModel);
      }
      private void cargarDataParaEditar(PlanillaRegulaViewModel model, int id)
      {

         PlanillaProcesoDTO planillaProcesoDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            planillaProcesoDTO = new PlanillaProcesoDTO
            {
               FechaDesde = DateTime.Today,
               FechaHasta = DateTime.Today,
               MonedaDTO = new MonedaDTO() { EsLocal = SiNoConstante.S}
            };
         }
         else
         {
            planillaProcesoDTO = planillaProcesoProxy.GetById(new PlanillaProcesoDTO() { PlanillaProcesoId = id });
         }

         model.PlanillaProcesoDTO = planillaProcesoDTO;
         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.PlanillaList, planillaProcesoDTO.PlanillaId);
         WebHelper.SelectedListItemSetValue(model.MesList, planillaProcesoDTO.MesId);
         WebHelper.SelectedListItemSetValue(model.MonedaList, planillaProcesoDTO.MonedaId);

         model.MenuItemList = new List<MenuItem>() {
            new MenuItem() { MenuItemType = MenuItemType.Exit }
         };
      }
      private void cargarListas(PlanillaRegulaViewModel model)
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
      [HttpPost("ListarRegularizacion")]
      public ActionResult ListarRegularizacion(string jsonModel)
      {

         Logger.InfoStart();

         PlanillaRegulaViewModel model = JsonSerializer.Deserialize<PlanillaRegulaViewModel>(jsonModel);

         try
         {
            List<PlanillaProcesoRegularizacionDTO> planillaProcesoRegularizacionDTOList = 
               planillaProcesoRegularizacionProxy.Search(model.PlanillaProcesoRegularizacionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = planillaProcesoRegularizacionDTOList.Count,
               rows = from f in planillaProcesoRegularizacionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PlanillaProcesoRegularizacionId,
                            f.PlanillaProcesoId,
                            f.PersonalId,
                            f.PersonalDTO.ProveedorDTO.RazonSocial,
                            f.ConceptoRemunerativoId,
                            ConceptoRemunerativoNombre = f.ConceptoRemunerativoDTO.Nombre,
                            f.Monto,
                            f.Observacion,
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
      [HttpPost("GrabarRegularizacion")]
      public ActionResult GrabarRegularizacion([FromBody] PlanillaRegulaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PlanillaProcesoRegularizacionDTO personalRemuneracionDTO = cargarPlanillaProcesoRegularizacionDTO(model);

            int newId = model.PlanillaProcesoRegularizacionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (personalRemuneracionDTO.DTOState)
            {
               case DTOState.Added:
                  newId = planillaProcesoRegularizacionProxy.Insert(personalRemuneracionDTO);
                  messageResourceKey = "RH_personal_regularizacion_actualizada";
                  break;

               case DTOState.Modified:
                  planillaProcesoRegularizacionProxy.Update(personalRemuneracionDTO);
                  messageResourceKey = "RH_personal_regularizacion_actualizada";
                  break;
            }

            personalRemuneracionDTO = planillaProcesoRegularizacionProxy.
               GetById(new PlanillaProcesoRegularizacionDTO() { PlanillaProcesoRegularizacionId = newId });

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
      private PlanillaProcesoRegularizacionDTO cargarPlanillaProcesoRegularizacionDTO(PlanillaRegulaViewModel model)
      {

         PlanillaProcesoRegularizacionDTO personalRemuneracionDTO;
         try
         {

            personalRemuneracionDTO = model.PlanillaProcesoRegularizacionDTO;

            personalRemuneracionDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(personalRemuneracionDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return personalRemuneracionDTO;
      }
      [HttpPost("EliminarRegularizacion")]
      public ActionResult EliminarRegularizacion([FromBody] PlanillaRegulaViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.PlanillaProcesoRegularizacionDTO.DTOState = DTOState.Deleted;

            planillaProcesoRegularizacionProxy.Delete(model.PlanillaProcesoRegularizacionDTO);

            resultResponse.Message = SR.GetString("RH_personal_regularizacion_anulada");

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
