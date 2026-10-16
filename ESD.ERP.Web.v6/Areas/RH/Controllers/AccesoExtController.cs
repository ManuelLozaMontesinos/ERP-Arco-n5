using ESD.Core;
using ESD.Core.Logging;
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
   public class AccesoExtController : AuthorizedController
   {
      private readonly IEstadoAprobacionProxy estadoAprobacionProxy;
      private readonly IPersonalSolicitudAccesoProxy personalSolicitudAccesoProxy;
      private readonly IPersonalProxy personalProxy;

      public AccesoExtController(
         IEstadoAprobacionProxy estadoAprobacionProxy,
         IPersonalSolicitudAccesoProxy personalSolicitudAccesoProxy,
         IPersonalProxy personalProxy
         )
      {
         this.estadoAprobacionProxy = estadoAprobacionProxy;
         this.personalSolicitudAccesoProxy = personalSolicitudAccesoProxy;
         this.personalProxy = personalProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         AccesoExtViewModel accesoExtViewModel = new AccesoExtViewModel();

         cargarListaEstadoAprobacion(accesoExtViewModel);

         accesoExtViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key="aprobar", Content="Aprobar" },
            new MenuItem() { Key="desaprobar", Content="Desaprobar" }
         };

         EstadoAprobacionDTO estadoAprobacionDTO = estadoAprobacionProxy.ObtenerPorCodigo(
            new EstadoAprobacionDTO() { Codigo = EstadoAprobacionConstante.PENDIENTE_APROBACION });

         WebHelper.SelectedListItemSetValue(accesoExtViewModel.EstadoAprobacionList, estadoAprobacionDTO.Id);

         return PartialView(accesoExtViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         AccesoExtViewModel model = JsonSerializer.Deserialize<AccesoExtViewModel>(jsonModel);

         model.PersonalSolicitudAccesoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PersonalSolicitudAccesoDTO.OrderBy = "psa.fecha_registro desc";

         try
         {
            List<PersonalSolicitudAccesoDTO> personalSolicitudAccesoDTOList = personalSolicitudAccesoProxy.Search(model.PersonalSolicitudAccesoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = personalSolicitudAccesoDTOList.Count,
               rows = from f in personalSolicitudAccesoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PersonalSolicitudAccesoId,
                            Solicitado = f.PersonalDTO.ProveedorDTO.RazonSocial,
                            Fecha = string.Format(StringFormatConstant.DATE_TIME, f.FechaRegistro),
                            f.HoraSolicitada,
                            Autorizado = f?.PersonalAutorizaDTO?.ProveedorDTO?.RazonSocial,
                            f.HoraAutorizada,
                            FechaAutoriza = f.FechaAutoriza == null ? string.Empty : string.Format(StringFormatConstant.DATE_TIME,f.FechaAutoriza.Value),
                            EstadoAprobacion = f.EstadoAprobacionDTO.Codigo,
                            EstadoAprobacionNombre = f.EstadoAprobacionDTO.Nombre,
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
      private void cargarListaEstadoAprobacion(AccesoExtViewModel model)
      {
         List<EstadoAprobacionDTO> estadoAprobacionDTOList = estadoAprobacionProxy.Search(new EstadoAprobacionDTO() { Estado = EstadoConstante.ACTIVO });
         model.EstadoAprobacionList = WebHelper.ToSelectListItem(estadoAprobacionDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("AprobarEditPopup")]
      public ActionResult AprobarEditPopup([FromBody] AccesoExtViewModel model)
      {
         try
         {
            PersonalSolicitudAccesoDTO personalSolicitudAccesoDTO = personalSolicitudAccesoProxy.GetById(new PersonalSolicitudAccesoDTO() {
               PersonalSolicitudAccesoId = model.PersonalSolicitudAccesoDTO.Id });

            personalSolicitudAccesoDTO.HoraAutorizada = personalSolicitudAccesoDTO.HoraSolicitada;

            model.PersonalSolicitudAccesoDTO = personalSolicitudAccesoDTO;

         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(model);
      }
      [HttpPost("Aprobar")]
      public ActionResult Aprobar([FromBody] AccesoExtViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            PersonalDTO personalDTO = personalProxy.SingleSearch(new PersonalDTO()
            {
               ProveedorDTO = new ProveedorDTO() { UsuarioId = ObtenerUsuarioId() }
            });

            model.PersonalSolicitudAccesoDTO.PersonalAutorizaId = personalDTO.Id;
            AuditDTO(model.PersonalSolicitudAccesoDTO);

            personalSolicitudAccesoProxy.Aprobar(model.PersonalSolicitudAccesoDTO);

            resultResponse.Message = SR.GetString("RH_personal_solicitud_acceso_aprobada");
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
      [HttpPost("Desaprobar")]
      public ActionResult Desaprobar([FromBody] AccesoExtViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PersonalDTO personalDTO = personalProxy.SingleSearch(new PersonalDTO()
            {
               ProveedorDTO = new ProveedorDTO() { UsuarioId = ObtenerUsuarioId() }
            });

            model.PersonalSolicitudAccesoDTO.PersonalAutorizaId = personalDTO.Id;
            AuditDTO(model.PersonalSolicitudAccesoDTO);

            personalSolicitudAccesoProxy.Desaprobar(model.PersonalSolicitudAccesoDTO);

            resultResponse.Message = SR.GetString("RH_personal_solicitud_acceso_desaprobada");
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