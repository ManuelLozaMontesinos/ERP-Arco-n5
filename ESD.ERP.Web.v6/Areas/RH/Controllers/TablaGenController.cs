using ESD.Core.Logging;
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
   public class TablaGenController : AuthorizedController
   {
      private readonly IPaisProxy paisProxy;
      private readonly IPaisEmisorProxy paisEmisorProxy;
      private readonly IOcupacionProxy ocupacionProxy;
      public TablaGenController(
         IPaisProxy paisProxy,
         IPaisEmisorProxy paisEmisorProxy,
         IOcupacionProxy ocupacionProxy
         )
      {
         this.paisProxy = paisProxy;
         this.paisEmisorProxy = paisEmisorProxy;
         this.ocupacionProxy = ocupacionProxy;
      }

      [Route("[action]")]
      public IActionResult PaisListPopup()
      {
         return PartialView();
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarPaisPopup(string jsonModel)
      {

         Logger.InfoStart();

         TablaGenViewModel model = JsonSerializer.Deserialize<TablaGenViewModel>(jsonModel);

         model.PaisDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PaisDTO.OrderBy = "nombre";

         try
         {
            List<PaisDTO> paisDTOList = paisProxy.Search(model.PaisDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = paisDTOList.Count,
               rows = from f in paisDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PaisId,
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
      [Route("[action]")]
      public IActionResult PaisEmisorListPopup()
      {
         ViewBag.ModalEditId = GetModalEditPanelId();
         return PartialView();
      }
      [HttpPost("ListarPaisEmisorPopup")]
      public ActionResult ListarPaisEmisorPopup(string jsonModel)
      {

         Logger.InfoStart();

         TablaGenViewModel model = JsonSerializer.Deserialize<TablaGenViewModel>(jsonModel);

         model.PaisEmisorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PaisEmisorDTO.OrderBy = "nombre";

         try
         {
            List<PaisEmisorDTO> paisEmisorDTOList = paisEmisorProxy.Search(model.PaisEmisorDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = paisEmisorDTOList.Count,
               rows = from f in paisEmisorDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PaisEmisorId,
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
      [Route("[action]")]
      public IActionResult OcupacionListPopup([FromBody] TablaGenViewModel model)
      {
         ViewBag.ModalEditId = GetModalEditPanelId();
         return PartialView(model);
      }
      [HttpPost("ListarOcupacionPopup")]
      public ActionResult ListarOcupacionPopup(string jsonModel)
      {

         Logger.InfoStart();

         TablaGenViewModel model = JsonSerializer.Deserialize<TablaGenViewModel>(jsonModel);

         model.OcupacionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.OcupacionDTO.OrderBy = "nombre";

         try
         {
            List<OcupacionDTO> ocupacionDTOList = ocupacionProxy.Search(model.OcupacionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = ocupacionDTOList.Count,
               rows = from f in ocupacionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.OcupacionId,
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

   }
}
