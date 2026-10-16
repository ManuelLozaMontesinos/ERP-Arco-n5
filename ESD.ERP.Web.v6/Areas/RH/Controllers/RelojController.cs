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
   public class RelojController : AuthorizedController
   {
      private readonly IRelojProxy relojProxy;

      public RelojController(
         IRelojProxy relojProxy
         )
      {
         this.relojProxy = relojProxy;
      }

      [Route("[action]")]
      public IActionResult Index()
      {
         RelojViewModel relojViewModel = new();

         return PartialView(relojViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();
         RelojViewModel model = JsonSerializer.Deserialize<RelojViewModel>(jsonModel);

         model.RelojDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.RelojDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
         model.RelojDTO.OrderBy = "nombre";

         try
         {
            List<RelojDTO> relojDTOList = relojProxy.Search(model.RelojDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = relojDTOList.Count,
               rows = from f in relojDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.RelojId,
                            f.Nombre,
                            f.RutaArchivo,
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
      public ActionResult Crear([FromBody] RelojViewModel model)
      {
         Logger.InfoStart();

         RelojViewModel relojViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(relojViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, relojViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] RelojViewModel model)
      {

         Logger.InfoStart();

         RelojViewModel relojViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(relojViewModel, model.RelojDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, relojViewModel);
      }
      private void cargarDataParaEditar(RelojViewModel model, int id)
      {

         RelojDTO relojDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            relojDTO = new RelojDTO();
         }
         else
         {
            relojDTO = relojProxy.GetById(new RelojDTO() { RelojId = id });
         }

         model.RelojDTO = relojDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] RelojViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RelojDTO relojDTO = cargarRelojDTO(model);

            int newId = model.RelojDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = relojProxy.Insert(relojDTO);
                  messageResourceKey = "RH_cargo_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  relojProxy.Update(relojDTO);
                  messageResourceKey = "RH_cargo_actualizado";
                  break;
            }

            relojDTO = relojProxy.GetById(new RelojDTO() { RelojId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, relojDTO.Nombre);
            resultResponse.Data = new
            {
               relojDTO.Id
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
      private RelojDTO cargarRelojDTO(RelojViewModel model)
      {

         RelojDTO relojDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               relojDTO = model.RelojDTO;
               relojDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
            }
            else
            {
               relojDTO = relojProxy.GetById(new RelojDTO()
               { RelojId = model.RelojDTO.Id });

               relojDTO = model.RelojDTO;
            }

            relojDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(relojDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return relojDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RelojDTO relojDTO = relojProxy
               .GetById(new RelojDTO() { RelojId = id });

            AuditDTO(relojDTO);

            relojProxy.Delete(relojDTO);

            resultResponse.Message = SR.GetString("RH_cargo_anulado");
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
