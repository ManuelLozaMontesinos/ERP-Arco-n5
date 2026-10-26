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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.RH.Controllers
{
   [Area("RH")]
   [Route("[controller]")]
   public class PlanillaController : AuthorizedController
   {
      private readonly IPlanillaProxy planillaProxy;
      private readonly IPlanillaConceptoRemunerativoProxy planillaConceptoRemunerativoProxy;
      private readonly ITipoTrabajadorProxy tipoTrabajadorProxy;
      public PlanillaController(
         IPlanillaProxy planillaProxy,
         IPlanillaConceptoRemunerativoProxy planillaConceptoRemunerativoProxy,
         ITipoTrabajadorProxy tipoTrabajadorProxy
         )
      {
         this.planillaProxy = planillaProxy;
         this.planillaConceptoRemunerativoProxy = planillaConceptoRemunerativoProxy;
         this.tipoTrabajadorProxy = tipoTrabajadorProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         PlanillaViewModel planillaViewModel = new();

         return PartialView(planillaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PlanillaViewModel model = JsonSerializer.Deserialize<PlanillaViewModel>(jsonModel);

         model.PlanillaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PlanillaDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
         model.PlanillaDTO.OrderBy = "p.nombre";

         try
         {
            List<PlanillaDTO> planillaDTOList = planillaProxy.Search(model.PlanillaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = planillaDTOList.Count,
               rows = from f in planillaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PlanillaId,
                            f.Nombre,
                            f.ProcesoAnual,
                            TipoTrabajador = f.TipoTrabajadorDTO.Nombre,
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
      public ActionResult Crear([FromBody] PlanillaViewModel model)
      {
         Logger.InfoStart();

         PlanillaViewModel planillaViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(planillaViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, planillaViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] PlanillaViewModel model)
      {

         Logger.InfoStart();

         PlanillaViewModel planillaViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(planillaViewModel, model.PlanillaDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, planillaViewModel);
      }
      private void cargarDataParaEditar(PlanillaViewModel model, int id)
      {

         PlanillaDTO planillaDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            planillaDTO = new PlanillaDTO();
         }
         else
         {
            planillaDTO = planillaProxy.GetById(new PlanillaDTO() { PlanillaId = id });
         }

         model.PlanillaDTO = planillaDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.TipoTrabajadorList, planillaDTO.TipoTrabajadorId);
      }
      private void cargarListas(PlanillaViewModel model)
      {
         List<TipoTrabajadorDTO> tipoTrabajadorDTOList = tipoTrabajadorProxy.Search(new TipoTrabajadorDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoTrabajadorList = WebHelper.ToSelectListItem(tipoTrabajadorDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] PlanillaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PlanillaDTO planillaDTO = cargarPlanillaDTO(model);

            int newId = model.PlanillaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = planillaProxy.Insert(planillaDTO);
                  messageResourceKey = "RH_planilla_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  planillaProxy.Update(planillaDTO);
                  messageResourceKey = "RH_planilla_actualizada";
                  break;
            }

            planillaDTO = planillaProxy.GetById(new PlanillaDTO() { PlanillaId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, planillaDTO.Nombre);
            resultResponse.Data = new
            {
               planillaDTO.Id
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
      private PlanillaDTO cargarPlanillaDTO(PlanillaViewModel model)
      {

         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               model.PlanillaDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
            }
            else
            {
               PlanillaDTO currentPlanilla = planillaProxy.GetById(new PlanillaDTO() { PlanillaId = model.PlanillaDTO.Id });
               model.PlanillaDTO.EmpresaId = currentPlanilla.EmpresaId;
            }

            model.PlanillaDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(model.PlanillaDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return model.PlanillaDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PlanillaDTO planillaDTO = planillaProxy.GetById(new PlanillaDTO() { PlanillaId = id });

            AuditDTO(planillaDTO);

            planillaProxy.Delete(planillaDTO);

            resultResponse.Message = SR.GetString("RH_planilla_anulada", planillaDTO.Nombre);
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
      [HttpPost("ListarConcepto")]
      public ActionResult ListarConceptoRemunerativo(string jsonModel)
      {

         Logger.InfoStart();

         PlanillaViewModel model = JsonSerializer.Deserialize<PlanillaViewModel>(jsonModel);

         model.PlanillaConceptoRemunerativoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PlanillaConceptoRemunerativoDTO.OrderBy = "pcr.orden";

         try
         {
            List<PlanillaConceptoRemunerativoDTO> planillaConceptoRemunerativoDTOList = 
               planillaConceptoRemunerativoProxy.Search(model.PlanillaConceptoRemunerativoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = planillaConceptoRemunerativoDTOList.Count,
               rows = from f in planillaConceptoRemunerativoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PlanillaConceptoRemunerativoId,
                            f.PlanillaId,
                            f.ConceptoRemunerativoId,
                            ConceptoRemunerativoNombre = f.ConceptoRemunerativoDTO.Nombre,
                            TipoConceptoRemunerativoNombre= f.ConceptoRemunerativoDTO.TipoConceptoRemunerativoDTO.Nombre,
                            f.Orden,
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
      [HttpPost("GrabarPlanillaConceptoRemunerativo")]
      public ActionResult GrabarPlanillaConceptoRemunerativo([FromBody] PlanillaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PlanillaConceptoRemunerativoDTO planillaConceptoRemunerativoDTO = cargarPlanillaConceptoRemunerativoDTO(model);

            int newId = model.PlanillaConceptoRemunerativoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (planillaConceptoRemunerativoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = planillaConceptoRemunerativoProxy.Insert(planillaConceptoRemunerativoDTO);
                  messageResourceKey = "RH_concepto_remunerativo_agregado";
                  break;

               case DTOState.Modified:
                  planillaConceptoRemunerativoProxy.Update(planillaConceptoRemunerativoDTO);
                  messageResourceKey = "RH_concepto_remunerativo_actualizado";
                  break;
            }

            planillaConceptoRemunerativoDTO = planillaConceptoRemunerativoProxy.
               GetById(new PlanillaConceptoRemunerativoDTO() { PlanillaConceptoRemunerativoId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, planillaConceptoRemunerativoDTO.ConceptoRemunerativoDTO.Nombre);
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
      private PlanillaConceptoRemunerativoDTO cargarPlanillaConceptoRemunerativoDTO(PlanillaViewModel model)
      {

         PlanillaConceptoRemunerativoDTO planillaConceptoRemunerativoDTO;
         try
         {

            planillaConceptoRemunerativoDTO = model.PlanillaConceptoRemunerativoDTO;

            planillaConceptoRemunerativoDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(planillaConceptoRemunerativoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return planillaConceptoRemunerativoDTO;
      }
      [HttpPost("EliminarPlanillaConceptoRemunerativo")]
      public ActionResult EliminarPlanillaConceptoRemunerativo([FromBody] PlanillaViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.PlanillaConceptoRemunerativoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.PlanillaConceptoRemunerativoDTO);

            planillaConceptoRemunerativoProxy.Delete(model.PlanillaConceptoRemunerativoDTO);

            resultResponse.Message = SR.GetString("RH_concepto_remunerativo_sin_nombre_anulado");
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
