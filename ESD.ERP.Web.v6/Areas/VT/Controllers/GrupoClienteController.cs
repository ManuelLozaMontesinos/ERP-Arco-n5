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
   public class GrupoClienteController : AuthorizedController
   {
      private readonly IGrupoProveedorProxy grupoProveedorProxy;
      private readonly IGrupoProveedorItemProxy grupoProveedorItemProxy;
      public GrupoClienteController(
         IGrupoProveedorProxy grupoProveedorProxy,
         IGrupoProveedorItemProxy grupoProveedorItemProxy
         )
      {
         this.grupoProveedorProxy = grupoProveedorProxy;
         this.grupoProveedorItemProxy = grupoProveedorItemProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         GrupoClienteViewModel grupoClienteViewModel = new();

         return PartialView(grupoClienteViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         GrupoClienteViewModel model = JsonSerializer.Deserialize<GrupoClienteViewModel>(jsonModel);
         model.GrupoProveedorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.GrupoProveedorDTO.OrderBy = "nombre";

         model.GrupoProveedorDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

         try
         {
            List<GrupoProveedorDTO> grupoProveedorDTOList = grupoProveedorProxy.Search(model.GrupoProveedorDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = grupoProveedorDTOList.Count,
               rows = from f in grupoProveedorDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.GrupoProveedorId,
                            f.Nombre,
                            f.IgnorarIncrementoFraccion,
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
      public ActionResult Crear([FromBody] GrupoClienteViewModel model)
      {
         Logger.InfoStart();

         GrupoClienteViewModel grupoClienteViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(grupoClienteViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, grupoClienteViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] GrupoClienteViewModel model)
      {

         Logger.InfoStart();

         GrupoClienteViewModel grupoClienteViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(grupoClienteViewModel, model.GrupoProveedorDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, grupoClienteViewModel);
      }
      private void cargarDataParaEditar(GrupoClienteViewModel model, int id)
      {

         GrupoProveedorDTO grupoProveedorDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            grupoProveedorDTO = new GrupoProveedorDTO();
         }
         else
         {
            grupoProveedorDTO = grupoProveedorProxy.GetById(new GrupoProveedorDTO() { GrupoProveedorId = id });
         }

         grupoProveedorDTO.IgnorarIncrementoFraccion = grupoProveedorDTO.IgnorarIncrementoFraccion == SiNoConstante.N ? EstadoConstante.INACTIVO : EstadoConstante.ACTIVO;

         model.GrupoProveedorDTO = grupoProveedorDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] GrupoClienteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            GrupoProveedorDTO grupoProveedorDTO = cargarGrupoProveedorDTO(model);

            int newId = model.GrupoProveedorDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = grupoProveedorProxy.Insert(grupoProveedorDTO);
                  messageResourceKey = "VT_grupo_cliente_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  grupoProveedorProxy.Update(grupoProveedorDTO);
                  messageResourceKey = "VT_grupo_cliente_actualizado";
                  break;
            }

            grupoProveedorDTO = grupoProveedorProxy.GetById(new GrupoProveedorDTO() { GrupoProveedorId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, grupoProveedorDTO.Nombre);
            resultResponse.Data = new
            {
               grupoProveedorDTO.Id
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
      private GrupoProveedorDTO cargarGrupoProveedorDTO(GrupoClienteViewModel model)
      {

         GrupoProveedorDTO grupoProveedorDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               grupoProveedorDTO = model.GrupoProveedorDTO;
               grupoProveedorDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
            }
            else
            {
               grupoProveedorDTO = grupoProveedorProxy.GetById(new GrupoProveedorDTO() 
                  { GrupoProveedorId = model.GrupoProveedorDTO.Id });

               grupoProveedorDTO = model.GrupoProveedorDTO;
            }

            //if (string.IsNullOrEmpty(model.GrupoProveedorDTO.Sigla))
            //   model.GrupoProveedorDTO.Sigla = string.Empty;

            grupoProveedorDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(grupoProveedorDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return grupoProveedorDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            GrupoProveedorDTO grupoProveedorDTO = grupoProveedorProxy
               .GetById(new GrupoProveedorDTO() { GrupoProveedorId = id });

            AuditDTO(grupoProveedorDTO);

            grupoProveedorProxy.Delete(grupoProveedorDTO);

            resultResponse.Message = SR.GetString("VT_grupo_cliente_anulado");
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
      [HttpPost("ListarGrupoProveedorItem")]
      public ActionResult ListarGrupoProveedorItem(string jsonModel)
      {

         Logger.InfoStart();

         GrupoClienteViewModel model = JsonSerializer.Deserialize<GrupoClienteViewModel>(jsonModel);

         model.GrupoProveedorItemDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.GrupoProveedorItemDTO.OrderBy = "p.razon_social";

         if (model.GrupoProveedorItemDTO.GrupoProveedorId == 0)
            model.GrupoProveedorItemDTO.GrupoProveedorId = -1;

         try
         {
            List<GrupoProveedorItemDTO> grupoProveedorItemDTOList = grupoProveedorItemProxy.Search(model.GrupoProveedorItemDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = grupoProveedorItemDTOList.Count,
               rows = from f in grupoProveedorItemDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.GrupoProveedorItemId,
                            f.GrupoProveedorId,
                            f.ProveedorId,
                            f.ProveedorDTO.RazonSocial,
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
      [HttpPost("GrabarGrupoProveedorItem")]
      public ActionResult GrabarGrupoProveedorItem([FromBody] GrupoClienteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            GrupoProveedorItemDTO grupoProveedorItemDTO = cargarGrupoProveedorItemDTO(model);

            int newId = model.GrupoProveedorItemDTO.Id;
            string messageResourceKey = "VT_cliente_agregado";

            newId = grupoProveedorItemProxy.Insert(grupoProveedorItemDTO);

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
      private GrupoProveedorItemDTO cargarGrupoProveedorItemDTO(GrupoClienteViewModel model)
      {

         GrupoProveedorItemDTO grupoProveedorItemDTO;
         try
         {

            grupoProveedorItemDTO = model.GrupoProveedorItemDTO;
            grupoProveedorItemDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(grupoProveedorItemDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return grupoProveedorItemDTO;
      }
      [HttpPost("EliminarGrupoProveedorItem")]
      public ActionResult EliminarGrupoProveedorItem([FromBody] GrupoClienteViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.GrupoProveedorItemDTO.DTOState = DTOState.Modified;
            AuditDTO(model.GrupoProveedorItemDTO);

            grupoProveedorItemProxy.Delete(model.GrupoProveedorItemDTO);

            resultResponse.Message = SR.GetString("VT_cliente_eliminado");
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
      [Route("ListPopup")]
      public IActionResult ListPopup([FromBody] GrupoClienteViewModel model)
      {
         if (model == null)
            model = new GrupoClienteViewModel();

         model.ModalEditId = GetModalEditPanelId();
         return PartialView(model);
      }

   }
}
