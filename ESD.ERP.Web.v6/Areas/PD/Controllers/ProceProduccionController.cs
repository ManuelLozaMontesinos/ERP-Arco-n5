using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.PD.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.PD.Controllers
{
   [Area("PD")]
   [Route("[controller]")]
   public class ProceProduccionController : AuthorizedController
   {
      private readonly IProcesoProduccionProxy procesoProduccionProxy;
      private readonly ISubProcesoProduccionProxy subProcesoProduccionProxy;
      private readonly ISubProcesoProduccionPersonalProxy subProcesoProduccionPersonalProxy;
      public ProceProduccionController(
         IProcesoProduccionProxy procesoProduccionProxy,
         ISubProcesoProduccionProxy subProcesoProduccionProxy,
         ISubProcesoProduccionPersonalProxy subProcesoProduccionPersonalProxy
         )
      {
         this.procesoProduccionProxy = procesoProduccionProxy;
         this.subProcesoProduccionProxy = subProcesoProduccionProxy;
         this.subProcesoProduccionPersonalProxy = subProcesoProduccionPersonalProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ProceProduccionViewModel procesoProduccionViewModel = new();

         return PartialView(procesoProduccionViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ProceProduccionViewModel model = JsonSerializer.Deserialize<ProceProduccionViewModel>(jsonModel);

         model.ProcesoProduccionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ProcesoProduccionDTO.OrderBy = "nombre";

         try
         {
            List<ProcesoProduccionDTO> procesoProduccionDTOList = procesoProduccionProxy.Search(model.ProcesoProduccionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = procesoProduccionDTOList.Count,
               rows = from f in procesoProduccionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ProcesoProduccionId,
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
      public ActionResult Crear([FromBody] ProceProduccionViewModel model)
      {
         Logger.InfoStart();

         ProceProduccionViewModel procesoProduccionViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(procesoProduccionViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, procesoProduccionViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ProceProduccionViewModel model)
      {

         Logger.InfoStart();

         ProceProduccionViewModel procesoProduccionViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(procesoProduccionViewModel, model.ProcesoProduccionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, procesoProduccionViewModel);
      }
      private void cargarDataParaEditar(ProceProduccionViewModel model, int id)
      {

         ProcesoProduccionDTO procesoProduccionDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            procesoProduccionDTO = new ProcesoProduccionDTO();
         }
         else
         {
            procesoProduccionDTO = procesoProduccionProxy.GetById(new ProcesoProduccionDTO() { ProcesoProduccionId = id });
         }

         model.ProcesoProduccionDTO = procesoProduccionDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] ProceProduccionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ProcesoProduccionDTO procesoProduccionDTO = cargarProcesoProduccionDTO(model);

            int newId = model.ProcesoProduccionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = procesoProduccionProxy.Insert(procesoProduccionDTO);
                  messageResourceKey = "PD_proceso_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  procesoProduccionProxy.Update(procesoProduccionDTO);
                  messageResourceKey = "PD_proceso_actualizado";
                  break;
            }

            procesoProduccionDTO = procesoProduccionProxy.GetById(new ProcesoProduccionDTO() { ProcesoProduccionId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, procesoProduccionDTO.Nombre);
            resultResponse.Data = new
            {
               procesoProduccionDTO.Id
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
      private ProcesoProduccionDTO cargarProcesoProduccionDTO(ProceProduccionViewModel model)
      {

         ProcesoProduccionDTO procesoProduccionDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               procesoProduccionDTO = model.ProcesoProduccionDTO;
            }
            else
            {
               procesoProduccionDTO = procesoProduccionProxy.GetById(new ProcesoProduccionDTO() 
                  { ProcesoProduccionId = model.ProcesoProduccionDTO.Id });

               procesoProduccionDTO = model.ProcesoProduccionDTO;
            }

            procesoProduccionDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(procesoProduccionDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return procesoProduccionDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ProcesoProduccionDTO procesoProduccionDTO = procesoProduccionProxy
               .GetById(new ProcesoProduccionDTO() { ProcesoProduccionId = id });

            AuditDTO(procesoProduccionDTO);

            procesoProduccionProxy.Delete(procesoProduccionDTO);

            resultResponse.Message = SR.GetString("PD_proceso_anulado");
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
      [HttpPost("ListarSubProcesoProduccion")]
      public ActionResult ListarSubProcesoProduccion(string jsonModel)
      {

         Logger.InfoStart();

         ProceProduccionViewModel model = JsonSerializer.Deserialize<ProceProduccionViewModel>(jsonModel);

         model.SubProcesoProduccionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.SubProcesoProduccionDTO.OrderBy = "spp.nombre";
         model.SubProcesoProduccionDTO.ProcesoProduccionId = NumberUtil.WhenValueIsZero(model.SubProcesoProduccionDTO.ProcesoProduccionId);

         try
         {
            List<SubProcesoProduccionDTO> subProcesoProduccionDTOList = subProcesoProduccionProxy.Search(model.SubProcesoProduccionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = subProcesoProduccionDTOList.Count,
               rows = from f in subProcesoProduccionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.SubProcesoProduccionId,
                            f.ProcesoProduccionId,
                            ProcesoNombre = f.ProcesoProduccionDTO.Nombre,
                            f.Nombre,
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
      [HttpPost("GrabarSubProcesoProduccion")]
      public ActionResult GrabarSubProcesoProduccion([FromBody] ProceProduccionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            SubProcesoProduccionDTO subProcesoProduccionDTO = cargarSubProcesoProduccionDTO(model);

            int newId = model.SubProcesoProduccionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (subProcesoProduccionDTO.DTOState)
            {
               case DTOState.Added:
                  newId = subProcesoProduccionProxy.Insert(subProcesoProduccionDTO);
                  messageResourceKey = "PD_sub_proceso_agregado";
                  break;

               case DTOState.Modified:
                  subProcesoProduccionProxy.Update(subProcesoProduccionDTO);
                  messageResourceKey = "PD_sub_proceso_actualizado";
                  break;
            }

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
      private SubProcesoProduccionDTO cargarSubProcesoProduccionDTO(ProceProduccionViewModel model)
      {

         SubProcesoProduccionDTO subProcesoProduccionDTO;
         try
         {

            if (model.SubProcesoProduccionDTO.DTOState == DTOState.Added)
            {
               subProcesoProduccionDTO = model.SubProcesoProduccionDTO;
            }
            else
            {
               subProcesoProduccionDTO = subProcesoProduccionProxy.GetById(new SubProcesoProduccionDTO() { SubProcesoProduccionId = model.SubProcesoProduccionDTO.Id });
               subProcesoProduccionDTO = model.SubProcesoProduccionDTO;
            }

            subProcesoProduccionDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(subProcesoProduccionDTO);
         }
         catch (Exception)
         {
            throw;
         }

         return subProcesoProduccionDTO;
      }
      [Route("SubProcesoListPopup")]
      public ActionResult SubProcesoListPopup()
      {

         ProceProduccionViewModel proceProduccionViewModel = new();
         proceProduccionViewModel.ModalEditId = GetModalEditPanelId();

         return PartialView(proceProduccionViewModel);
      }
      [HttpPost("ListarSubProcesoProduccionPopup")]
      public ActionResult ListarSubProcesoProduccionPopup(string jsonModel)
      {

         Logger.InfoStart();

         ProceProduccionViewModel model = JsonSerializer.Deserialize<ProceProduccionViewModel>(jsonModel);

         model.SubProcesoProduccionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.SubProcesoProduccionDTO.OrderBy = "spp.nombre";

         try
         {
            List<SubProcesoProduccionDTO> subProcesoProduccionDTOList = subProcesoProduccionProxy.Search(model.SubProcesoProduccionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = subProcesoProduccionDTOList.Count,
               rows = from f in subProcesoProduccionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.SubProcesoProduccionId,
                            f.ProcesoProduccionId,
                            ProcesoNombre = f.ProcesoProduccionDTO.Nombre,
                            f.Nombre,
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
      [HttpPost("ListarSubProcesoProduccionPersonal")]
      public ActionResult ListarSubProcesoProduccionPersonal(string jsonModel)
      {

         Logger.InfoStart();

         ProceProduccionViewModel model = JsonSerializer.Deserialize<ProceProduccionViewModel>(jsonModel);

         model.SubProcesoProduccionPersonalDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.SubProcesoProduccionPersonalDTO.OrderBy = "prv.razon_social";
         model.SubProcesoProduccionPersonalDTO.Estado = EstadoConstante.ACTIVO;

         try
         {
            List<SubProcesoProduccionPersonalDTO> subProcesoProduccionPersonalDTOList = subProcesoProduccionPersonalProxy.Search(model.SubProcesoProduccionPersonalDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = subProcesoProduccionPersonalDTOList.Count,
               rows = from f in subProcesoProduccionPersonalDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.SubProcesoProduccionPersonalId,
                            f.SubProcesoProduccionId,
                            f.PersonalId,
                            f.PersonalDTO.ProveedorDTO.RazonSocial,
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
      [HttpPost("GrabarSubProcesoProduccionPersonal")]
      public ActionResult GrabarSubProcesoProduccionPersonal([FromBody] ProceProduccionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            SubProcesoProduccionPersonalDTO subProcesoProduccionPersonalDTO = cargarSubProcesoProduccionPersonalDTO(model);

            int newId = model.SubProcesoProduccionPersonalDTO.Id;
            string messageResourceKey = string.Empty;
            switch (subProcesoProduccionPersonalDTO.DTOState)
            {
               case DTOState.Added:
                  newId = subProcesoProduccionPersonalProxy.Insert(subProcesoProduccionPersonalDTO);
                  messageResourceKey = "PD_operador_agregado";
                  break;

               case DTOState.Modified:
                  subProcesoProduccionPersonalProxy.Update(subProcesoProduccionPersonalDTO);
                  messageResourceKey = "PD_operador_actualizado";
                  break;
            }

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
      private SubProcesoProduccionPersonalDTO cargarSubProcesoProduccionPersonalDTO(ProceProduccionViewModel model)
      {

         SubProcesoProduccionPersonalDTO subProcesoProduccionPersonalDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               subProcesoProduccionPersonalDTO = model.SubProcesoProduccionPersonalDTO;
            }
            else
            {
               subProcesoProduccionPersonalDTO = subProcesoProduccionPersonalProxy.GetById(new SubProcesoProduccionPersonalDTO()
                  { SubProcesoProduccionPersonalId = model.SubProcesoProduccionPersonalDTO.Id });

               subProcesoProduccionPersonalDTO = model.SubProcesoProduccionPersonalDTO;
            }

            subProcesoProduccionPersonalDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(subProcesoProduccionPersonalDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return subProcesoProduccionPersonalDTO;
      }
      [HttpPost("EliminarSubProcesoProduccionPersonal")]
      public JsonResult EliminarSubProcesoProduccionPersonal([FromBody] ProceProduccionViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.SubProcesoProduccionPersonalDTO.DTOState = DTOState.Modified;
            AuditDTO(model.SubProcesoProduccionPersonalDTO);

            SubProcesoProduccionPersonalDTO subProcesoProduccionPersonalDTO = subProcesoProduccionPersonalProxy.GetById(model.SubProcesoProduccionPersonalDTO);

            subProcesoProduccionPersonalProxy.Delete(subProcesoProduccionPersonalDTO);

            resultResponse.Message = SR.GetString("PD_operador_eliminado");
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
      [Route("SubProcesoOperadorListPopup")]
      public ActionResult SubProcesoOperadorListPopup([FromBody] ProceProduccionViewModel model)
      {
         model.ModalEditId = GetModalEditPanelId();

         return PartialView(model);
      }
   }
}