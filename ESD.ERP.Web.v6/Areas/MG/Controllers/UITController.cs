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
   public class UITController : AuthorizedController
   {
      private readonly IUITProxy uitProxy;
      public UITController(
         IUITProxy uitProxy
         )
      {
         this.uitProxy = uitProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         UITViewModel remMinVitalViewModel = new();

         return PartialView(remMinVitalViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         UITViewModel model = new()
         {
            UITDTO = new UITDTO()
         };

         model.UITDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.UITDTO.OrderBy = "fecha_alta desc";

         try
         {
            List<UITDTO> uitDTOList = uitProxy.Search(model.UITDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = uitDTOList.Count,
               rows = from f in uitDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.UITId,
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
      public ActionResult Crear([FromBody] UITViewModel model)
      {
         Logger.InfoStart();

         UITViewModel remMinVitalViewModel = new()
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
      public ActionResult Editar([FromBody] UITViewModel model)
      {

         Logger.InfoStart();

         UITViewModel remMinVitalViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(remMinVitalViewModel, model.UITDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, remMinVitalViewModel);
      }
      private void cargarDataParaEditar(UITViewModel model, int id)
      {

         UITDTO uitDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            uitDTO = new UITDTO
            {
               FechaAlta = DateTime.Today
            };
         }
         else
         {
            uitDTO = uitProxy.GetById(new UITDTO() { UITId = id });
         }

         model.UITDTO = uitDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] UITViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            UITDTO uitDTO = cargarUITDTO(model);

            int newId = model.UITDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = uitProxy.Insert(uitDTO);
                  messageResourceKey = "MG_uit_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  uitProxy.Update(uitDTO);
                  messageResourceKey = "MG_uit_actualizada";
                  break;
            }

            uitDTO = uitProxy.GetById(new UITDTO() { UITId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey);
            resultResponse.Data = new
            {
               uitDTO.Id
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
      private UITDTO cargarUITDTO(UITViewModel model)
      {

         UITDTO uitDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               uitDTO = model.UITDTO;
            }
            else
            {
               uitDTO = uitProxy.GetById(new UITDTO() 
                  { UITId = model.UITDTO.Id });

               uitDTO = model.UITDTO;
            }

            uitDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(uitDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return uitDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            UITDTO uitDTO = uitProxy
               .GetById(new UITDTO() { UITId = id });

            AuditDTO(uitDTO);

            uitProxy.Delete(uitDTO);

            resultResponse.Message = SR.GetString("MG_uit_anulada");
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
