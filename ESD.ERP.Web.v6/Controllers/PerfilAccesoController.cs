using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.Web.Model;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using ESD.Core;

namespace ESD.ERP.Web.Controllers
{
   public class PerfilAccesoController : AuthorizedController
   {
      private readonly IPeriodoProxy periodoProxy;
      private readonly IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy;
      private readonly IDirectivaUsuarioProxy directivaUsuarioProxy;
      private readonly INotificacionPushProxy notificacionPushProxy;

      public PerfilAccesoController(
         IPeriodoProxy periodoProxy,
         IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy,
         IDirectivaUsuarioProxy directivaUsuarioProxy,
         INotificacionPushProxy notificacionPushProxy
         )
      {
         this.periodoProxy = periodoProxy;
         this.periodoEmpresaSedeProxy = periodoEmpresaSedeProxy;
         this.directivaUsuarioProxy = directivaUsuarioProxy;
         this.notificacionPushProxy = notificacionPushProxy;
      }
      public IActionResult Index()
      {
         PerfilAccesoViewModel perfilAccesoViewModel = new();

         cargarListas(perfilAccesoViewModel);

         return View(perfilAccesoViewModel);
      }
      private void cargarListas(PerfilAccesoViewModel model)
      {
         List<PeriodoDTO> periodoDTOList = periodoProxy.Search(new PeriodoDTO()
         {
            OrderBy = "p.periodo desc",
            Estado = EstadoConstante.ACTIVO
         });

         model.PeriodoList = WebHelper.ToSelectListItem(periodoDTOList,
            x => x.Id.ToString(), x => x.Codigo.ToString(),SelectListFirstOptionType.Empty);

         List<EmpresaDTO> empresaDTOList = obtenerListaEmpresa(periodoDTOList[0].PeriodoId);
         model.EmpresaList = WebHelper.ToSelectListItem(empresaDTOList, x => x.Id.ToString(),
            x => x.Nombre, SelectListFirstOptionType.Select, empresaDTOList[0].EmpresaId.ToString());

         List<PeriodoEmpresaSedeDTO> periodoEmpresaSedeDTOList = obtenerListaPeriodoEmpresaSede(periodoDTOList[0].Id,
            empresaDTOList[0].Id);
         model.PeriodoEmpresaSedeList = WebHelper.ToSelectListItem(periodoEmpresaSedeDTOList, x => x.Id.ToString(),
            x => x.EmpresaSedeDTO.Nombre, SelectListFirstOptionType.Select, periodoEmpresaSedeDTOList[0].Id.ToString());

      }
      [HttpGet("ListarEmpresa")]
      public JsonResult ListarEmpresa(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            List<EmpresaDTO> empresaDTOList = obtenerListaEmpresa(id);

            resultResponse.Data = WebHelper.ToSelectListItem(empresaDTOList, x => x.Id.ToString(), 
               x => x.Nombre, SelectListFirstOptionType.Select);

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
      private List<EmpresaDTO> obtenerListaEmpresa(int periodoId)
      {
         List<EmpresaDTO> empresaDTOList = periodoEmpresaSedeProxy.ObtenerEmpresaList(new PeriodoEmpresaSedeDTO()
         {
            PeriodoId = periodoId,
            Estado = EstadoConstante.ACTIVO
         });

         return empresaDTOList;
      }
      [HttpGet("ListarPeriodoEmpresaSede")]
      public JsonResult ListarPeriodoEmpresaSede(int periodoId, int empresaId)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            List<PeriodoEmpresaSedeDTO> periodoEmpresaSedeDTOList = obtenerListaPeriodoEmpresaSede(periodoId, empresaId);

            resultResponse.Data = WebHelper.ToSelectListItem(periodoEmpresaSedeDTOList, x => x.Id.ToString(),
               x => x.EmpresaSedeDTO.Nombre, SelectListFirstOptionType.Select);

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
      private List<PeriodoEmpresaSedeDTO> obtenerListaPeriodoEmpresaSede(int periodoId, int empresaId)
      {
         List<int> valorList = null;
         if (!EsAdministrador())
         {
            List<DirectivaUsuarioDTO> directivaUsuarioDTOList = directivaUsuarioProxy.Search(new DirectivaUsuarioDTO() {
               UsuarioId = ObtenerUsuarioId(),
               DirectivaReglaDTO = new DirectivaReglaDTO() { Codigo = DirectivaReglaConstante.ASIGNACION_DE_SEDES }
            });

            valorList = directivaUsuarioDTOList.Select(x => x.Valor).ToList();
         }

         List<PeriodoEmpresaSedeDTO> periodoEmpresaSedeDTOList = periodoEmpresaSedeProxy.Search(new PeriodoEmpresaSedeDTO()
         {
            PeriodoId = periodoId,
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = empresaId},
            EmpresaSedeIdList = valorList,
            Estado = EstadoConstante.ACTIVO
         });

         return periodoEmpresaSedeDTOList;
      }
      [HttpPost("Grabar")]
      public IActionResult Grabar([FromBody] PerfilAccesoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            
            //resultResponse.Url = Url.Action(nameof(HomeController.Index), "Home", new { id = model.PeriodoEmpresaSedeId });
            resultResponse.Url = "Home?cid=" + CreateCID(
               string.Concat(model.PeriodoId.ToString(),",",
                  model.EmpresaId.ToString(),",",
                  model.PeriodoEmpresaSedeId.ToString())
               );
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
      [HttpGet("ListarNotificaciones")]
      public IActionResult ListarNotificaciones()
      {
         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            List<NotificacionPushDTO> notificacionPushDTOList = notificacionPushProxy.Search(new NotificacionPushDTO()
            {
               OrderBy = "fecha_reg desc",
               Fecha = DateTime.Today,
               UsuarioId = ObtenerUsuarioId()
            });

            resultResponse.Data = new
            {
               Total = notificacionPushDTOList.Count,
               List = from f in notificacionPushDTOList.AsEnumerable()
                      select new
                      {
                         f.NotificacionPushId,
                         f.Titulo,
                         f.Descripcion,
                         Fecha = string.Format(StringFormatConstant.DATE_TIME,f.FechaRegistro)
                      }
            };
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
   }
}