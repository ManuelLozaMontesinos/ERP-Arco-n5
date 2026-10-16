using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.AS.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.AS.Controllers
{
   [Area("AS")]
   [Route("[controller]")]
   public class PeriodoController : AuthorizedController
   {
      private readonly IPeriodoProxy periodoProxy;

      public PeriodoController(
         IPeriodoProxy periodoProxy
         )
      {
         this.periodoProxy = periodoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         PeriodoViewModel periodoViewModel = new();

         return PartialView(periodoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PeriodoViewModel model = JsonSerializer.Deserialize<PeriodoViewModel>(jsonModel);

         model.PeriodoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PeriodoDTO.OrderBy = "p.periodo";

         try
         {
            List<PeriodoDTO> periodoDTOList = periodoProxy.Search(model.PeriodoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = periodoDTOList.Count,
               rows = from f in periodoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PeriodoId,
                            f.Codigo,
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
      public ActionResult Crear([FromBody]PeriodoViewModel model)
      {
         Logger.InfoStart();

         PeriodoViewModel periodoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(periodoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, periodoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody]PeriodoViewModel model)
      {

         Logger.InfoStart();

         PeriodoViewModel periodoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(periodoViewModel, model.PeriodoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, periodoViewModel);
      }
      private void cargarDataParaEditar(PeriodoViewModel model, int id)
      {

         PeriodoDTO periodoDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            periodoDTO = new PeriodoDTO
            {
               Estado = EstadoConstante.ACTIVO,
               HabilitarProcesoBatch = SiNoConstante.S
            };
         }
         else
         {
            periodoDTO = periodoProxy.GetById(new PeriodoDTO() { PeriodoId = id });
         }

         periodoDTO.HabilitarProcesoBatch = periodoDTO.HabilitarProcesoBatch == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;

         model.PeriodoDTO = periodoDTO;

      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody]PeriodoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PeriodoDTO periodoDTO = cargarPeriodoDTO(model);

            int newId = model.PeriodoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = periodoProxy.Insert(periodoDTO);
                  messageResourceKey = "AS_periodo_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  periodoProxy.Update(periodoDTO);
                  messageResourceKey = "AS_periodo_actualizado";
                  break;
            }

            periodoDTO = periodoProxy.GetById(new PeriodoDTO() { PeriodoId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, periodoDTO.Codigo);
            resultResponse.Data = new
            {
               periodoDTO.Id
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
      private PeriodoDTO cargarPeriodoDTO(PeriodoViewModel model)
      {

         PeriodoDTO periodoDTO;
         try
         {

            periodoDTO = model.PeriodoDTO;

            periodoDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(periodoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return periodoDTO;
      }
   }
}