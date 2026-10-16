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
   public class OportunidadController : AuthorizedController
   {
      private readonly IOportunidadVentaProxy tipoProfesionOficioProxy;
      public OportunidadController(
         IOportunidadVentaProxy tipoProfesionOficioProxy
         )
      {
         this.tipoProfesionOficioProxy = tipoProfesionOficioProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         OportunidadViewModel tipoProfesionOficioViewModel = new();

         return PartialView(tipoProfesionOficioViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         OportunidadViewModel model = new()
         {
            OportunidadVentaDTO = new OportunidadVentaDTO()
         };

         model.OportunidadVentaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OportunidadVentaDTO.OrderBy = "nombre";

         try
         {
            List<OportunidadVentaDTO> tipoProfesionOficioDTOList = tipoProfesionOficioProxy.Search(model.OportunidadVentaDTO);

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
                            f.OportunidadVentaId,
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
      public ActionResult Crear([FromBody] OportunidadViewModel model)
      {
         Logger.InfoStart();

         OportunidadViewModel tipoProfesionOficioViewModel = new()
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
      public ActionResult Editar([FromBody] OportunidadViewModel model)
      {

         Logger.InfoStart();

         OportunidadViewModel tipoProfesionOficioViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(tipoProfesionOficioViewModel, model.OportunidadVentaDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, tipoProfesionOficioViewModel);
      }
      private void cargarDataParaEditar(OportunidadViewModel model, int id)
      {

         OportunidadVentaDTO tipoProfesionOficioDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            tipoProfesionOficioDTO = new OportunidadVentaDTO();
         }
         else
         {
            tipoProfesionOficioDTO = tipoProfesionOficioProxy.GetById(new OportunidadVentaDTO() { OportunidadVentaId = id });
         }

         model.OportunidadVentaDTO = tipoProfesionOficioDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] OportunidadViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            OportunidadVentaDTO tipoProfesionOficioDTO = cargarOportunidadVentaDTO(model);

            int newId = model.OportunidadVentaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = tipoProfesionOficioProxy.Insert(tipoProfesionOficioDTO);
                  messageResourceKey = "MK_oportunidad_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  tipoProfesionOficioProxy.Update(tipoProfesionOficioDTO);
                  messageResourceKey = "MK_oportunidad_actualizada";
                  break;
            }

            tipoProfesionOficioDTO = tipoProfesionOficioProxy.GetById(new OportunidadVentaDTO() { OportunidadVentaId = newId });

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
      private OportunidadVentaDTO cargarOportunidadVentaDTO(OportunidadViewModel model)
      {

         OportunidadVentaDTO tipoProfesionOficioDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               tipoProfesionOficioDTO = model.OportunidadVentaDTO;
            }
            else
            {
               tipoProfesionOficioDTO = tipoProfesionOficioProxy.GetById(new OportunidadVentaDTO() 
                  { OportunidadVentaId = model.OportunidadVentaDTO.Id });

               tipoProfesionOficioDTO = model.OportunidadVentaDTO;
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
            OportunidadVentaDTO tipoProfesionOficioDTO = tipoProfesionOficioProxy
               .GetById(new OportunidadVentaDTO() { OportunidadVentaId = id });

            AuditDTO(tipoProfesionOficioDTO);

            tipoProfesionOficioProxy.Delete(tipoProfesionOficioDTO);

            resultResponse.Message = SR.GetString("MK_oportunidad_anulada");
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
