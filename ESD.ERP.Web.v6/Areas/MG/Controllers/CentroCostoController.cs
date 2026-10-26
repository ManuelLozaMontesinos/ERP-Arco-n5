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
   public class CentroCostoController : AuthorizedController
   {
      private readonly ICentroCostoProxy centroCostoProxy;
      public CentroCostoController(
         ICentroCostoProxy centroCostoProxy
         )
      {
         this.centroCostoProxy = centroCostoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         CentroCostoViewModel centroCostoViewModel = new();

         return PartialView(centroCostoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         CentroCostoViewModel model = new()
         {
            CentroCostoDTO = new CentroCostoDTO()
         };

         int empresaId = ObtenerEmpresaId();
         if (empresaId <= 0)
         {
            empresaId = 1;
         }

         model.CentroCostoDTO.EmpresaId = empresaId;
         model.CentroCostoDTO.Estado = EstadoConstante.ACTIVO;
         model.CentroCostoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CentroCostoDTO.OrderBy = "nombre";

         try
         {
            List<CentroCostoDTO> centroCostoDTOList = centroCostoProxy.Search(model.CentroCostoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = centroCostoDTOList.Count,
               rows = from f in centroCostoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.CentroCostoId,
                            f.Nombre,
                            f.Sigla,
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
      public ActionResult Crear([FromBody] CentroCostoViewModel model)
      {
         Logger.InfoStart();

         CentroCostoViewModel centroCostoViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(centroCostoViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, centroCostoViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] CentroCostoViewModel model)
      {

         Logger.InfoStart();

         CentroCostoViewModel centroCostoViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(centroCostoViewModel, model.CentroCostoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, centroCostoViewModel);
      }
      private void cargarDataParaEditar(CentroCostoViewModel model, int id)
      {

         CentroCostoDTO centroCostoDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            centroCostoDTO = new CentroCostoDTO();
         }
         else
         {
            centroCostoDTO = centroCostoProxy.GetById(new CentroCostoDTO() { CentroCostoId = id });
         }

         model.CentroCostoDTO = centroCostoDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] CentroCostoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CentroCostoDTO centroCostoDTO = cargarCentroCostoDTO(model);

            int newId = model.CentroCostoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = centroCostoProxy.Insert(centroCostoDTO);
                  messageResourceKey = "MG_centro_costo_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  centroCostoProxy.Update(centroCostoDTO);
                  messageResourceKey = "MG_centro_costo_actualizado";
                  break;
            }

            centroCostoDTO = centroCostoProxy.GetById(new CentroCostoDTO() { CentroCostoId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, centroCostoDTO.Nombre);
            resultResponse.Data = new
            {
               centroCostoDTO.Id
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
      private CentroCostoDTO cargarCentroCostoDTO(CentroCostoViewModel model)
      {

         CentroCostoDTO centroCostoDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               centroCostoDTO = model.CentroCostoDTO;
               int empId = ObtenerEmpresaId();
               centroCostoDTO.EmpresaId = empId > 0 ? empId : 1;
            }
            else
            {
               centroCostoDTO = centroCostoProxy.GetById(new CentroCostoDTO() 
                  { CentroCostoId = model.CentroCostoDTO.Id });

               centroCostoDTO.Nombre = model.CentroCostoDTO.Nombre;
               centroCostoDTO.Sigla = model.CentroCostoDTO.Sigla;
            }

            if (string.IsNullOrEmpty(model.CentroCostoDTO.Sigla))
               model.CentroCostoDTO.Sigla = string.Empty;

            centroCostoDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(centroCostoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return centroCostoDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CentroCostoDTO centroCostoDTO = centroCostoProxy
               .GetById(new CentroCostoDTO() { CentroCostoId = id });

            AuditDTO(centroCostoDTO);

            centroCostoProxy.Delete(centroCostoDTO);

            resultResponse.Message = SR.GetString("MG_centro_costo_anulado");
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
