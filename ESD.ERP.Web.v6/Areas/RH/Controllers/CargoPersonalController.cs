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
   public class CargoPersonalController : AuthorizedController
   {
      private readonly ICargoPersonalProxy cargoPersonalProxy;

      public CargoPersonalController(
         ICargoPersonalProxy cargoPersonalProxy
         )
      {
         this.cargoPersonalProxy = cargoPersonalProxy;
      }

      [Route("[action]")]
      public IActionResult Index()
      {
         CargoPersonalViewModel cargoPersonalViewModel = new();

         return PartialView(cargoPersonalViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         CargoPersonalViewModel model = new()
         {
            CargoPersonalDTO = new CargoPersonalDTO()
         };

         model.CargoPersonalDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CargoPersonalDTO.OrderBy = "nombre";

         try
         {
            List<CargoPersonalDTO> cargoPersonalDTOList = cargoPersonalProxy.Search(model.CargoPersonalDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = cargoPersonalDTOList.Count,
               rows = from f in cargoPersonalDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.CargoPersonalId,
                            f.Codigo,
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
      public ActionResult Crear([FromBody] CargoPersonalViewModel model)
      {
         Logger.InfoStart();

         CargoPersonalViewModel cargoPersonalViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(cargoPersonalViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, cargoPersonalViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] CargoPersonalViewModel model)
      {

         Logger.InfoStart();

         CargoPersonalViewModel cargoPersonalViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(cargoPersonalViewModel, model.CargoPersonalDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, cargoPersonalViewModel);
      }
      private void cargarDataParaEditar(CargoPersonalViewModel model, int id)
      {

         CargoPersonalDTO cargoPersonalDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            cargoPersonalDTO = new CargoPersonalDTO();
         }
         else
         {
            cargoPersonalDTO = cargoPersonalProxy.GetById(new CargoPersonalDTO() { CargoPersonalId = id });
         }

         model.CargoPersonalDTO = cargoPersonalDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] CargoPersonalViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CargoPersonalDTO cargoPersonalDTO = cargarCargoPersonalDTO(model);

            int newId = model.CargoPersonalDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = cargoPersonalProxy.Insert(cargoPersonalDTO);
                  messageResourceKey = "RH_cargo_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  cargoPersonalProxy.Update(cargoPersonalDTO);
                  messageResourceKey = "RH_cargo_actualizado";
                  break;
            }

            cargoPersonalDTO = cargoPersonalProxy.GetById(new CargoPersonalDTO() { CargoPersonalId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, cargoPersonalDTO.Nombre);
            resultResponse.Data = new
            {
               cargoPersonalDTO.Id,
               cargoPersonalDTO.Codigo
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
      private CargoPersonalDTO cargarCargoPersonalDTO(CargoPersonalViewModel model)
      {

         CargoPersonalDTO cargoPersonalDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               cargoPersonalDTO = model.CargoPersonalDTO;
            }
            else
            {
               cargoPersonalDTO = cargoPersonalProxy.GetById(new CargoPersonalDTO()
               { CargoPersonalId = model.CargoPersonalDTO.Id });

               cargoPersonalDTO = model.CargoPersonalDTO;
            }

            cargoPersonalDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(cargoPersonalDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return cargoPersonalDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CargoPersonalDTO cargoPersonalDTO = cargoPersonalProxy
               .GetById(new CargoPersonalDTO() { CargoPersonalId = id });

            AuditDTO(cargoPersonalDTO);

            cargoPersonalProxy.Delete(cargoPersonalDTO);

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
      [Route("[action]")]
      public IActionResult ListPopup()
      {
         return PartialView();
      }

   }
}
