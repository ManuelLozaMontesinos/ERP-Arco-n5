using ESD.Core;
using ESD.Core.Cryptography;
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