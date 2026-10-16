using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
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
   public class CajaController : AuthorizedController
   {
      private readonly ICajaProxy cajaProxy;
      private readonly ICajaPersonalProxy cajaPersonalProxy;

      public CajaController(
         ICajaProxy cajaProxy,
         ICajaPersonalProxy cajaPersonalProxy
         )
      {
         this.cajaProxy = cajaProxy;
         this.cajaPersonalProxy = cajaPersonalProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         CajaViewModel cajaViewModel = new();

         return PartialView(cajaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         CajaViewModel model = JsonSerializer.Deserialize<CajaViewModel>(jsonModel);

         model.CajaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CajaDTO.OrderBy = "c.nombre";

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         model.CajaDTO.EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId;

         try
         {
            List<CajaDTO> cajaDTOList = cajaProxy.Search(model.CajaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = cajaDTOList.Count,
               rows = from f in cajaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.CajaId,
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
      public ActionResult Crear([FromBody] CajaViewModel model)
      {
         Logger.InfoStart();

         CajaViewModel cajaViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(cajaViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, cajaViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] CajaViewModel model)
      {

         Logger.InfoStart();

         CajaViewModel cajaViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(cajaViewModel, model.CajaDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, cajaViewModel);
      }
      private void cargarDataParaEditar(CajaViewModel model, int id)
      {

         CajaDTO cajaDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            cajaDTO = new CajaDTO();
         }
         else
         {
            cajaDTO = cajaProxy.GetById(new CajaDTO() { CajaId = id });
         }

         model.CajaDTO = cajaDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] CajaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CajaDTO cajaDTO = cargarCajaDTO(model);

            int newId = model.CajaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = cajaProxy.Insert(cajaDTO);
                  messageResourceKey = "VT_caja_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  cajaProxy.Update(cajaDTO);
                  messageResourceKey = "VT_caja_actualizada";
                  break;
            }

            cajaDTO = cajaProxy.GetById(new CajaDTO() { CajaId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, cajaDTO.Nombre);
            resultResponse.Data = new
            {
               cajaDTO.Id
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
      private CajaDTO cargarCajaDTO(CajaViewModel model)
      {

         CajaDTO cajaDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               cajaDTO = model.CajaDTO;

               PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
               cajaDTO.EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId;
            }
            else
            {
               cajaDTO = cajaProxy.GetById(new CajaDTO() { CajaId = model.CajaDTO.Id });
               cajaDTO = model.CajaDTO;
            }

            cajaDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(cajaDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return cajaDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CajaDTO cajaDTO = cajaProxy.GetById(new CajaDTO() { CajaId = id });

            AuditDTO(cajaDTO);

            cajaProxy.Delete(cajaDTO);

            resultResponse.Message = SR.GetString("VT_caja_anulada", cajaDTO.Nombre);
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
      [HttpPost("ListarCajaPersonal")]
      public ActionResult ListarCajaPersonal(string jsonModel)
      {

         Logger.InfoStart();

         CajaViewModel model = JsonSerializer.Deserialize<CajaViewModel>(jsonModel);

         model.CajaPersonalDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CajaPersonalDTO.OrderBy = "prv.razon_social";

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         model.CajaPersonalDTO.CajaDTO = new CajaDTO() { 
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId
         };

         try
         {
            List<CajaPersonalDTO> cajaPersonalDTOList = cajaPersonalProxy.Search(model.CajaPersonalDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = cajaPersonalDTOList.Count,
               rows = from f in cajaPersonalDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.CajaPersonalId,
                            f.CajaId,
                            f.PersonalId,
                            f.PersonalDTO.ProveedorDTO.RazonSocial,
                            f.Estado,
                            DTOState = DTOState.Unchanged
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
      [HttpPost("GrabarCajaPersonal")]
      public ActionResult GrabarCajaPersonal([FromBody] CajaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CajaPersonalDTO cajaPersonalDTO = cargarCajaPersonalDTO(model);

            string messageResourceKey = "VT_caja_personal_agregado";

            cajaPersonalProxy.Insert(cajaPersonalDTO);

            resultResponse.Message = SR.GetString(messageResourceKey);
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
      private CajaPersonalDTO cargarCajaPersonalDTO(CajaViewModel model)
      {

         CajaPersonalDTO cajaPersonalDTO;
         try
         {

            cajaPersonalDTO = model.CajaPersonalDTO;
            cajaPersonalDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(cajaPersonalDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return cajaPersonalDTO;
      }
      [HttpPost("EliminarCajaPersonal")]
      public ActionResult EliminarCajaPersonal([FromBody] CajaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.CajaPersonalDTO.DTOState = DTOState.Modified;
            AuditDTO(model.CajaPersonalDTO);

            cajaPersonalProxy.Delete(model.CajaPersonalDTO);

            resultResponse.Message = SR.GetString("VT_caja_personal_eliminado");
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
