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
   public class RemMinVitalController : AuthorizedController
   {
      private readonly IRemuneracionMinimaVitalProxy remuneracionMinimaVitalProxy;
      public RemMinVitalController(
         IRemuneracionMinimaVitalProxy remuneracionMinimaVitalProxy
         )
      {
         this.remuneracionMinimaVitalProxy = remuneracionMinimaVitalProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         RemMinVitalViewModel remMinVitalViewModel = new();

         return PartialView(remMinVitalViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         RemMinVitalViewModel model = new()
         {
            RemuneracionMinimaVitalDTO = new RemuneracionMinimaVitalDTO()
         };

         model.RemuneracionMinimaVitalDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.RemuneracionMinimaVitalDTO.OrderBy = "rmv.fecha_alta desc";

         try
         {
            List<RemuneracionMinimaVitalDTO> remuneracionMinimaVitalDTOList = remuneracionMinimaVitalProxy.Search(model.RemuneracionMinimaVitalDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = remuneracionMinimaVitalDTOList.Count,
               rows = from f in remuneracionMinimaVitalDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.RemuneracionMinimaVitalId,
                            f.Monto,
                            FechaAlta = f.FechaAlta.ToShortDateString(),
                            FechaBaja = f.FechaBaja == null ? string.Empty : f.FechaBaja.Value.ToShortDateString(),
                            Vigente = f.FechaBaja == null ? SiNoConstante.S : SiNoConstante.N,
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
      public ActionResult Crear([FromBody] RemMinVitalViewModel model)
      {
         Logger.InfoStart();

         RemMinVitalViewModel remMinVitalViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(remMinVitalViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, remMinVitalViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] RemMinVitalViewModel model)
      {

         Logger.InfoStart();

         RemMinVitalViewModel remMinVitalViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(remMinVitalViewModel, model.RemuneracionMinimaVitalDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, remMinVitalViewModel);
      }
      private void cargarDataParaEditar(RemMinVitalViewModel model, int id)
      {

         RemuneracionMinimaVitalDTO remuneracionMinimaVitalDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            remuneracionMinimaVitalDTO = new RemuneracionMinimaVitalDTO
            {
               FechaAlta = DateTime.Today
            };
         }
         else
         {
            remuneracionMinimaVitalDTO = remuneracionMinimaVitalProxy.GetById(new RemuneracionMinimaVitalDTO() { RemuneracionMinimaVitalId = id });
         }

         model.RemuneracionMinimaVitalDTO = remuneracionMinimaVitalDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] RemMinVitalViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RemuneracionMinimaVitalDTO remuneracionMinimaVitalDTO = cargarRemuneracionMinimaVitalDTO(model);

            int newId = model.RemuneracionMinimaVitalDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = remuneracionMinimaVitalProxy.Insert(remuneracionMinimaVitalDTO);
                  messageResourceKey = "MG_rmv_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  remuneracionMinimaVitalProxy.Update(remuneracionMinimaVitalDTO);
                  messageResourceKey = "MG_rmv_actualizada";
                  break;
            }

            remuneracionMinimaVitalDTO = remuneracionMinimaVitalProxy.GetById(new RemuneracionMinimaVitalDTO() { RemuneracionMinimaVitalId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey);
            resultResponse.Data = new
            {
               remuneracionMinimaVitalDTO.Id
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
      private RemuneracionMinimaVitalDTO cargarRemuneracionMinimaVitalDTO(RemMinVitalViewModel model)
      {

         RemuneracionMinimaVitalDTO remuneracionMinimaVitalDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               remuneracionMinimaVitalDTO = model.RemuneracionMinimaVitalDTO;
            }
            else
            {
               remuneracionMinimaVitalDTO = remuneracionMinimaVitalProxy.GetById(new RemuneracionMinimaVitalDTO() 
                  { RemuneracionMinimaVitalId = model.RemuneracionMinimaVitalDTO.Id });

               remuneracionMinimaVitalDTO = model.RemuneracionMinimaVitalDTO;
            }

            remuneracionMinimaVitalDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(remuneracionMinimaVitalDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return remuneracionMinimaVitalDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RemuneracionMinimaVitalDTO remuneracionMinimaVitalDTO = remuneracionMinimaVitalProxy
               .GetById(new RemuneracionMinimaVitalDTO() { RemuneracionMinimaVitalId = id });

            AuditDTO(remuneracionMinimaVitalDTO);

            remuneracionMinimaVitalProxy.Delete(remuneracionMinimaVitalDTO);

            resultResponse.Message = SR.GetString("MG_rmv_anulada");
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
