using ESD.Core;
using ESD.Core.Cryptography;
using ESD.Core.Web;
using ESD.Core.Service.DTO;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Unity;

namespace ESD.ERP.Web.Infrastructure
{
   [ResponseCache(Duration = 0, NoStore = true)]
   public class DataController : Controller
   {

      private IUsuarioProxy usuarioProxy;
      private IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy;
      private IRolUsuarioProxy rolUsuarioProxy;
      private IParametroSistemaProxy parametroSistemaProxy;
      private IRolMenuProxy rolMenuProxy;

      [InjectionMethod]
      public void UseUsuarioProxy(IUsuarioProxy usuarioProxy)
      {
         this.usuarioProxy = usuarioProxy;
      }
      [InjectionMethod]
      public void UsePeriodoEmpresaSedeProxy(IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy)
      {
         this.periodoEmpresaSedeProxy = periodoEmpresaSedeProxy;
      }
      [InjectionMethod]
      public void UseRolUsuarioProxy(IRolUsuarioProxy rolUsuarioProxy)
      {
         this.rolUsuarioProxy = rolUsuarioProxy;
      }
      [InjectionMethod]
      public void UseParametroSistemaProxy(IParametroSistemaProxy parametroSistemaProxy)
      {
         this.parametroSistemaProxy = parametroSistemaProxy;
      }
      [InjectionMethod]
      public void UseRolMenuProxy(IRolMenuProxy rolMenuProxy)
      {
         this.rolMenuProxy = rolMenuProxy;
      }
      public int ObtenerMaximoRegistrosBusqueda()
      {
         int maxItems = 500; //Convert.ToInt32(ConfigurationManager.AppSettings[WebConfigConstante.MAX_REG_BUSQUEDA]);
         return maxItems;
      }
      public int ObtenerUsuarioId()
      {
         string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

         if (string.IsNullOrEmpty(value))
            return 0;

         return Convert.ToInt32(value);
      }
      public UsuarioDTO ObtenerUsuarioDTO()
      {
         int usuarioId = ObtenerUsuarioId();

         if (usuarioId == 0)
            return null;

         return usuarioProxy.GetById(new UsuarioDTO() { UsuarioId = usuarioId });
      }
      public bool EsAdministrador()
      {
         List<string> rolList = ObtenerRoles();

         if (rolList.Contains(RolConstante.SYS_ADMIN))
            return true;
         if (rolList.Contains(RolConstante.LOCAL_ADMIN))
            return true;
         if (rolList.Contains(RolConstante.TI))
            return true;

         return false;
      }
      public bool EsPricing() 
      {
        List<string> rolList = ObtenerRoles();

        if (rolList.Contains(RolConstante.PRICING))
            return true;

        return false;
      }

      public bool EsCaja()
      {
        List<string> rolList = ObtenerRoles();

        if (rolList.Contains(RolConstante.CAJA_OPERADOR))
            return true;

        return false;
      }
      
      
      public List<string> ObtenerRoles()
      {
         UsuarioDTO usuarioDTO = ObtenerUsuarioDTO();

         List<RolUsuarioDTO> rolUsuarioDTOList = rolUsuarioProxy.Search(new RolUsuarioDTO() {
            UsuarioId = usuarioDTO.UsuarioId
         });

         return rolUsuarioDTOList.Select(x => x.RolDTO.Codigo).ToList();
      }
      public bool UsuarioEnRol(List<string> list)
      {
         UsuarioDTO usuarioDTO = ObtenerUsuarioDTO();

         List<RolUsuarioDTO> rolUsuarioDTOList = rolUsuarioProxy.Search(new RolUsuarioDTO()
         {
            UsuarioId = usuarioDTO.UsuarioId,
            RolList = list
         });

         return rolUsuarioDTOList.Count > 0;
      }
      public List<RolMenuDTO> ObtenerPermisosUsuario()
      {
         if (HttpContext != null && HttpContext.Items.ContainsKey("USER_MENU_PERMISSIONS"))
         {
            return (List<RolMenuDTO>)HttpContext.Items["USER_MENU_PERMISSIONS"];
         }

         if (rolMenuProxy == null && HttpContext != null)
         {
            rolMenuProxy = HttpContext.RequestServices?.GetService(typeof(IRolMenuProxy)) as IRolMenuProxy;
         }

         List<string> roles = ObtenerRoles();
         if (roles == null || roles.Count == 0 || rolMenuProxy == null)
            return new List<RolMenuDTO>();

         List<RolMenuDTO> rolMenuDTOList = rolMenuProxy.Search(new RolMenuDTO()
         {
            RolCodigoList = roles,
            Lectura = SiNoConstante.S
         });

         if (HttpContext != null && rolMenuDTOList != null)
         {
            HttpContext.Items["USER_MENU_PERMISSIONS"] = rolMenuDTOList;
         }

         return rolMenuDTOList ?? new List<RolMenuDTO>();
      }
      private bool EsPermisoCoincidente(RolMenuDTO p, string controllerName, string areaName = null, string menuUrl = null)
      {
         if (p?.MenuDTO == null)
            return false;

         string menuDtoUrl = p.MenuDTO.Url ?? string.Empty;
         string menuDtoArea = p.MenuDTO.Area ?? string.Empty;

         var menuDtoSegments = menuDtoUrl.Trim().Trim('/').Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

         // 1. If explicit menuUrl was passed, check direct URL or segment match
         if (!string.IsNullOrWhiteSpace(menuUrl))
         {
            string cleanMenuUrl = menuUrl.Trim().Trim('/').ToLower();
            string cleanDtoUrl = menuDtoUrl.Trim().Trim('/').ToLower();

            if (cleanMenuUrl == cleanDtoUrl)
               return true;

            var inputSegments = cleanMenuUrl.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var seg in inputSegments)
            {
               if (seg.Equals("index", StringComparison.OrdinalIgnoreCase) || 
                   seg.Equals("listar", StringComparison.OrdinalIgnoreCase) || 
                   seg.Equals("crear", StringComparison.OrdinalIgnoreCase) || 
                   seg.Equals("editar", StringComparison.OrdinalIgnoreCase)) 
                  continue;

               if (menuDtoSegments.Any(ms => ms.Equals(seg, StringComparison.OrdinalIgnoreCase)))
                  return true;
            }
         }

         // 2. Match by controllerName
         if (!string.IsNullOrWhiteSpace(controllerName))
         {
            string cleanController = controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) 
               ? controllerName.Substring(0, controllerName.Length - 10) 
               : controllerName;

            bool controllerMatches = menuDtoSegments.Any(s => s.Equals(cleanController, StringComparison.OrdinalIgnoreCase));
            if (controllerMatches)
            {
               if (!string.IsNullOrWhiteSpace(areaName) && !string.IsNullOrWhiteSpace(menuDtoArea))
               {
                  return areaName.Equals(menuDtoArea, StringComparison.OrdinalIgnoreCase);
               }
               return true;
            }
         }

         return false;
      }
      public bool TienePermisoEscritura(string menuUrl = null)
      {
         if (EsAdministrador())
            return true;

         string controllerName = ControllerContext.ActionDescriptor?.ControllerName ?? string.Empty;
         string areaName = ControllerContext.RouteData?.Values["area"]?.ToString() ?? string.Empty;

         if (string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(menuUrl))
         {
            var parts = menuUrl.Trim().Trim('/').Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
               controllerName = parts.Length > 1 ? parts[1] : parts[0];
               if (parts.Length > 1) areaName = parts[0];
            }
         }

         if (string.IsNullOrEmpty(controllerName))
            return false;

         if (controllerName.Equals("PerfilAcceso", StringComparison.OrdinalIgnoreCase) ||
             controllerName.Equals("Home", StringComparison.OrdinalIgnoreCase) ||
             controllerName.Equals("Menu", StringComparison.OrdinalIgnoreCase) ||
             controllerName.Equals("Login", StringComparison.OrdinalIgnoreCase))
         {
            return true;
         }

         List<RolMenuDTO> permisos = ObtenerPermisosUsuario();
         if (permisos == null || permisos.Count == 0)
            return false;

         return permisos.Any(p =>
            p.Escritura == SiNoConstante.S &&
            EsPermisoCoincidente(p, controllerName, areaName, menuUrl)
         );
      }
      public bool TienePermisoLectura(string menuUrl = null)
      {
         if (EsAdministrador())
            return true;

         string controllerName = ControllerContext.ActionDescriptor?.ControllerName ?? string.Empty;
         string areaName = ControllerContext.RouteData?.Values["area"]?.ToString() ?? string.Empty;

         if (string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(menuUrl))
         {
            var parts = menuUrl.Trim().Trim('/').Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
               controllerName = parts.Length > 1 ? parts[1] : parts[0];
               if (parts.Length > 1) areaName = parts[0];
            }
         }

         if (string.IsNullOrEmpty(controllerName))
            return false;

         if (controllerName.Equals("PerfilAcceso", StringComparison.OrdinalIgnoreCase) ||
             controllerName.Equals("Home", StringComparison.OrdinalIgnoreCase) ||
             controllerName.Equals("Menu", StringComparison.OrdinalIgnoreCase) ||
             controllerName.Equals("Login", StringComparison.OrdinalIgnoreCase))
         {
            return true;
         }

         List<RolMenuDTO> permisos = ObtenerPermisosUsuario();
         if (permisos == null || permisos.Count == 0)
            return false;

         return permisos.Any(p =>
            p.Lectura == SiNoConstante.S &&
            EsPermisoCoincidente(p, controllerName, areaName, menuUrl)
         );
      }
      public override void OnActionExecuting(ActionExecutingContext context)
      {
         base.OnActionExecuting(context);

         string currentController = ControllerContext.ActionDescriptor?.ControllerName ?? string.Empty;
         if (currentController.Equals("PerfilAcceso", StringComparison.OrdinalIgnoreCase) ||
             currentController.Equals("Home", StringComparison.OrdinalIgnoreCase) ||
             currentController.Equals("Menu", StringComparison.OrdinalIgnoreCase) ||
             currentController.Equals("Login", StringComparison.OrdinalIgnoreCase))
         {
            return;
         }

         bool tieneEscritura = TienePermisoEscritura();
         ViewData["TienePermisoEscritura"] = tieneEscritura;
         ViewData["EsSoloLectura"] = !tieneEscritura;
         if (ViewBag != null)
         {
            ViewBag.TienePermisoEscritura = tieneEscritura;
            ViewBag.EsSoloLectura = !tieneEscritura;
         }
         if (context.HttpContext != null)
         {
            context.HttpContext.Items["TienePermisoEscritura"] = tieneEscritura;
            context.HttpContext.Items["EsSoloLectura"] = !tieneEscritura;
            if (context.HttpContext.Response != null && !context.HttpContext.Response.HasStarted)
            {
               context.HttpContext.Response.Headers["X-App-SoloLectura"] = tieneEscritura ? "false" : "true";
            }
         }

         // Universal backend write protection for read-only roles
         if (!tieneEscritura && Microsoft.AspNetCore.Http.HttpMethods.IsPost(context.HttpContext.Request.Method))
         {
            string actionName = context.ActionDescriptor?.RouteValues["action"]?.ToLower() ?? string.Empty;
            if (actionName == "grabar" || 
                actionName == "eliminar" || 
                actionName == "aprobar" || 
                actionName == "desaprobar" || 
                actionName == "generarpedido" || 
                actionName == "generarcomprobante" || 
                actionName == "cambiarestado" ||
                actionName == "enviarproduccion" ||
                actionName == "cerraranticipo" ||
                actionName == "extornar")
            {
               context.Result = new JsonResult(new ResultResponse
               {
                  Success = false,
                  Message = "No cuenta con permisos de escritura para realizar modificaciones en este mÃ³dulo."
               });
               return;
            }
         }
      }

      public override ViewResult View()
      {
         procesarPermisosModelo(null);
         return base.View();
      }
      public override ViewResult View(object model)
      {
         procesarPermisosModelo(model);
         return base.View(model);
      }
      public override ViewResult View(string viewName)
      {
         procesarPermisosModelo(null);
         return base.View(viewName);
      }
      public override ViewResult View(string viewName, object model)
      {
         procesarPermisosModelo(model);
         return base.View(viewName, model);
      }

      public override PartialViewResult PartialView()
      {
         procesarPermisosModelo(null);
         return base.PartialView();
      }
      public override PartialViewResult PartialView(object model)
      {
         procesarPermisosModelo(model);
         return base.PartialView(model);
      }
      public override PartialViewResult PartialView(string viewName)
      {
         procesarPermisosModelo(null);
         return base.PartialView(viewName);
      }
      public override PartialViewResult PartialView(string viewName, object model)
      {
         procesarPermisosModelo(model);
         return base.PartialView(viewName, model);
      }

      public override void OnActionExecuted(ActionExecutedContext context)
      {
         base.OnActionExecuted(context);
         bool tieneEscritura = TienePermisoEscritura();
         ViewData["TienePermisoEscritura"] = tieneEscritura;
         ViewData["EsSoloLectura"] = !tieneEscritura;
         if (ViewBag != null)
         {
            ViewBag.TienePermisoEscritura = tieneEscritura;
            ViewBag.EsSoloLectura = !tieneEscritura;
         }
         if (context.HttpContext != null)
         {
            context.HttpContext.Items["TienePermisoEscritura"] = tieneEscritura;
            context.HttpContext.Items["EsSoloLectura"] = !tieneEscritura;
            if (context.HttpContext.Response != null && !context.HttpContext.Response.HasStarted)
            {
               context.HttpContext.Response.Headers["X-App-SoloLectura"] = tieneEscritura ? "false" : "true";
            }
         }

         if (context.Result is ViewResult viewResult)
         {
            procesarPermisosModelo(viewResult.Model);
         }
         else if (context.Result is PartialViewResult partialViewResult)
         {
            procesarPermisosModelo(partialViewResult.Model);
         }
      }

      private void procesarPermisosModelo(object model)
      {
         bool tieneEscritura = TienePermisoEscritura();
         ViewData["TienePermisoEscritura"] = tieneEscritura;
         ViewData["EsSoloLectura"] = !tieneEscritura;
         if (ViewBag != null)
         {
            ViewBag.TienePermisoEscritura = tieneEscritura;
            ViewBag.EsSoloLectura = !tieneEscritura;
         }
         if (HttpContext != null)
         {
            HttpContext.Items["TienePermisoEscritura"] = tieneEscritura;
            HttpContext.Items["EsSoloLectura"] = !tieneEscritura;
            if (HttpContext.Response != null && !HttpContext.Response.HasStarted)
            {
               HttpContext.Response.Headers["X-App-SoloLectura"] = tieneEscritura ? "false" : "true";
            }
         }

         if (model == null)
            return;

         try
         {
            var props = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
               if (typeof(List<MenuItem>).IsAssignableFrom(prop.PropertyType))
               {
                  var list = prop.GetValue(model) as List<MenuItem>;
                  if (list != null && list.Count > 0)
                  {
                     AplicarPermisosMenu(list);
                  }
               }
            }
         }
         catch
         {
         }
      }

      public void AplicarPermisosMenu(List<MenuItem> menuItemList, string menuUrl = null)
      {
         if (menuItemList == null || menuItemList.Count == 0)
            return;

         bool tieneEscritura = TienePermisoEscritura(menuUrl);
         ViewData["TienePermisoEscritura"] = tieneEscritura;
         ViewData["EsSoloLectura"] = !tieneEscritura;
         if (ViewBag != null)
         {
            ViewBag.TienePermisoEscritura = tieneEscritura;
            ViewBag.EsSoloLectura = !tieneEscritura;
         }
         if (HttpContext != null)
         {
            HttpContext.Items["TienePermisoEscritura"] = tieneEscritura;
            HttpContext.Items["EsSoloLectura"] = !tieneEscritura;
            if (HttpContext.Response != null && !HttpContext.Response.HasStarted)
            {
               HttpContext.Response.Headers["X-App-SoloLectura"] = tieneEscritura ? "false" : "true";
            }
         }

         if (tieneEscritura)
            return;

         menuItemList.RemoveAll(item =>
            item.MenuItemType == MenuItemType.New ||
            item.MenuItemType == MenuItemType.Delete ||
            item.MenuItemType == MenuItemType.Save ||
            item.MenuItemType == MenuItemType.Cancel ||
            item.MenuItemType == MenuItemType.ImportXLS ||
            item.Key == MenuItemKey.NEW ||
            item.Key == MenuItemKey.DELETE ||
            item.Key == MenuItemKey.SAVE ||
            item.Key == MenuItemKey.CANCEL ||
            item.Key == MenuItemKey.IMPORT_XLS ||
            item.Key == "new" ||
            item.Key == "delete" ||
            item.Key == "save" ||
            item.Key == "cancel" ||
            item.Key == "aprobar" ||
            item.Key == "desaprobar" ||
            item.Key == "soli_auto" ||
            item.Key == "gen_pedido" ||
            item.Key == "gen_compro" ||
            item.Key == "cambiar_cotizacion" ||
            item.Key == "cambiar_estado" ||
            item.Key == "enviar_prod" ||
            item.Key == "cerrar" ||
            item.Key == "extornar"
         );

         foreach (var item in menuItemList)
         {
            if (item.MenuItemType == MenuItemType.Edit || 
                item.Key == MenuItemKey.EDIT || 
                item.Key == "edit" || 
                (item.Content != null && item.Content.Trim().Equals("Editar", StringComparison.OrdinalIgnoreCase)))
            {
               item.MenuItemType = MenuItemType.Custom;
               item.Key = MenuItemKey.EDIT;
               item.Content = "Consultar";
               item.FontImage = "fas fa-search";
            }
         }
      }
      public int ObtenerPeriodoId(string cid)
      {
         return obtenerValorVariable(cid,0);
      }
      public int ObtenerEmpresaId()
      {
         return obtenerValorVariable(string.Empty, 1);
      }
      public int ObtenerEmpresaId(string cid)
      {
         return obtenerValorVariable(cid, 1);
      }
      public int ObtenerPeriodoEmpresaSedeId(string cid)
      {
         return obtenerValorVariable(cid, 2);
      }
      public int ObtenerPeriodoEmpresaSedeId()
      {
         return obtenerValorVariable(string.Empty, 2);
      }
      private int obtenerValorVariable(string variableString, int index) {

         if (string.IsNullOrEmpty(variableString)) {
            variableString = Request.Headers["x-app-cid"];
            if (string.IsNullOrEmpty(variableString))
               variableString = string.Empty;
         }

         string value = Base64UrlEncoder.Decode(variableString);

         Cryptor cryptor = new Cryptor();
         string decryptString = cryptor.Decrypt(value, cryptor.GetEncodeBytes("User"));

         string[] variableList = decryptString.Split(',');

         return Convert.ToInt32(variableList[index]);
      }
      public PeriodoEmpresaSedeDTO ObtenerPeriodoEmpresaSedeDTO()
      {
         int periodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId();

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = periodoEmpresaSedeProxy
            .GetById(new PeriodoEmpresaSedeDTO() { PeriodoEmpresaSedeId = periodoEmpresaSedeId });

         return periodoEmpresaSedeDTO;
      }
      public PeriodoEmpresaSedeDTO ObtenerPeriodoEmpresaSedeDTO(string cid)
      {
         int periodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(cid);

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = periodoEmpresaSedeProxy
            .GetById(new PeriodoEmpresaSedeDTO() { PeriodoEmpresaSedeId = periodoEmpresaSedeId });

         return periodoEmpresaSedeDTO;
      }
      public PeriodoDTO ObtenerPeriodoDTO(string cid)
      {
         int periodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(cid);

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = periodoEmpresaSedeProxy
            .GetById(new PeriodoEmpresaSedeDTO() { PeriodoEmpresaSedeId = periodoEmpresaSedeId });

         return periodoEmpresaSedeDTO.PeriodoDTO;
      }
      public PeriodoDTO ObtenerPeriodoDTO()
      {
         int periodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId();

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = periodoEmpresaSedeProxy
            .GetById(new PeriodoEmpresaSedeDTO() { PeriodoEmpresaSedeId = periodoEmpresaSedeId });

         return periodoEmpresaSedeDTO.PeriodoDTO;
      }
      public void AuditDTO(DTOBase dto)
      {

         int usuarioId = ObtenerUsuarioId();

         changeAuditProperty("UsuarioCreacionId", dto, usuarioId);
         changeAuditProperty("UsuarioModificacionId", dto, usuarioId);
         changeAuditProperty("FechaModificacion", dto, DateTime.Now);
      }
      private void changeAuditProperty(string propertyName, object dto, object newValue)
      {
         Type type = dto.GetType();

         PropertyInfo propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
         if (propertyInfo == null)
            return;

         //object defaultValue = propertyInfo.GetValue(dto, null);
         object value = TypeUtil.ChangeType(newValue, propertyInfo.PropertyType);
         propertyInfo.SetValue(dto, value, null);
      }
      public string CreateCID(string variableString) {

         string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "jwt")?.Value;

         Cryptor cryptor = new Cryptor();
         string cid = cryptor.Encrypt(variableString, cryptor.GetEncodeBytes("User"));

         return Base64UrlEncoder.Encode(cid);
      }
      public string RandomString(int length)
      {
         Random random = new Random();
         const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
         var chars = Enumerable.Range(0, length)
             .Select(x => pool[random.Next(0, pool.Length)]);
         return new string(chars.ToArray());
      }
      public string GetModalEditPanelId()
      {
         return "mp-edit-panel-" + RandomString(10);
      }
      public string ObtenerDireccionServicioGeneracionFormatoImpresion()
      {

         ParametroSistemaDTO parametroSistemaDTO = parametroSistemaProxy.SingleSearch(new ParametroSistemaDTO() { 
            Codigo = ParametroSistemaConstante.DATOS_MATRICIAL_URL,
            Estado = EstadoConstante.ACTIVO
         });

         if (parametroSistemaDTO == null)
            return string.Empty;

         return parametroSistemaDTO.ValorCadena;
      }
   }
   public class CheckAndHandleFileResultAttribute : ResultFilterAttribute
   {
      private const string FILE_DOWNLOAD_COOKIE_NAME = "fileDownload";
      public override void OnResultExecuting(ResultExecutingContext context)
      {
         if (context.Result is FileResult)
            context.HttpContext.Response.Cookies.Append(FILE_DOWNLOAD_COOKIE_NAME, "true", new Microsoft.AspNetCore.Http.CookieOptions()
            {
               Path = "/"
            });
         else
            if (context.HttpContext.Request.Cookies[FILE_DOWNLOAD_COOKIE_NAME] != null)
            context.HttpContext.Response.Cookies.Append(FILE_DOWNLOAD_COOKIE_NAME, "true", new Microsoft.AspNetCore.Http.CookieOptions()
            {
               Expires = DateTime.Now.AddYears(-1)
            });

         base.OnResultExecuting(context);
      }
   }
} 



