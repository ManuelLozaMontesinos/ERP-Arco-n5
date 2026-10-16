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
   public class RutaProduccionController : AuthorizedController
   {
      private readonly IRutaProduccionProxy rutaProduccionProxy;
      private readonly IProcesoProduccionProxy procesoProduccionProxy;
      private readonly ISubProcesoProduccionProxy subProcesoProduccionProxy;
      private readonly ISubProcesoProduccionPersonalProxy subProcesoProduccionPersonalProxy;
      public RutaProduccionController(
         IRutaProduccionProxy rutaProduccionProxy,
         IProcesoProduccionProxy procesoProduccionProxy,
         ISubProcesoProduccionProxy subProcesoProduccionProxy,
         ISubProcesoProduccionPersonalProxy subProcesoProduccionPersonalProxy
         )
      {
         this.rutaProduccionProxy = rutaProduccionProxy;
         this.procesoProduccionProxy = procesoProduccionProxy;
         this.subProcesoProduccionProxy = subProcesoProduccionProxy;
         this.subProcesoProduccionPersonalProxy = subProcesoProduccionPersonalProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         RutaProduccionViewModel rutaProduccionViewModel = new();

         return PartialView(rutaProduccionViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         RutaProduccionViewModel model = JsonSerializer.Deserialize<RutaProduccionViewModel>(jsonModel);

         model.RutaProduccionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.RutaProduccionDTO.OrderBy = "nombre";

         try
         {
            List<RutaProduccionDTO> rutaProduccionDTOList = rutaProduccionProxy.Search(model.RutaProduccionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = rutaProduccionDTOList.Count,
               rows = from f in rutaProduccionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.RutaProduccionId,
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
      public ActionResult Crear([FromBody] RutaProduccionViewModel model)
      {
         Logger.InfoStart();

         RutaProduccionViewModel rutaProduccionViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(rutaProduccionViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, rutaProduccionViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] RutaProduccionViewModel model)
      {

         Logger.InfoStart();

         RutaProduccionViewModel rutaProduccionViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(rutaProduccionViewModel, model.RutaProduccionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, rutaProduccionViewModel);
      }
      private void cargarDataParaEditar(RutaProduccionViewModel model, int id)
      {

         RutaProduccionDTO rutaProduccionDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            rutaProduccionDTO = new RutaProduccionDTO();
         }
         else
         {
            rutaProduccionDTO = rutaProduccionProxy.GetById(new RutaProduccionDTO() { RutaProduccionId = id });
         }

         model.RutaProduccionDTO = rutaProduccionDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] RutaProduccionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RutaProduccionDTO rutaProduccionDTO = cargarRutaProduccionDTO(model);

            int newId = model.RutaProduccionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = rutaProduccionProxy.Insert(rutaProduccionDTO);
                  messageResourceKey = "PD_ruta_produccion_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  rutaProduccionProxy.Update(rutaProduccionDTO);
                  messageResourceKey = "PD_ruta_produccion_actualizada";
                  break;
            }

            rutaProduccionDTO = rutaProduccionProxy.GetById(new RutaProduccionDTO() { RutaProduccionId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, rutaProduccionDTO.Nombre);
            resultResponse.Data = new
            {
               rutaProduccionDTO.Id
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
      private RutaProduccionDTO cargarRutaProduccionDTO(RutaProduccionViewModel model)
      {

         RutaProduccionDTO rutaProduccionDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               rutaProduccionDTO = model.RutaProduccionDTO;
            }
            else
            {
               rutaProduccionDTO = rutaProduccionProxy.GetById(new RutaProduccionDTO() 
                  { RutaProduccionId = model.RutaProduccionDTO.Id });

               rutaProduccionDTO = model.RutaProduccionDTO;
            }

            rutaProduccionDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(rutaProduccionDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return rutaProduccionDTO;
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
      public ActionResult ListarProcesoProduccion(string jsonModel)
      {

         Logger.InfoStart();

         RutaProduccionViewModel model = JsonSerializer.Deserialize<RutaProduccionViewModel>(jsonModel);

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
      public ActionResult GrabarSubProcesoProduccion([FromBody] RutaProduccionViewModel model)
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
      private SubProcesoProduccionDTO cargarSubProcesoProduccionDTO(RutaProduccionViewModel model)
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
      [HttpPost("ListarSubProcesoProduccionPersonal")]
      public ActionResult ListarSubProcesoProduccionPersonal(string jsonModel)
      {

         Logger.InfoStart();

         RutaProduccionViewModel model = JsonSerializer.Deserialize<RutaProduccionViewModel>(jsonModel);

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
      public ActionResult GrabarSubProcesoProduccionPersonal([FromBody] RutaProduccionViewModel model)
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
      private SubProcesoProduccionPersonalDTO cargarSubProcesoProduccionPersonalDTO(RutaProduccionViewModel model)
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
      public JsonResult EliminarSubProcesoProduccionPersonal([FromBody] RutaProduccionViewModel model)
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
   }
}