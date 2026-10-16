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
   public class CondPagoController : AuthorizedController
   {
      private readonly ICondicionPagoProxy condicionPagoProxy;
      private readonly ITipoCondicionPagoProxy tipoCondicionPagoProxy;
      public CondPagoController(
         ICondicionPagoProxy condicionPagoProxy,
         ITipoCondicionPagoProxy tipoCondicionPagoProxy
         )
      {
         this.condicionPagoProxy = condicionPagoProxy;
         this.tipoCondicionPagoProxy = tipoCondicionPagoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         CondPagoViewModel condPagoViewModel = new();

         return PartialView(condPagoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         CondPagoViewModel model = JsonSerializer.Deserialize<CondPagoViewModel>(jsonModel);

         model.CondicionPagoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CondicionPagoDTO.OrderBy = "cp.cond_pago";

         model.CondicionPagoDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

         try
         {
            List<CondicionPagoDTO> condicionPagoDTOList = condicionPagoProxy.Search(model.CondicionPagoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = condicionPagoDTOList.Count,
               rows = from f in condicionPagoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.CondicionPagoId,
                            f.Codigo,
                            f.Nombre,
                            TipoCondicionPago = f.TipoCondicionPagoDTO.Nombre,
                            f.Dia,
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
      public ActionResult Crear([FromBody] CondPagoViewModel model)
      {
         Logger.InfoStart();

         CondPagoViewModel condPagoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(condPagoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, condPagoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] CondPagoViewModel model)
      {

         Logger.InfoStart();

         CondPagoViewModel condPagoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(condPagoViewModel, model.CondicionPagoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, condPagoViewModel);
      }
      private void cargarDataParaEditar(CondPagoViewModel model, int id)
      {

         CondicionPagoDTO condicionPagoDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            condicionPagoDTO = new CondicionPagoDTO();
         }
         else
         {
            condicionPagoDTO = condicionPagoProxy.GetById(new CondicionPagoDTO() { CondicionPagoId = id });
         }

         model.CondicionPagoDTO = condicionPagoDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.TipoCondicionPagoList, condicionPagoDTO.TipoCondicionPagoId);
      }
      private void cargarListas(CondPagoViewModel model)
      {
         List<TipoCondicionPagoDTO> tipoCondicionPagoDTOList = tipoCondicionPagoProxy.Search(new TipoCondicionPagoDTO()
         {
            Estado = EstadoConstante.ACTIVO,
         });
         model.TipoCondicionPagoList = WebHelper.ToSelectListItem(tipoCondicionPagoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] CondPagoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CondicionPagoDTO condicionPagoDTO = cargarCondicionPagoDTO(model);

            int newId = model.CondicionPagoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = condicionPagoProxy.Insert(condicionPagoDTO);
                  messageResourceKey = "MG_condicion_pago_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  condicionPagoProxy.Update(condicionPagoDTO);
                  messageResourceKey = "MG_condicion_pago_actualizada";
                  break;
            }

            condicionPagoDTO = condicionPagoProxy.GetById(new CondicionPagoDTO() { CondicionPagoId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, condicionPagoDTO.Nombre);
            resultResponse.Data = new
            {
               condicionPagoDTO.Id,
               condicionPagoDTO.Codigo
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
      private CondicionPagoDTO cargarCondicionPagoDTO(CondPagoViewModel model)
      {

         CondicionPagoDTO condicionPagoDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               condicionPagoDTO = model.CondicionPagoDTO;
               condicionPagoDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
            }
            else
            {
               condicionPagoDTO = condicionPagoProxy.GetById(new CondicionPagoDTO() { CondicionPagoId = model.CondicionPagoDTO.Id });
               condicionPagoDTO = model.CondicionPagoDTO;
            }

            condicionPagoDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(condicionPagoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return condicionPagoDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CondicionPagoDTO condicionPagoDTO = condicionPagoProxy.GetById(new CondicionPagoDTO() { CondicionPagoId = id });

            AuditDTO(condicionPagoDTO);

            condicionPagoProxy.Delete(condicionPagoDTO);

            resultResponse.Message = SR.GetString("MG_condicion_pago_anulada", condicionPagoDTO.Nombre);
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
