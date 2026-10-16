using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class MedioPagoController : AuthorizedController
   {
      private readonly IMedioPagoProxy medioPagoProxy;

      public MedioPagoController(
         IMedioPagoProxy medioPagoProxy
         )
      {
         this.medioPagoProxy = medioPagoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         MedioPagoViewModel medioPagoViewModel = new();

         return PartialView(medioPagoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         MedioPagoViewModel model = JsonSerializer.Deserialize<MedioPagoViewModel>(jsonModel);

         model.MedioPagoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MedioPagoDTO.OrderBy = "mp.medio_pago";

         model.MedioPagoDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

         try
         {
            List<MedioPagoDTO> medioPagoDTOList = medioPagoProxy.Search(model.MedioPagoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = medioPagoDTOList.Count,
               rows = from f in medioPagoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.MedioPagoId,
                            f.Codigo,
                            f.Nombre,
                            f.PorcentajeTarjeta,
                            f.Abreviatura,
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
      public ActionResult Crear([FromBody] MedioPagoViewModel model)
      {
         Logger.InfoStart();

         MedioPagoViewModel medioPagoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(medioPagoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, medioPagoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] MedioPagoViewModel model)
      {

         Logger.InfoStart();

         MedioPagoViewModel medioPagoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(medioPagoViewModel, model.MedioPagoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, medioPagoViewModel);
      }
      private void cargarDataParaEditar(MedioPagoViewModel model, int id)
      {

         MedioPagoDTO medioPagoDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            medioPagoDTO = new MedioPagoDTO();
         }
         else
         {
            medioPagoDTO = medioPagoProxy.GetById(new MedioPagoDTO() { MedioPagoId = id });
         }

         model.MedioPagoDTO = medioPagoDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] MedioPagoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            MedioPagoDTO medioPagoDTO = cargarMedioPagoDTO(model);

            int newId = model.MedioPagoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = medioPagoProxy.Insert(medioPagoDTO);
                  messageResourceKey = "VT_medio_pago_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  medioPagoProxy.Update(medioPagoDTO);
                  messageResourceKey = "VT_medio_pago_actualizado";
                  break;
            }

            medioPagoDTO = medioPagoProxy.GetById(new MedioPagoDTO() { MedioPagoId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, medioPagoDTO.Nombre);
            resultResponse.Data = new
            {
               medioPagoDTO.Id
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
      private MedioPagoDTO cargarMedioPagoDTO(MedioPagoViewModel model)
      {

         MedioPagoDTO medioPagoDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               medioPagoDTO = model.MedioPagoDTO;
               medioPagoDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
            }
            else
            {
               medioPagoDTO = medioPagoProxy.GetById(new MedioPagoDTO() { MedioPagoId = model.MedioPagoDTO.Id });
               medioPagoDTO = model.MedioPagoDTO;
            }

            if (string.IsNullOrEmpty(model.MedioPagoDTO.Abreviatura))
               model.MedioPagoDTO.Abreviatura = string.Empty;

            medioPagoDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(medioPagoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return medioPagoDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            MedioPagoDTO medioPagoDTO = medioPagoProxy.GetById(new MedioPagoDTO() { MedioPagoId = id });

            AuditDTO(medioPagoDTO);

            medioPagoProxy.Delete(medioPagoDTO);

            resultResponse.Message = SR.GetString("VT_medio_pago_anulado", medioPagoDTO.Nombre);
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
