using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.MK.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.MK.Controllers
{
   [Area("MK")]
   [Route("[controller]")]
   public class LeadSourceController : AuthorizedController
   {
      private readonly ILeadSourceProxy tipoProfesionOficioProxy;
      public LeadSourceController(
         ILeadSourceProxy tipoProfesionOficioProxy
         )
      {
         this.tipoProfesionOficioProxy = tipoProfesionOficioProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         LeadSourceViewModel tipoProfesionOficioViewModel = new();

         return PartialView(tipoProfesionOficioViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         LeadSourceViewModel model = new()
         {
            LeadSourceDTO = new LeadSourceDTO()
         };

         model.LeadSourceDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.LeadSourceDTO.OrderBy = "nombre";

         try
         {
            List<LeadSourceDTO> tipoProfesionOficioDTOList = tipoProfesionOficioProxy.Search(model.LeadSourceDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = tipoProfesionOficioDTOList.Count,
               rows = from f in tipoProfesionOficioDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.LeadSourceId,
                            f.Nombre,
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
      public ActionResult Crear([FromBody] LeadSourceViewModel model)
      {
         Logger.InfoStart();

         LeadSourceViewModel tipoProfesionOficioViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(tipoProfesionOficioViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, tipoProfesionOficioViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] LeadSourceViewModel model)
      {

         Logger.InfoStart();

         LeadSourceViewModel tipoProfesionOficioViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(tipoProfesionOficioViewModel, model.LeadSourceDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, tipoProfesionOficioViewModel);
      }
      private void cargarDataParaEditar(LeadSourceViewModel model, int id)
      {

         LeadSourceDTO tipoProfesionOficioDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            tipoProfesionOficioDTO = new LeadSourceDTO();
         }
         else
         {
            tipoProfesionOficioDTO = tipoProfesionOficioProxy.GetById(new LeadSourceDTO() { LeadSourceId = id });
         }

         model.LeadSourceDTO = tipoProfesionOficioDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] LeadSourceViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            LeadSourceDTO tipoProfesionOficioDTO = cargarLeadSourceDTO(model);

            int newId = model.LeadSourceDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = tipoProfesionOficioProxy.Insert(tipoProfesionOficioDTO);
                  messageResourceKey = "MK_lead_source_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  tipoProfesionOficioProxy.Update(tipoProfesionOficioDTO);
                  messageResourceKey = "MK_lead_source_actualizado";
                  break;
            }

            tipoProfesionOficioDTO = tipoProfesionOficioProxy.GetById(new LeadSourceDTO() { LeadSourceId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, tipoProfesionOficioDTO.Nombre);
            resultResponse.Data = new
            {
               tipoProfesionOficioDTO.Id
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
      private LeadSourceDTO cargarLeadSourceDTO(LeadSourceViewModel model)
      {

         LeadSourceDTO tipoProfesionOficioDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               tipoProfesionOficioDTO = model.LeadSourceDTO;
            }
            else
            {
               tipoProfesionOficioDTO = tipoProfesionOficioProxy.GetById(new LeadSourceDTO() 
                  { LeadSourceId = model.LeadSourceDTO.Id });

               tipoProfesionOficioDTO = model.LeadSourceDTO;
            }

            tipoProfesionOficioDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(tipoProfesionOficioDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return tipoProfesionOficioDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            LeadSourceDTO tipoProfesionOficioDTO = tipoProfesionOficioProxy
               .GetById(new LeadSourceDTO() { LeadSourceId = id });

            AuditDTO(tipoProfesionOficioDTO);

            tipoProfesionOficioProxy.Delete(tipoProfesionOficioDTO);

            resultResponse.Message = SR.GetString("MK_lead_source_anulado");
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
