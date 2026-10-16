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
   public class TipoProfesionController : AuthorizedController
   {
      private readonly ITipoProfesionOficioProxy tipoProfesionOficioProxy;
      public TipoProfesionController(
         ITipoProfesionOficioProxy tipoProfesionOficioProxy
         )
      {
         this.tipoProfesionOficioProxy = tipoProfesionOficioProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         TipoProfesionViewModel tipoProfesionOficioViewModel = new();

         return PartialView(tipoProfesionOficioViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         TipoProfesionViewModel model = new()
         {
            TipoProfesionOficioDTO = new TipoProfesionOficioDTO()
         };

         model.TipoProfesionOficioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.TipoProfesionOficioDTO.OrderBy = "nombre";

         try
         {
            List<TipoProfesionOficioDTO> tipoProfesionOficioDTOList = tipoProfesionOficioProxy.Search(model.TipoProfesionOficioDTO);

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
                            f.TipoProfesionOficioId,
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
      public ActionResult Crear([FromBody] TipoProfesionViewModel model)
      {
         Logger.InfoStart();

         TipoProfesionViewModel tipoProfesionOficioViewModel = new()
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
      public ActionResult Editar([FromBody] TipoProfesionViewModel model)
      {

         Logger.InfoStart();

         TipoProfesionViewModel tipoProfesionOficioViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(tipoProfesionOficioViewModel, model.TipoProfesionOficioDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, tipoProfesionOficioViewModel);
      }
      private void cargarDataParaEditar(TipoProfesionViewModel model, int id)
      {

         TipoProfesionOficioDTO tipoProfesionOficioDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            tipoProfesionOficioDTO = new TipoProfesionOficioDTO();
         }
         else
         {
            tipoProfesionOficioDTO = tipoProfesionOficioProxy.GetById(new TipoProfesionOficioDTO() { TipoProfesionOficioId = id });
         }

         model.TipoProfesionOficioDTO = tipoProfesionOficioDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] TipoProfesionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TipoProfesionOficioDTO tipoProfesionOficioDTO = cargarTipoProfesionOficioDTO(model);

            int newId = model.TipoProfesionOficioDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = tipoProfesionOficioProxy.Insert(tipoProfesionOficioDTO);
                  messageResourceKey = "MK_tipo_profesion_oficio_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  tipoProfesionOficioProxy.Update(tipoProfesionOficioDTO);
                  messageResourceKey = "MK_tipo_profesion_oficio_actualizado";
                  break;
            }

            tipoProfesionOficioDTO = tipoProfesionOficioProxy.GetById(new TipoProfesionOficioDTO() { TipoProfesionOficioId = newId });

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
      private TipoProfesionOficioDTO cargarTipoProfesionOficioDTO(TipoProfesionViewModel model)
      {

         TipoProfesionOficioDTO tipoProfesionOficioDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               tipoProfesionOficioDTO = model.TipoProfesionOficioDTO;
            }
            else
            {
               tipoProfesionOficioDTO = tipoProfesionOficioProxy.GetById(new TipoProfesionOficioDTO() 
                  { TipoProfesionOficioId = model.TipoProfesionOficioDTO.Id });

               tipoProfesionOficioDTO = model.TipoProfesionOficioDTO;
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
            TipoProfesionOficioDTO tipoProfesionOficioDTO = tipoProfesionOficioProxy
               .GetById(new TipoProfesionOficioDTO() { TipoProfesionOficioId = id });

            AuditDTO(tipoProfesionOficioDTO);

            tipoProfesionOficioProxy.Delete(tipoProfesionOficioDTO);

            resultResponse.Message = SR.GetString("MK_tipo_profesion_oficio_anulado");
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
