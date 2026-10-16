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
   public class PlanillaProcesoController : AuthorizedController
   {
      private readonly IPlanillaProxy planillaProxy;
      private readonly IMesProxy mesProxy;
      private readonly IPlanillaProcesoProxy planillaProcesoProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IEstadoPlanillaProcesoProxy estadoPlanillaProcesoProxy;
      private readonly IPeriodicidadRemunerativaProxy periodicidadRemunerativaProxy;
      public PlanillaProcesoController(
         IPlanillaProxy planillaProxy,
         IMesProxy mesProxy,
         IPlanillaProcesoProxy planillaProcesoProxy,
         IMonedaProxy monedaProxy,
         IEstadoPlanillaProcesoProxy estadoPlanillaProcesoProxy,
         IPeriodicidadRemunerativaProxy periodicidadRemunerativaProxy
         )
      {
         this.planillaProxy = planillaProxy;
         this.mesProxy = mesProxy;
         this.planillaProcesoProxy = planillaProcesoProxy;
         this.monedaProxy = monedaProxy;
         this.estadoPlanillaProcesoProxy = estadoPlanillaProcesoProxy;
         this.periodicidadRemunerativaProxy = periodicidadRemunerativaProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         PlanillaProcesoViewModel planillaProcesoViewModel = new();

         cargarListas(planillaProcesoViewModel);

         return PartialView(planillaProcesoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PlanillaProcesoViewModel model = JsonSerializer.Deserialize<PlanillaProcesoViewModel>(jsonModel);

         model.PlanillaProcesoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PlanillaProcesoDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.PlanillaProcesoDTO.OrderBy = "p.nombre";

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
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody] PlanillaProcesoViewModel model)
      {
         Logger.InfoStart();

         PlanillaProcesoViewModel planillaProcesoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(planillaProcesoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, planillaProcesoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] PlanillaProcesoViewModel model)
      {

         Logger.InfoStart();

         PlanillaProcesoViewModel planillaProcesoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(planillaProcesoViewModel, model.PlanillaProcesoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, planillaProcesoViewModel);
      }
      private void cargarDataParaEditar(PlanillaProcesoViewModel model, int id)
      {

         PlanillaProcesoDTO planillaProcesoDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            MesDTO mesDTO = mesProxy.ObtenerPorCodigo(new MesDTO() { Codigo = DateTime.Now.Month.ToString("00") });
            MonedaDTO monedaDTO = monedaProxy.SingleSearch(new MonedaDTO() { EsLocal = SiNoConstante.S });

            planillaProcesoDTO = new PlanillaProcesoDTO
            {
               MesId = mesDTO.Id,
               FechaDesde = DateTime.Today,
               FechaHasta = DateTime.Today,
               MonedaDTO = monedaDTO,
               MonedaId = monedaDTO.Id
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
         WebHelper.SelectedListItemSetValue(model.PeriodicidadRemunerativaList, planillaProcesoDTO.PeriodicidadRemunerativaId);
      }
      private void cargarListas(PlanillaProcesoViewModel model)
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

         List<PeriodicidadRemunerativaDTO> periodicidadRemunerativaDTOList = periodicidadRemunerativaProxy.Search(new PeriodicidadRemunerativaDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.PeriodicidadRemunerativaList = WebHelper.ToSelectListItem(periodicidadRemunerativaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpGet("ObtenerMoneda")]
      public JsonResult ObtenerMoneda(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            MonedaDTO monedaDTO = monedaProxy.GetById(new MonedaDTO() { MonedaId = id });

            resultResponse.Data = new
            {
               monedaDTO.EsLocal
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] PlanillaProcesoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PlanillaProcesoDTO planillaProcesoDTO = cargarPlanillaProcesoDTO(model);

            int newId = model.PlanillaProcesoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = planillaProcesoProxy.Insert(planillaProcesoDTO);
                  messageResourceKey = "RH_planilla_proceso_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  planillaProcesoProxy.Update(planillaProcesoDTO);
                  messageResourceKey = "RH_planilla_proceso_actualizado";
                  break;
            }

            resultResponse.Message = SR.GetString(messageResourceKey);
            resultResponse.Data = new
            {
               Id = newId
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
      private PlanillaProcesoDTO cargarPlanillaProcesoDTO(PlanillaProcesoViewModel model)
      {

         try
         {

            PlanillaProcesoDTO planillaProcesoDTO = null;
            if (model.EditAction == ViewModelEditAction.New)
            {
               planillaProcesoDTO = model.PlanillaProcesoDTO;
               planillaProcesoDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
               EstadoPlanillaProcesoDTO estadoPlanillaProcesoDTO = estadoPlanillaProcesoProxy
                  .ObtenerPorCodigo(new EstadoPlanillaProcesoDTO() { Codigo = EstadoPlanillaProcesoConstante.ABIERTO });
               planillaProcesoDTO.EstadoPlanillaProcesoId = estadoPlanillaProcesoDTO.Id;
            }
            else
            {
               planillaProcesoDTO = planillaProcesoProxy.GetById(new PlanillaProcesoDTO() { PlanillaProcesoId = model.PlanillaProcesoDTO.Id });
               model.PlanillaProcesoDTO.EstadoPlanillaProcesoId = planillaProcesoDTO.EstadoPlanillaProcesoId;

               planillaProcesoDTO = model.PlanillaProcesoDTO;
            }

            planillaProcesoDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(planillaProcesoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return model.PlanillaProcesoDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PlanillaProcesoDTO planillaProcesoDTO = planillaProcesoProxy.GetById(new PlanillaProcesoDTO() { PlanillaProcesoId = id });

            AuditDTO(planillaProcesoDTO);

            planillaProcesoProxy.Delete(planillaProcesoDTO);

            resultResponse.Message = SR.GetString("RH_planilla_proceso_anulado");
         }
         catch (Exception e)
         {
            Logger.Error(e);
            resultResponse.Message = e.Message;
            resultResponse.Success = false;
         }

         Logger.InfoFinish();

         return Json(resultResponse);
      }
   }
}
