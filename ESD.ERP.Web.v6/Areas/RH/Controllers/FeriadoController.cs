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
   public class FeriadoController : AuthorizedController
   {
      private readonly IFeriadoProxy feriadoProxy;
      public FeriadoController(
         IFeriadoProxy feriadoProxy
         )
      {
         this.feriadoProxy = feriadoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         FeriadoViewModel feriadoViewModel = new();

         return PartialView(feriadoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         FeriadoViewModel model = new()
         {
            FeriadoDTO = new FeriadoDTO()
         };

         model.FeriadoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.FeriadoDTO.OrderBy = "f.fecha desc";

         try
         {
            List<FeriadoDTO> feriadoDTOList = feriadoProxy.Search(model.FeriadoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = feriadoDTOList.Count,
               rows = from f in feriadoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.FeriadoId,
                            Fecha = f.Fecha.ToShortDateString(),
                            EstadoNombre = f.Estado == EstadoConstante.ACTIVO ? EstadoNombreConstante.ACTIVO : EstadoNombreConstante.INACTIVO,
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
      public ActionResult Crear([FromBody] FeriadoViewModel model)
      {
         Logger.InfoStart();

         FeriadoViewModel feriadoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(feriadoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, feriadoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] FeriadoViewModel model)
      {

         Logger.InfoStart();

         FeriadoViewModel feriadoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(feriadoViewModel, model.FeriadoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, feriadoViewModel);
      }
      private void cargarDataParaEditar(FeriadoViewModel model, int id)
      {

         FeriadoDTO feriadoDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            feriadoDTO = new FeriadoDTO() {
               Fecha = DateTime.Today,
               Estado = EstadoConstante.ACTIVO
            };
         }
         else
         {
            feriadoDTO = feriadoProxy.GetById(new FeriadoDTO() { FeriadoId = id });
         }

         model.FeriadoDTO = feriadoDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] FeriadoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            FeriadoDTO feriadoDTO = cargarFeriadoDTO(model);

            int newId = model.FeriadoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = feriadoProxy.Insert(feriadoDTO);
                  messageResourceKey = "RH_feriado_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  feriadoProxy.Update(feriadoDTO);
                  messageResourceKey = "RH_feriado_actualizado";
                  break;
            }

            feriadoDTO = feriadoProxy.GetById(new FeriadoDTO() { FeriadoId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey);
            resultResponse.Data = new
            {
               feriadoDTO.Id
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
      private FeriadoDTO cargarFeriadoDTO(FeriadoViewModel model)
      {

         FeriadoDTO feriadoDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               feriadoDTO = model.FeriadoDTO;
            }
            else
            {
               feriadoDTO = feriadoProxy.GetById(new FeriadoDTO() 
                  { FeriadoId = model.FeriadoDTO.Id });

               feriadoDTO = model.FeriadoDTO;
            }

            AuditDTO(feriadoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return feriadoDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            FeriadoDTO feriadoDTO = feriadoProxy
               .GetById(new FeriadoDTO() { FeriadoId = id });

            AuditDTO(feriadoDTO);

            feriadoProxy.Delete(feriadoDTO);

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
