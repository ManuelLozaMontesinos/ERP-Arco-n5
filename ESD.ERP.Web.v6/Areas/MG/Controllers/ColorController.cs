using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.MG.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.MG.Controllers
{
   [Area("MG")]
   [Route("[controller]")]
   public class ColorController : AuthorizedController
   {
      private readonly IColorProxy colorProxy;
      public ColorController(
         IColorProxy colorProxy
         )
      {
         this.colorProxy = colorProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ColorViewModel colorViewModel = new();

         return PartialView(colorViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ColorViewModel model = JsonSerializer.Deserialize<ColorViewModel>(jsonModel);

         model.ColorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ColorDTO.OrderBy = "nombre";

         try
         {
            List<ColorDTO> colorDTOList = colorProxy.Search(model.ColorDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = colorDTOList.Count,
               rows = from f in colorDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ColorId,
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
      public ActionResult Crear([FromBody] ColorViewModel model)
      {
         Logger.InfoStart();

         ColorViewModel colorViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(colorViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, colorViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ColorViewModel model)
      {

         Logger.InfoStart();

         ColorViewModel colorViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(colorViewModel, model.ColorDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, colorViewModel);
      }
      private void cargarDataParaEditar(ColorViewModel model, int id)
      {

         ColorDTO colorDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            colorDTO = new ColorDTO();
         }
         else
         {
            colorDTO = colorProxy.GetById(new ColorDTO() { ColorId = id });
         }

         model.ColorDTO = colorDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] ColorViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ColorDTO colorDTO = cargarColorDTO(model);

            int newId = model.ColorDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = colorProxy.Insert(colorDTO);
                  messageResourceKey = "MG_color_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  colorProxy.Update(colorDTO);
                  messageResourceKey = "MG_color_actualizado";
                  break;
            }

            colorDTO = colorProxy.GetById(new ColorDTO() { ColorId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, colorDTO.Nombre);
            resultResponse.Data = new
            {
               colorDTO.Id
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
      private ColorDTO cargarColorDTO(ColorViewModel model)
      {

         ColorDTO colorDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               colorDTO = model.ColorDTO;
            }
            else
            {
               colorDTO = colorProxy.GetById(new ColorDTO() 
                  { ColorId = model.ColorDTO.Id });

               colorDTO = model.ColorDTO;
            }

            colorDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(colorDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return colorDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ColorDTO colorDTO = colorProxy
               .GetById(new ColorDTO() { ColorId = id });

            AuditDTO(colorDTO);

            colorProxy.Delete(colorDTO);

            resultResponse.Message = SR.GetString("MG_color_anulado");
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
      [Route("ListPopup")]
      public ActionResult ListPopup()
      {
         ViewBag.ModalEditId = GetModalEditPanelId();
         return PartialView();
      }
      [Route("CreatePopup")]
      public IActionResult CreatePopup([FromBody] ColorViewModel colorViewModel)
      {
         colorViewModel.EditAction = ViewModelEditAction.New;
         colorViewModel.IsNew = true;

         colorViewModel.ModalEditId = GetModalEditPanelId();
         return PartialView(colorViewModel);
      }
      [HttpPost("GrabarPopup")]
      public ActionResult GrabarPopup([FromBody] ColorViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ViewModelState.EditAction = ViewModelEditAction.New;
            ColorDTO colorDTO = cargarColorDTO(model);

            int newId = colorProxy.Insert(colorDTO);

            colorDTO = colorProxy.GetById(new ColorDTO() { ColorId = newId });

            resultResponse.Message = SR.GetString("MG_color_agregado", colorDTO.Nombre);
            resultResponse.Data = new
            {
               colorDTO.Id,
               colorDTO.Nombre
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

   }
}
