using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
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
   public class RegPensionarioController : AuthorizedController
   {
      private readonly IRegimenPensionarioProxy regimenPensionarioProxy;
      private readonly IRegimenPensionarioEsquemaProxy regimenPensionarioEsquemaProxy;
      private readonly IRegimenPensionarioComisionProxy regimenPensionarioComisionProxy;
      private readonly ITipoEsquemaPensionProxy tipoEsquemaPensionProxy;
      private readonly ITipoAporteComisionProxy tipoAporteComisionProxy;
      public RegPensionarioController(
         IRegimenPensionarioProxy regimenPensionarioProxy,
         IRegimenPensionarioEsquemaProxy regimenPensionarioEsquemaProxy,
         IRegimenPensionarioComisionProxy regimenPensionarioComisionProxy,
         ITipoEsquemaPensionProxy tipoEsquemaPensionProxy,
         ITipoAporteComisionProxy tipoAporteComisionProxy
         )
      {
         this.regimenPensionarioProxy = regimenPensionarioProxy;
         this.regimenPensionarioEsquemaProxy = regimenPensionarioEsquemaProxy;
         this.regimenPensionarioComisionProxy = regimenPensionarioComisionProxy;
         this.tipoEsquemaPensionProxy = tipoEsquemaPensionProxy;
         this.tipoAporteComisionProxy = tipoAporteComisionProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         RegPensionarioViewModel regPensionarioViewModel = new();

         return PartialView(regPensionarioViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         RegPensionarioViewModel model = JsonSerializer.Deserialize<RegPensionarioViewModel>(jsonModel);

         model.RegimenPensionarioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.RegimenPensionarioDTO.OrderBy = "rp.reg_pension";

         try
         {
            List<RegimenPensionarioDTO> regimenPensionarioDTOList = regimenPensionarioProxy.Search(model.RegimenPensionarioDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = regimenPensionarioDTOList.Count,
               rows = from f in regimenPensionarioDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.RegimenPensionarioId,
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
      public ActionResult Crear([FromBody] RegPensionarioViewModel model)
      {
         Logger.InfoStart();

         RegPensionarioViewModel regPensionarioViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(regPensionarioViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, regPensionarioViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] RegPensionarioViewModel model)
      {

         Logger.InfoStart();

         RegPensionarioViewModel regPensionarioViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(regPensionarioViewModel, model.RegimenPensionarioDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, regPensionarioViewModel);
      }
      private void cargarDataParaEditar(RegPensionarioViewModel model, int id)
      {

         RegimenPensionarioDTO regimenPensionarioDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            regimenPensionarioDTO = new RegimenPensionarioDTO();
         }
         else
         {
            regimenPensionarioDTO = regimenPensionarioProxy.GetById(new RegimenPensionarioDTO() { RegimenPensionarioId = id });
         }

         model.RegimenPensionarioDTO = regimenPensionarioDTO;

         cargarListas(model);

         List<TipoEsquemaPensionDTO> tipoEsquemaPensionDTOList = tipoEsquemaPensionProxy.
            Search(new TipoEsquemaPensionDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoEsquemaPensionString = string.Join(SeparadorConstante.PUNTO_COMA, tipoEsquemaPensionDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

         List<TipoAporteComisionDTO> tipoAporteComisionDTOList = tipoAporteComisionProxy.
            Search(new TipoAporteComisionDTO() { Estado = EstadoConstante.ACTIVO });
         model.TipoAporteComisionString = string.Join(SeparadorConstante.PUNTO_COMA, tipoAporteComisionDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

      }
      private void cargarListas(RegPensionarioViewModel model)
      {

         List<RegimenPensionarioEsquemaDTO> regimenPensionarioEsquemaDTOList = 
            obtenerListaRegimenPensionarioEsquema(model.RegimenPensionarioDTO.Id);

         model.RegimenPensionarioEsquemaList = WebHelper.ToSelectListItem(regimenPensionarioEsquemaDTOList, x => x.Id.ToString(), 
            x => x.TipoEsquemaPensionDTO.Nombre, SelectListFirstOptionType.Select);
      }
      private List<RegimenPensionarioEsquemaDTO> obtenerListaRegimenPensionarioEsquema(int regimenPensionarioId)
      {
         List<RegimenPensionarioEsquemaDTO> regimenPensionarioEsquemaDTOList = regimenPensionarioEsquemaProxy.Search(
            new RegimenPensionarioEsquemaDTO() { RegimenPensionarioId = regimenPensionarioId });

         return regimenPensionarioEsquemaDTOList;
      }
      [HttpGet("ListarEsquema")]
      public JsonResult ListarEsquema(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            List<RegimenPensionarioEsquemaDTO> regimenPensionarioEsquemaDTOList = obtenerListaRegimenPensionarioEsquema(id);

            resultResponse.Data = WebHelper.ToSelectListItem(regimenPensionarioEsquemaDTOList, x => x.Id.ToString(),
            x => x.TipoEsquemaPensionDTO.Nombre, SelectListFirstOptionType.Select);
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] RegPensionarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RegimenPensionarioDTO regimenPensionarioDTO = cargarRegimenPensionarioDTO(model);

            int newId = model.RegimenPensionarioDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = regimenPensionarioProxy.Insert(regimenPensionarioDTO);
                  messageResourceKey = "RH_regimen_pensionario_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  regimenPensionarioProxy.Update(regimenPensionarioDTO);
                  messageResourceKey = "RH_regimen_pensionario_actualizado";
                  break;
            }

            regimenPensionarioDTO = regimenPensionarioProxy.GetById(new RegimenPensionarioDTO() { RegimenPensionarioId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, regimenPensionarioDTO.Nombre);
            resultResponse.Data = new
            {
               regimenPensionarioDTO.Id
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
      private RegimenPensionarioDTO cargarRegimenPensionarioDTO(RegPensionarioViewModel model)
      {

         try
         {

            model.RegimenPensionarioDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(model.RegimenPensionarioDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return model.RegimenPensionarioDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RegimenPensionarioDTO regimenPensionarioDTO = regimenPensionarioProxy.GetById(new RegimenPensionarioDTO() { RegimenPensionarioId = id });

            AuditDTO(regimenPensionarioDTO);

            regimenPensionarioProxy.Delete(regimenPensionarioDTO);

            resultResponse.Message = SR.GetString("RH_regimen_pensionario_anulado", regimenPensionarioDTO.Nombre);
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
      [HttpPost("ListarRegimenPensionarioEsquema")]
      public ActionResult ListarRegimenPensionarioEsquema(string jsonModel)
      {

         Logger.InfoStart();

         RegPensionarioViewModel model = JsonSerializer.Deserialize<RegPensionarioViewModel>(jsonModel);

         model.RegimenPensionarioEsquemaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();

         try
         {
            List<RegimenPensionarioEsquemaDTO> regimenPensionarioEsquemaDTOList =
               regimenPensionarioEsquemaProxy.Search(model.RegimenPensionarioEsquemaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = regimenPensionarioEsquemaDTOList.Count,
               rows = from f in regimenPensionarioEsquemaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.RegimenPensionarioEsquemaId,
                            f.RegimenPensionarioId,
                            f.TipoEsquemaPensionId,
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
      [HttpPost("GrabarRegimenPensionarioEsquema")]
      public ActionResult GrabarRegimenPensionarioEsquema([FromBody] RegPensionarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RegimenPensionarioEsquemaDTO regimenPensionarioEsquemaDTO = cargarRegimenPensionarioEsquemaDTO(model);

            int newId = model.RegimenPensionarioEsquemaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (regimenPensionarioEsquemaDTO.DTOState)
            {
               case DTOState.Added:
                  newId = regimenPensionarioEsquemaProxy.Insert(regimenPensionarioEsquemaDTO);
                  messageResourceKey = "RH_tipo_esquema_pension_agregado";
                  break;

               case DTOState.Modified:
                  regimenPensionarioEsquemaProxy.Update(regimenPensionarioEsquemaDTO);
                  messageResourceKey = "RH_tipo_esquema_pension_actualizado";
                  break;
            }

            regimenPensionarioEsquemaDTO = regimenPensionarioEsquemaProxy.
               GetById(new RegimenPensionarioEsquemaDTO() { RegimenPensionarioEsquemaId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, regimenPensionarioEsquemaDTO.TipoEsquemaPensionDTO.Nombre);
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
      private RegimenPensionarioEsquemaDTO cargarRegimenPensionarioEsquemaDTO(RegPensionarioViewModel model)
      {

         RegimenPensionarioEsquemaDTO regimenPensionarioEsquemaDTO;
         try
         {

            regimenPensionarioEsquemaDTO = model.RegimenPensionarioEsquemaDTO;

            regimenPensionarioEsquemaDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(regimenPensionarioEsquemaDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return regimenPensionarioEsquemaDTO;
      }
      [HttpPost("EliminarRegimenPensionarioEsquema")]
      public ActionResult EliminarRegimenPensionarioEsquema([FromBody] RegPensionarioViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.RegimenPensionarioEsquemaDTO.DTOState = DTOState.Modified;
            AuditDTO(model.RegimenPensionarioEsquemaDTO);

            regimenPensionarioEsquemaProxy.Delete(model.RegimenPensionarioEsquemaDTO);

            resultResponse.Message = SR.GetString("RH_tipo_esquema_pension_anulado");
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
      [HttpPost("ListarRegimenPensionarioComision")]
      public ActionResult ListarRegimenPensionarioComision(string jsonModel)
      {

         Logger.InfoStart();

         RegPensionarioViewModel model = JsonSerializer.Deserialize<RegPensionarioViewModel>(jsonModel);

         model.RegimenPensionarioComisionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.RegimenPensionarioComisionDTO.OrderBy = "rpc.fecha_alta, tac.tipo_apo_comi";

         try
         {
            List<RegimenPensionarioComisionDTO> regimenPensionarioComisionDTOList =
               regimenPensionarioComisionProxy.Search(model.RegimenPensionarioComisionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = regimenPensionarioComisionDTOList.Count,
               rows = from f in regimenPensionarioComisionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.RegimenPensionarioComisionId,
                            f.RegimenPensionarioEsquemaId,
                            f.TipoAporteComisionId,
                            f.Porcentaje,
                            FechaAlta = f.FechaAlta.ToShortDateString(),
                            FechaBaja = f.FechaBaja == null ? string.Empty : f.FechaBaja.Value.ToShortDateString(),
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
      [HttpPost("GrabarRegimenPensionarioComision")]
      public ActionResult GrabarRegimenPensionarioComision([FromBody] RegPensionarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RegimenPensionarioComisionDTO regimenPensionarioComisionDTO = cargarRegimenPensionarioComisionDTO(model);

            int newId = model.RegimenPensionarioComisionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (regimenPensionarioComisionDTO.DTOState)
            {
               case DTOState.Added:
                  newId = regimenPensionarioComisionProxy.Insert(regimenPensionarioComisionDTO);
                  messageResourceKey = "RH_regimen_pensionario_comision_agregado";
                  break;

               case DTOState.Modified:
                  regimenPensionarioComisionProxy.Update(regimenPensionarioComisionDTO);
                  messageResourceKey = "RH_regimen_pensionario_comision_actualizado";
                  break;
            }

            regimenPensionarioComisionDTO = regimenPensionarioComisionProxy.
               GetById(new RegimenPensionarioComisionDTO() { RegimenPensionarioComisionId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, regimenPensionarioComisionDTO.TipoAporteComisionDTO.Nombre);
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
      private RegimenPensionarioComisionDTO cargarRegimenPensionarioComisionDTO(RegPensionarioViewModel model)
      {

         RegimenPensionarioComisionDTO regimenPensionarioComisionDTO;
         try
         {

            regimenPensionarioComisionDTO = model.RegimenPensionarioComisionDTO;

            regimenPensionarioComisionDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(regimenPensionarioComisionDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return regimenPensionarioComisionDTO;
      }
   }
}
