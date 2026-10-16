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
   public class UsuarioController : AuthorizedController
   {
      private readonly IUsuarioProxy usuarioProxy;
      private readonly IOrigenRegistroProxy origenRegistroProxy;
      private readonly IRolUsuarioProxy rolUsuarioProxy;
      private readonly IDirectivaReglaProxy directivaReglaProxy;
      private readonly IDirectivaUsuarioProxy directivaUsuarioProxy;
      private readonly IProveedorProxy proveedorProxy;
      private readonly IUsuarioHorarioProxy usuarioHorarioProxy;
      private readonly IParametroSistemaProxy parametroSistemaProxy;

      public UsuarioController(
         IUsuarioProxy usuarioProxy,
         IOrigenRegistroProxy origenRegistroProxy,
         IRolUsuarioProxy rolUsuarioProxy,
         IDirectivaReglaProxy directivaReglaProxy,
         IDirectivaUsuarioProxy directivaUsuarioProxy,
         IProveedorProxy proveedorProxy,
         IUsuarioHorarioProxy usuarioHorarioProxy,
         IParametroSistemaProxy parametroSistemaProxy
         )
      {
         this.usuarioProxy = usuarioProxy;
         this.origenRegistroProxy = origenRegistroProxy;
         this.rolUsuarioProxy = rolUsuarioProxy;
         this.directivaReglaProxy = directivaReglaProxy;
         this.directivaUsuarioProxy = directivaUsuarioProxy;
         this.proveedorProxy = proveedorProxy;
         this.usuarioHorarioProxy = usuarioHorarioProxy;
         this.parametroSistemaProxy = parametroSistemaProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         UsuarioViewModel usuarioViewModel = new();

         usuarioViewModel.MenuItemList = new List<MenuItem>() { 
            new MenuItem() { Key="habilitar", Content="Habilitar"},
            new MenuItem() { Key="deshabilitar", Content="Deshabilitar"},
            new MenuItem() { Key="cambiar", Content="Cambiar Contraseña"}};

         return PartialView(usuarioViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)  
      {

         Logger.InfoStart();

         UsuarioViewModel model = JsonSerializer.Deserialize<UsuarioViewModel>(jsonModel);

         model.UsuarioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.UsuarioDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
         model.UsuarioDTO.OrderBy = "nombre_completo";

         try
         {
            List<UsuarioDTO> usuarioDTOList = usuarioProxy.Search(model.UsuarioDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = usuarioDTOList.Count,
               rows = from f in usuarioDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.UsuarioId,
                            f.UsuarioLogon,
                            f.NombreCompleto,
                            f.Email,
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
      public ActionResult Crear([FromBody]UsuarioViewModel model)
      {
         Logger.InfoStart();

         UsuarioViewModel usuarioViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(usuarioViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, usuarioViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody]UsuarioViewModel model)
      {

         Logger.InfoStart();

         UsuarioViewModel usuarioViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(usuarioViewModel, model.UsuarioDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, usuarioViewModel);
      }
      private void cargarDataParaEditar(UsuarioViewModel model, int id)
      {

         UsuarioDTO usuarioDTO;
         if (model.EditAction == ViewModelEditAction.New)
         {
            usuarioDTO = new UsuarioDTO
            {
               Estado = EstadoConstante.ACTIVO,
               NoExpira = EstadoConstante.ACTIVO
            };
         }
         else
         {
            usuarioDTO = usuarioProxy.GetById(new UsuarioDTO() { UsuarioId = id });

            usuarioDTO.CambiarContrasenaProximo = usuarioDTO.CambiarContrasenaProximo == SiNoConstante.N ? EstadoConstante.INACTIVO : EstadoConstante.ACTIVO;
            usuarioDTO.NoPuedeCambiarContrasena = usuarioDTO.NoPuedeCambiarContrasena == SiNoConstante.N ? EstadoConstante.INACTIVO : EstadoConstante.ACTIVO;
            usuarioDTO.NoExpira = usuarioDTO.NoExpira == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
            usuarioDTO.TieneHorario = usuarioDTO.TieneHorario == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         }

         model.UsuarioDTO = usuarioDTO;

         ProveedorDTO proveedorDTO = null;
         if (usuarioDTO.Id != 0) {
            proveedorDTO = proveedorProxy.SingleSearch(new ProveedorDTO()
            {
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
               UsuarioId = usuarioDTO.Id
            });
         }

         if (proveedorDTO == null)
            proveedorDTO = new ProveedorDTO();

         model.UsuarioDTO.ProveedorDTO = proveedorDTO;
         model.UsuarioDTO.ProveedorId = proveedorDTO.Id;

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy
            .SingleSearch(new ParametroSistemaDTO() { Codigo = ParametroSistemaConstante.HORARIO_ACCESO_LV });

         model.HoraInicio = parametroSistemaDTO.RangoNumericoDesde.ToString("00.00").Replace(SeparadorConstante.PUNTO,SeparadorConstante.DOS_PUNTOS);
         model.HoraFin = parametroSistemaDTO.RangoNumericoHasta.ToString("00.00").Replace(SeparadorConstante.PUNTO, SeparadorConstante.DOS_PUNTOS);

         cargarListas(model);

         MenuBarTagHelper menuBarTagHelper = new();

         model.MenuItemList =menuBarTagHelper.CreateMenuStyleList(MenuItemStyle.EditAndSaveCancel);

         model.MenuItemList.Add(new MenuItem() { Key = "selectLS", Content = "L-S", Disabled = true });
         model.MenuItemList.Add(new MenuItem() { Key = "selectNone", Content = "Ningun Día", Disabled = true });
      }
      private void cargarListas(UsuarioViewModel model)
      {

         List<DirectivaReglaDTO> directivaReglaDTOList = directivaReglaProxy.Search(new DirectivaReglaDTO() { Estado = EstadoConstante.ACTIVO });
         model.DirectivaReglaList = WebHelper.ToSelectListItem(directivaReglaDTOList, x => x.Id.ToString(), x => x.Descripcion,
            SelectListFirstOptionType.Select);

      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody]UsuarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            UsuarioDTO usuarioDTO = cargarUsuarioDTO(model);

            int newId = model.UsuarioDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = usuarioProxy.Insert(usuarioDTO);
                  messageResourceKey = "AS_usuario_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  usuarioProxy.Update(usuarioDTO);
                  messageResourceKey = "AS_usuario_actualizado";
                  break;
            }

            usuarioDTO = usuarioProxy.GetById(new UsuarioDTO() { UsuarioId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, usuarioDTO.UsuarioLogon);
            resultResponse.Data = new
            {
               usuarioDTO.Id
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
      private UsuarioDTO cargarUsuarioDTO(UsuarioViewModel model)
      {

         UsuarioDTO usuarioDTO;
         try
         {

            usuarioDTO = model.UsuarioDTO;

            if (model.EditAction == ViewModelEditAction.New)
            {
               OrigenRegistroDTO origenRegistroDTO = origenRegistroProxy.ObtenerPorCodigo(new OrigenRegistroDTO() { Codigo = OrigenRegistroConstante.USUARIO });

               usuarioDTO.OrigenRegistroId = origenRegistroDTO.Id;
            }

            if (string.IsNullOrEmpty(usuarioDTO.EmailAlterno))
               usuarioDTO.EmailAlterno = string.Empty;

            if (string.IsNullOrEmpty(usuarioDTO.CodigoRecuperacionContrasena))
               usuarioDTO.CodigoRecuperacionContrasena = string.Empty;

            if (string.IsNullOrEmpty(usuarioDTO.TieneHorario))
               usuarioDTO.TieneHorario = SiNoConstante.N;

            if (usuarioDTO.ProveedorId.HasValue)
            {
               usuarioDTO.ProveedorDTO = new ProveedorDTO()
               {
                  EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
                  ProveedorId = usuarioDTO.ProveedorId.Value
               };
            }

            //AuditDTO(usuarioDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return usuarioDTO;
      }
      [HttpGet("Habilitar")]
      public JsonResult Habilitar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            UsuarioDTO usuarioDTO = usuarioProxy.GetById(new UsuarioDTO() { UsuarioId = id});

            usuarioDTO.Estado = EstadoConstante.ACTIVO;

            usuarioProxy.ActualizarEstado(usuarioDTO);

            resultResponse.Message = SR.GetString("AS_usuario_habilitado", usuarioDTO.UsuarioLogon);
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
      [HttpGet("Deshabilitar")]
      public JsonResult Deshabilitar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            UsuarioDTO usuarioDTO = usuarioProxy.GetById(new UsuarioDTO() { UsuarioId = id });

            usuarioDTO.Estado = EstadoConstante.INACTIVO;

            usuarioProxy.ActualizarEstado(usuarioDTO);

            resultResponse.Message = SR.GetString("AS_usuario_deshabilitado", usuarioDTO.UsuarioLogon);
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
      [HttpPost("CambiarContrasenaPopup")]
      public ActionResult ContrasenaEditPopup([FromBody] UsuarioViewModel model)
      {
         if (model == null)
            model = new();

         if (model.UsuarioDTO == null)
            model.UsuarioDTO = new UsuarioDTO()
            {
               UsuarioId = ObtenerUsuarioId()
            };

         return PartialView(model);
      }
      [HttpPost("CambiarContrasena")]
      public ActionResult CambiarContrasena([FromBody] UsuarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            usuarioProxy.ActualizarContrasena(model.UsuarioDTO);

            resultResponse.Message = SR.GetString("AS_contrasena_actualizada");

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
      [HttpPost("CambiarContrasenaAnterior")]
      public ActionResult CambiarContrasenaAnterior([FromBody] UsuarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            usuarioProxy.ActualizarContrasenaAnterior(model.UsuarioDTO);

            resultResponse.Message = SR.GetString("AS_contrasena_actualizada");

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
      [HttpPost("ListarDirectivaRegla")]
      public ActionResult ListarDirectivaRegla(string jsonModel)
      {

         Logger.InfoStart();

         UsuarioViewModel model = JsonSerializer.Deserialize<UsuarioViewModel>(jsonModel);

         model.DirectivaUsuarioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();

         try
         {
            List<DirectivaUsuarioDTO> directivaUsuarioDTOList = directivaUsuarioProxy.Search(model.DirectivaUsuarioDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = directivaUsuarioDTOList.Count,
               rows = from f in directivaUsuarioDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.DirectivaUsuarioId,
                            f.DirectivaReglaId,
                            f.UsuarioId,
                            f.Valor,
                            f.Codigo,
                            f.Descripcion,
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
      [HttpPost("GrabarDirectivaUsuario")]
      public ActionResult GrabarDirectivaUsuario([FromBody] UsuarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            directivaUsuarioProxy.Insert(model.DirectivaUsuarioDTO);

            resultResponse.Message = SR.GetString("AS_directiva_usuario_agregado");
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
      [HttpPost("EliminarDirectivaUsuario")]
      public ActionResult EliminarDirectivaUsuario([FromBody] UsuarioViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.DirectivaUsuarioDTO.DTOState = DTOState.Deleted;

            directivaUsuarioProxy.Delete(model.DirectivaUsuarioDTO);

            resultResponse.Message = SR.GetString("AS_directiva_usuario_eliminada");
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
      [HttpPost("ListarRoles")]
      public ActionResult ListarRoles(string jsonModel)
      {

         Logger.InfoStart();

         UsuarioViewModel model = JsonSerializer.Deserialize<UsuarioViewModel>(jsonModel);

         model.RolUsuarioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.RolUsuarioDTO.OrderBy = "r.nombre";

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
                            f.RolDTO.Nombre
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
      [HttpPost("GrabarRol")]
      public ActionResult GrabarRol([FromBody] UsuarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            rolUsuarioProxy.Insert(model.RolUsuarioDTO);

            resultResponse.Message = SR.GetString("AS_rol_usuario_2_agregado");
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
      [HttpPost("EliminarRol")]
      public ActionResult EliminarRol([FromBody] UsuarioViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.RolUsuarioDTO.DTOState = DTOState.Deleted;

            rolUsuarioProxy.Delete(model.RolUsuarioDTO);

            resultResponse.Message = SR.GetString("AS_rol_eliminado");
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
      public IActionResult ListPopup()
      {
         ViewBag.ModalEditId = GetModalEditPanelId();
         return PartialView();
      }
      [HttpPost("ListarUsuarioHorarios")]
      public ActionResult ListarUsuarioHorarios(string jsonModel)
      {

         Logger.InfoStart();

         UsuarioViewModel model = JsonSerializer.Deserialize<UsuarioViewModel>(jsonModel);

         model.UsuarioHorarioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();

         try
         {
            List<UsuarioHorarioDTO> usuarioHorarioDTOList = usuarioHorarioProxy.ListarPorUsuarioId(model.UsuarioHorarioDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = usuarioHorarioDTOList.Count,
               rows = from f in usuarioHorarioDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.UsuarioHorarioId,
                            f.UsuarioId,
                            f.DiaId,
                            f.DiaDTO.Codigo,
                            f.DiaDTO.Nombre,
                            f.HoraInicio,
                            f.HoraFin,
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
      [HttpPost("GrabarUsuarioHorarioLunesSabado")]
      public ActionResult GrabarUsuarioHorarioLunesSabado([FromBody] UsuarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         string[] sabado = new string[] { DiaConstante.SABADO};
         string[] excludeKeys = new string[] { DiaConstante.DOMINGO};
         string horaInicio = string.Empty;
         string horaFin = string.Empty;

         try
         {
            List<UsuarioHorarioDTO> usuarioHorarioDTOList = usuarioHorarioProxy.ListarPorUsuarioId(model.UsuarioHorarioDTO);

            ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy
               .SingleSearch(new ParametroSistemaDTO() { Codigo = ParametroSistemaConstante.HORARIO_ACCESO_LV });

            foreach (UsuarioHorarioDTO usuarioHorarioDTO in usuarioHorarioDTOList)
            {
               if (excludeKeys.Contains(usuarioHorarioDTO.DiaDTO.Codigo))
               {
                  horaInicio = string.Empty;
                  horaFin = string.Empty;
               }
               else if (sabado.Contains(usuarioHorarioDTO.DiaDTO.Codigo))
               {
                  horaInicio = parametroSistemaDTO.RangoNumericoDesde.ToString("00.00").Replace(SeparadorConstante.PUNTO, SeparadorConstante.DOS_PUNTOS);
                  horaFin = "14:00";
               }
               else
               {
                  horaInicio = parametroSistemaDTO.RangoNumericoDesde.ToString("00.00").Replace(SeparadorConstante.PUNTO, SeparadorConstante.DOS_PUNTOS);
                  horaFin = parametroSistemaDTO.RangoNumericoHasta.ToString("00.00").Replace(SeparadorConstante.PUNTO, SeparadorConstante.DOS_PUNTOS);
               }

               usuarioHorarioDTO.HoraInicio = horaInicio;
               usuarioHorarioDTO.HoraFin = horaFin;
               usuarioHorarioDTO.Estado = EstadoConstante.ACTIVO;
               AuditDTO(usuarioHorarioDTO);
            }

            usuarioHorarioProxy.ActualizarLote(usuarioHorarioDTOList);

            resultResponse.Message = SR.GetString("AS_usuario_horario_actualizacion_lote");
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
      [HttpPost("GrabarUsuarioHorarioDiario")]
      public ActionResult GrabarUsuarioHorarioDiario([FromBody] UsuarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            List<UsuarioHorarioDTO> usuarioHorarioDTOList = usuarioHorarioProxy.ListarPorUsuarioId(model.UsuarioHorarioDTO);

            ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy
               .SingleSearch(new ParametroSistemaDTO() { Codigo = ParametroSistemaConstante.HORARIO_ACCESO_DIARIO });

            foreach (UsuarioHorarioDTO usuarioHorarioDTO in usuarioHorarioDTOList)
            {
               usuarioHorarioDTO.HoraInicio = parametroSistemaDTO.RangoNumericoDesde.ToString("00.00").Replace(SeparadorConstante.PUNTO, SeparadorConstante.DOS_PUNTOS);
               usuarioHorarioDTO.HoraFin = parametroSistemaDTO.RangoNumericoHasta.ToString("00.00").Replace(SeparadorConstante.PUNTO, SeparadorConstante.DOS_PUNTOS);
               usuarioHorarioDTO.Estado = EstadoConstante.ACTIVO;
               AuditDTO(usuarioHorarioDTO);
            }

            usuarioHorarioProxy.ActualizarLote(usuarioHorarioDTOList);

            resultResponse.Message = SR.GetString("AS_usuario_horario_actualizacion_lote");
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
      [HttpPost("EliminarUsuarioHorario")]
      public ActionResult EliminarUsuarioHorario([FromBody] UsuarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            List<UsuarioHorarioDTO> usuarioHorarioDTOList = usuarioHorarioProxy.ListarPorUsuarioId(model.UsuarioHorarioDTO);

            foreach (UsuarioHorarioDTO usuarioHorarioDTO in usuarioHorarioDTOList)
            {
               usuarioHorarioDTO.HoraInicio = string.Empty;
               usuarioHorarioDTO.HoraFin = string.Empty;
               usuarioHorarioDTO.Estado = EstadoConstante.ACTIVO;
               AuditDTO(usuarioHorarioDTO);
            }

            usuarioHorarioProxy.ActualizarLote(usuarioHorarioDTOList);

            resultResponse.Message = SR.GetString("AS_usuario_horario_actualizacion_lote");
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
      [HttpPost("GrabarUsuarioHorario")]
      public ActionResult GrabarUsuarioHorario([FromBody] UsuarioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            if (string.IsNullOrEmpty(model.UsuarioHorarioDTO.HoraExtension))
               model.UsuarioHorarioDTO.HoraExtension = string.Empty;

            AuditDTO(model.UsuarioHorarioDTO);

            if (model.UsuarioHorarioDTO.UsuarioHorarioId == 0)
               usuarioHorarioProxy.Insert(model.UsuarioHorarioDTO);
            else
               usuarioHorarioProxy.Update(model.UsuarioHorarioDTO);

            resultResponse.Message = SR.GetString("AS_usuario_horario_actualizado");
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