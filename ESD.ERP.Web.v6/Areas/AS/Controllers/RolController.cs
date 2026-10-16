using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
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
   public class RolController : AuthorizedController
   {
      private readonly IRolProxy rolProxy;
      private readonly IOrigenRegistroProxy origenRegistroProxy;
      private readonly IRolUsuarioProxy rolUsuarioProxy;
      private readonly IRolMenuProxy rolMenuProxy;
      public RolController(
         IRolProxy rolProxy,
         IOrigenRegistroProxy origenRegistroProxy,
         IRolUsuarioProxy rolUsuarioProxy,
         IRolMenuProxy rolMenuProxy
         )
      {
         this.rolProxy = rolProxy;
         this.rolUsuarioProxy = rolUsuarioProxy;
         this.origenRegistroProxy = origenRegistroProxy;
         this.rolMenuProxy = rolMenuProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         RolViewModel rolViewModel = new();

         return PartialView(rolViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         RolViewModel model = JsonSerializer.Deserialize<RolViewModel>(jsonModel);

         model.RolDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.RolDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
         model.RolDTO.OrderBy = "r.nombre";

         try
         {
            List<RolDTO> rolDTOList = rolProxy.Search(model.RolDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = rolDTOList.Count,
               rows = from f in rolDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.RolId,
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
      public ActionResult Crear([FromBody]RolViewModel model)
      {
         Logger.InfoStart();

         RolViewModel rolViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(rolViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, rolViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody]RolViewModel model)
      {

         Logger.InfoStart();

         RolViewModel rolViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(rolViewModel, model.RolDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, rolViewModel);
      }
      private void cargarDataParaEditar(RolViewModel model, int id)
      {

         RolDTO rolDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            rolDTO = new RolDTO
            {
               Estado = EstadoConstante.ACTIVO,
            };
         }
         else
         {
            rolDTO = rolProxy.GetById(new RolDTO() { RolId = id });
         }

         model.RolDTO = rolDTO;

      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody]RolViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RolDTO rolDTO = cargarRolDTO(model);

            int newId = model.RolDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = rolProxy.Insert(rolDTO);
                  messageResourceKey = "AS_rol_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  rolProxy.Update(rolDTO);
                  messageResourceKey = "AS_rol_actualizado";
                  break;
            }

            rolDTO = rolProxy.GetById(new RolDTO() { RolId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, rolDTO.Nombre);
            resultResponse.Data = new
            {
               rolDTO.Id
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
      private RolDTO cargarRolDTO(RolViewModel model)
      {

         RolDTO rolDTO;
         try
         {

            rolDTO = model.RolDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               rolDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

               OrigenRegistroDTO origenRegistroDTO = origenRegistroProxy.ObtenerPorCodigo(new OrigenRegistroDTO() { Codigo = OrigenRegistroConstante.USUARIO });
               rolDTO.OrigenRegistroId = origenRegistroDTO.Id;
            }
            int rowId = 1;
            foreach (RolMenuDTO rolMenuDTO in model.RolMenuDTOList) {
               rolMenuDTO.RolMenuId = rowId;
               rowId++;
            }

            rolDTO.RolMenuDTOList = model.RolMenuDTOList;

            rolDTO.Estado = EstadoConstante.ACTIVO;
            AuditDTO(rolDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return rolDTO;
      }
      [HttpPost("ListarUsuario")]
      public ActionResult ListarUsuario(string jsonModel)
      {

         Logger.InfoStart();

         RolViewModel model = JsonSerializer.Deserialize<RolViewModel>(jsonModel);

         model.RolUsuarioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.RolUsuarioDTO.OrderBy = "u.nombre_completo";

         try
         {
            List<RolUsuarioDTO> rolUsuarioDTOList = rolUsuarioProxy.Search(model.RolUsuarioDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = rolUsuarioDTOList.Count,
               rows = from f in rolUsuarioDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.RolUsuarioId,
                            f.RolId,
                            f.UsuarioId,
                            f.UsuarioDTO.UsuarioLogon,
                            f.UsuarioDTO.NombreCompleto,
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
      [HttpPost("GrabarUsuario")]
      public ActionResult GrabarUsuario([FromBody] RolViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            rolUsuarioProxy.Insert(model.RolUsuarioDTO);

            resultResponse.Message = SR.GetString("AS_rol_usuario_agregado");
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
      [HttpPost("EliminarUsuario")]
      public ActionResult EliminarUsuario([FromBody] RolViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.RolUsuarioDTO.DTOState = DTOState.Deleted;

            rolUsuarioProxy.Delete(model.RolUsuarioDTO);

            resultResponse.Message = SR.GetString("AS_rol_usuario_eliminado");
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
      [HttpPost("ListarMenu")]
      public ActionResult ListarMenu(string jsonModel)
      {

         Logger.InfoStart();

         RolViewModel model = JsonSerializer.Deserialize<RolViewModel>(jsonModel);

         try
         {
            List<RolMenuDTO> rolMenuDTOList = rolMenuProxy.ListarPorRolId(new RolMenuDTO() { RolId = model .RolUsuarioDTO.RolId});

            var jsonData = new
            {
               total = 1,
               page = 1,
               records = rolMenuDTOList.Count,
               rows = from f in rolMenuDTOList.AsEnumerable()
                      select new
                      {
                         id = f.MenuId.ToString(),
                         isLeaf = !rolMenuDTOList.Exists(e => e.MenuDTO.MenuPadreId == f.MenuId),
                         expanded = true,
                         parent = f.MenuDTO.MenuPadreId == null ? string.Empty : f.MenuDTO.MenuPadreId.ToString(),
                         level = f.MenuDTO.MenuPadreId == null ? 0 : getLevel(rolMenuDTOList, f.MenuDTO.MenuPadreId),
                         f.RolMenuId,
                         f.RolId,
                         f.MenuId,
                         f.MenuDTO.Nombre,
                         Lectura = f.Lectura == SiNoConstante.S ? DigitoConstante.UNO : DigitoConstante.CERO,
                         Escritura = f.Escritura == SiNoConstante.S ? DigitoConstante.UNO : DigitoConstante.CERO
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
      private int getLevel(List<RolMenuDTO> list, int? menuPadreId)
      {
         RolMenuDTO rolMenuDTO = list.Where(x => x.MenuDTO.MenuId == menuPadreId).FirstOrDefault();
         if (rolMenuDTO.MenuDTO.MenuPadreId == null)
            return 1;
         else
            return 1 + getLevel(list, rolMenuDTO.MenuDTO.MenuPadreId);
      }
      [Route("ListPopup")]
      public IActionResult ListPopup()
      {
         ViewBag.ModalEditId = GetModalEditPanelId();
         return PartialView();
      }
   }
}