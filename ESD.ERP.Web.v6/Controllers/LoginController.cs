using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.Web.Model;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESD.ERP.Web.Controllers
{
   [Authorize]
   public class LoginController : Controller
   {

      private readonly ISeguridadProxy seguridadProxy;
      private readonly IUsuarioProxy usuarioProxy;
      private readonly IPersonalProxy personalProxy;
      private readonly IPersonalSolicitudAccesoProxy personalSolicitudAccesoProxy;
      private readonly IEstadoAprobacionProxy estadoAprobacionProxy;
      public LoginController(
         ISeguridadProxy seguridadProxy,
         IUsuarioProxy usuarioProxy,
         IPersonalProxy personalProxy,
         IPersonalSolicitudAccesoProxy personalSolicitudAccesoProxy,
         IEstadoAprobacionProxy estadoAprobacionProxy
         )
      {
         this.seguridadProxy = seguridadProxy;
         this.usuarioProxy = usuarioProxy;
         this.personalProxy = personalProxy;
         this.personalSolicitudAccesoProxy = personalSolicitudAccesoProxy;
         this.estadoAprobacionProxy = estadoAprobacionProxy;
      }
      [HttpGet]
      [AllowAnonymous]
      public IActionResult Index()
      {
         //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

         return View();
      }

      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Index(LoginViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            if (ModelState.IsValid)
            {

               ServiceResponse serviceResponse = seguridadProxy.Autenticar(new UsuarioDTO()
                  { UsuarioLogon = model.UserName, Password = model.Password });

               UsuarioDTO usuarioDTO = JsonConvert.DeserializeObject<UsuarioDTO>(serviceResponse.Data.ToString());

               ClaimsIdentity claimsIdentity = new(new[] {
                  new Claim(ClaimTypes.NameIdentifier, usuarioDTO.Id.ToString()),
                  new Claim(ClaimTypes.Name, usuarioDTO.UsuarioLogon ?? usuarioDTO.Id.ToString()),
                  new Claim("jwt", serviceResponse.Token)
               }, CookieAuthenticationDefaults.AuthenticationScheme);

               ClaimsPrincipal claimsPrincipal = new();
               claimsPrincipal.AddIdentity(claimsIdentity);

               await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties()
               {
                  IsPersistent = false,
                  ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
               });

               if(usuarioDTO.CambiarContrasenaProximo == SiNoConstante.S)
                  resultResponse.Url = Url.Action("ChangePassword", "Login");
               else
               {
                  switch (usuarioDTO.CodigoHorarioExtendido)
                  {
                     case DigitoConstante.DOS:
                        throw new Exception(usuarioDTO.MensajeHorarioExtendido);

                     case DigitoConstante.TRES:
                     case DigitoConstante.NUEVE:
                        resultResponse.Url = Url.Action("SoliAccesoAdicional", "Login");
                        break;

                     default:
                        resultResponse.Url = redirectToLocal(string.Empty);
                        break;
                  }
                        

               }
            }
            else {
               resultResponse.Message = SR.GetString("AS_inicio_sesion_incorrecto");
               resultResponse.Success = false;
            }
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
      private string redirectToLocal(string returnUrl)
      {
         if (Url.IsLocalUrl(returnUrl))
         {
            return Url.Content(returnUrl);
         }
         else
         {
            return Url.Action(nameof(PerfilAccesoController.Index), "pf"); //PerfilAcceso
         }
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Logout()
      {
         await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

         return RedirectToAction(nameof(HomeController.Index), "Home");
      }

      public async Task<IActionResult> SignOutAsync()
      {
         await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
         return RedirectToAction("Index");
      }

      [HttpGet]
      public IActionResult ChangePassword()
      {
         string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

         LoginViewModel loginViewModel = new()
         {
            UsuarioDTO = usuarioProxy.GetById(new UsuarioDTO() { UsuarioId = Convert.ToInt32(value) })
         };

         return View(loginViewModel);
      }
      [HttpPost]
      [ValidateAntiForgeryToken]
      public IActionResult ChangePassword(LoginViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            UsuarioDTO usuarioDTO = usuarioProxy.ObtenerPorCodigo(new UsuarioDTO() { UsuarioLogon = model.UserName });

            usuarioProxy.ActualizarContrasenaAnterior(new UsuarioDTO() { 
               UsuarioId = usuarioDTO.Id,
               ContrasenaAnterior = model.OldPassword,
               Password = model.Password
            });

            resultResponse.Url = redirectToLocal(string.Empty);

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

      [HttpGet]
      public IActionResult SoliAccesoAdicional()
      {
         string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

         LoginViewModel loginViewModel = new()
         {
            UsuarioDTO = usuarioProxy.GetById(new UsuarioDTO() { UsuarioId = Convert.ToInt32(value) }),
            HoraSolicitada = DateTime.Now.AddHours(1).ToString("HH:mm")
         };

         return View(loginViewModel);
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public IActionResult SoliAccesoAdicional(LoginViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            EstadoAprobacionDTO estadoAprobacionDTO = estadoAprobacionProxy.ObtenerPorCodigo(new EstadoAprobacionDTO() { 
               Codigo = EstadoAprobacionConstante.PENDIENTE_APROBACION
            });

            PersonalDTO personalDTO = personalProxy.SingleSearch(new PersonalDTO() { 
               ProveedorDTO = new ProveedorDTO() { UsuarioId = model.UsuarioId }
            });

            personalSolicitudAccesoProxy.Insert(new PersonalSolicitudAccesoDTO()
            {
               PersonalId = personalDTO.Id,
               FechaRegistro = DateTime.Now,
               HoraSolicitada = model.HoraSolicitada,
               HoraAutorizada = string.Empty,
               EstadoAprobacionId = estadoAprobacionDTO.Id,
               UsuarioCreacionId = model.UsuarioId,
               FechaCreacion = DateTime.Now,
            });

            resultResponse.Url = redirectToLocal(string.Empty);

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