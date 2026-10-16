using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service;
using ESD.Core.Service.Security;
using ESD.ERP.Business;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ESD.ERP.WebApi.Controllers
{
   [Produces("application/json")]
   [Route("api/Seguridad")]
   public class SeguridadController : AuthorizedController
   {
      private readonly ITokenGenerator tokenGenerator;
      private readonly IUsuarioBusiness usuarioBusiness;
      private readonly IUsuarioHorarioBusiness usuarioHorarioBusiness;
      private readonly IPersonalSolicitudAccesoBusiness personalSolicitudAccesoBusiness;

      public SeguridadController(
         ITokenGenerator tokenGenerator,
         IUsuarioBusiness usuarioBusiness,
         IUsuarioHorarioBusiness usuarioHorarioBusiness,
         IPersonalSolicitudAccesoBusiness personalSolicitudAccesoBusiness
         )
      {
         this.tokenGenerator = tokenGenerator;
         this.usuarioBusiness = usuarioBusiness;
         this.usuarioHorarioBusiness = usuarioHorarioBusiness;
         this.personalSolicitudAccesoBusiness = personalSolicitudAccesoBusiness;
      }

      [AllowAnonymous]
      [HttpPost("Autenticar")]
      public ActionResult<ServiceResponse> Autenticar([FromBody] UsuarioDTO dto)
      {
         Logger.InfoStart();
         ServiceResponse serviceResponse = new();

         try
         {
            UsuarioDTO usuarioDTO = usuarioBusiness.ObtenerPorCodigo(dto.UsuarioLogon);
            if (usuarioDTO == null)
               throw new Exception(SR.GetString("EX_usuario_contrasena_incorrecto"));

            if(usuarioDTO.NoExpira == SiNoConstante.N)
            {
               if(DateTime.Today > usuarioDTO.FechaExpiracion.Value)
                  throw new Exception(SR.GetString("EX_usuario_fecha_expirada"));
            }

            if (usuarioDTO.Estado == EstadoConstante.INACTIVO)
               throw new Exception(SR.GetString("EX_usuario_inhabilitado"));

            dto.UsuarioId = usuarioDTO.Id;
            if (!usuarioBusiness.ValidarContrasena(dto))
               throw new Exception(SR.GetString("EX_usuario_contrasena_incorrecto"));

            string token = tokenGenerator.GenerateTokenJwt(usuarioDTO.Id.ToString(), usuarioDTO.UsuarioLogon);
            usuarioDTO.CodigoHorarioExtendido = DigitoConstante.CERO;

            if (usuarioDTO.TieneHorario == SiNoConstante.S)
            {
               string[] horarioResult = calcularRequiereHorarioExtendido(usuarioDTO.Id);
               usuarioDTO.CodigoHorarioExtendido = horarioResult[0];
               usuarioDTO.MensajeHorarioExtendido = horarioResult[1];
            }

            serviceResponse.Data = new ServiceResponse() { Data = usuarioDTO, Token = token };
         }
         catch (Exception e)
         {
            Logger.Error(e);
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
         }

         return serviceResponse;
      }
      private string[] calcularRequiereHorarioExtendido(int usuarioId)
      {

         string inicio = string.Empty;
         string fin = string.Empty;
         int dia = (int)DateTime.Today.DayOfWeek;
         string[] horarioResult = new string[] { DigitoConstante.CERO, string.Empty };

         if (dia == 0)
            dia = 7;

         UsuarioHorarioDTO usuarioHorarioDTO = usuarioHorarioBusiness.SingleSearch(new UsuarioHorarioDTO()
         {
            UsuarioId = usuarioId,
            DiaDTO = new DiaDTO() { Codigo = dia.ToString() },
            TieneHorarioAsignado = SiNoConstante.S
         });

         bool esValido = false;

         DateTime horaInicio;
         if (usuarioHorarioDTO != null)
         {
            inicio = usuarioHorarioDTO.HoraInicio;
            fin = usuarioHorarioDTO.HoraFin;

            horaInicio = DateTime.Parse(string.Format(StringFormatConstant.DATE, DateTime.Today) + SeparadorConstante.ESPACIO +
                        string.Concat(inicio, ":00"));

            esValido = esHorarioValido(horaInicio, fin);
         }

         if (esValido)
            return horarioResult;

         PersonalSolicitudAccesoDTO personalSolicitudAccesoDTO = personalSolicitudAccesoBusiness.SingleSearch(new PersonalSolicitudAccesoDTO()
         {
            OrderBy = "psa.fecha_registro desc",
            FechaDesde = DateTime.Today,
            FechaHasta = DateTime.Today,
            PersonalDTO = new PersonalDTO()
            {
               ProveedorDTO = new ProveedorDTO() { UsuarioId = usuarioId }
            }
         });

         string horaExtension = string.Empty;
         if (personalSolicitudAccesoDTO != null)
         {
            string estadoAprobacion = personalSolicitudAccesoDTO.EstadoAprobacionDTO.Codigo;
            horaInicio = DateTime.Now;

            switch (estadoAprobacion)
            {
               case EstadoAprobacionConstante.PENDIENTE_APROBACION:
                  horarioResult = new string[] { DigitoConstante.DOS, SR.GetString("EX_personal_solicitud_acceso_pendiente_aprobacion") };
                  break;

               case EstadoAprobacionConstante.DESAPROBADO:
                  horarioResult = new string[] { DigitoConstante.TRES, SR.GetString("EX_personal_solicitud_acceso_desaprobado") };
                  break;

               default:
                  horaExtension = personalSolicitudAccesoDTO.HoraAutorizada;
                  esValido = esHorarioValido(horaInicio, horaExtension);
                  if(!esValido)
                     horarioResult = new string[] { DigitoConstante.NUEVE, SR.GetString("EX_usuario_horario_no_permitido") };
                  break;

            }
         }
         else
         {
            horarioResult = new string[] { DigitoConstante.NUEVE, SR.GetString("EX_usuario_horario_no_permitido") };
         }

         return horarioResult;
      }
      private bool esHorarioValido(DateTime horaInicio, string fin)
      {
         DateTime hora = DateTime.Now;

         DateTime horaFin = DateTime.Parse(string.Format(StringFormatConstant.DATE, DateTime.Today) + SeparadorConstante.ESPACIO +
            string.Concat(fin, ":00"));

         if (!(horaInicio <= hora && hora <= horaFin))
            return false;

         return true;
      }
      [AllowAnonymous]
      [HttpGet("TestStatus")]
      public string TestStatus()
      {
         return "Status: " + DateTime.Now.ToString();
      }
   }
}