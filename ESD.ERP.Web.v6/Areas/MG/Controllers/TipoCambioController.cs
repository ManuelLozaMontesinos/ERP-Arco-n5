using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.MG.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.MG.Controllers
{
   [Area("MG")]
   [Route("[controller]")]
   public class TipoCambioController : AuthorizedController
   {
      private readonly ITipoCambioProxy tipoCambioProxy;
      private readonly ITipoCambioDiaProxy tipoCambioDiaProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IOrigenRegistroProxy origenRegistroProxy;

      public TipoCambioController(
         ITipoCambioProxy tipoCambioProxy,
         ITipoCambioDiaProxy tipoCambioDiaProxy,
         IMonedaProxy monedaProxy,
         IOrigenRegistroProxy origenRegistroProxy
         )
      {
         this.tipoCambioProxy = tipoCambioProxy;
         this.tipoCambioDiaProxy = tipoCambioDiaProxy;
         this.monedaProxy = monedaProxy;
         this.origenRegistroProxy = origenRegistroProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         TipoCambioViewModel tipoCambioViewModel = new();

         return PartialView(tipoCambioViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         TipoCambioViewModel model = new()
         {
            TipoCambioDTO = new TipoCambioDTO()
         };

         model.TipoCambioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.TipoCambioDTO.OrderBy = "nombre";

         try
         {
            List<TipoCambioDTO> tipoCambioDTOList = tipoCambioProxy.Search(model.TipoCambioDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = tipoCambioDTOList.Count,
               rows = from f in tipoCambioDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.Codigo,
                            f.TipoCambioId,
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
      public ActionResult Crear([FromBody] TipoCambioViewModel model)
      {
         Logger.InfoStart();

         TipoCambioViewModel tipoCambioViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(tipoCambioViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, tipoCambioViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] TipoCambioViewModel model)
      {

         Logger.InfoStart();

         TipoCambioViewModel tipoCambioViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(tipoCambioViewModel, model.TipoCambioDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, tipoCambioViewModel);
      }
      private void cargarDataParaEditar(TipoCambioViewModel model, int id)
      {

         TipoCambioDTO tipoCambioDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            tipoCambioDTO = new TipoCambioDTO();
         }
         else
         {
            tipoCambioDTO = tipoCambioProxy.GetById(new TipoCambioDTO() { TipoCambioId = id });
         }

         model.TipoCambioDTO = tipoCambioDTO;

         cargarListas(model);
      }
      private void cargarListas(TipoCambioViewModel model)
      {
         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO() { 
            EsLocal = SiNoConstante.N,
            Estado = EstadoConstante.ACTIVO 
         });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
         model.MonedaString = string.Join(SeparadorConstante.PUNTO_COMA, monedaDTOList
            .Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] TipoCambioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TipoCambioDTO tipoCambioDTO = cargarTipoCambioDTO(model);

            int newId = model.TipoCambioDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = tipoCambioProxy.Insert(tipoCambioDTO);
                  messageResourceKey = "MG_tipo_cambio_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  tipoCambioProxy.Update(tipoCambioDTO);
                  messageResourceKey = "MG_tipo_cambio_actualizado";
                  break;
            }

            tipoCambioDTO = tipoCambioProxy.GetById(new TipoCambioDTO() { TipoCambioId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, tipoCambioDTO.Nombre);
            resultResponse.Data = new
            {
               tipoCambioDTO.Id
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
      private TipoCambioDTO cargarTipoCambioDTO(TipoCambioViewModel model)
      {

         TipoCambioDTO tipoCambioDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               tipoCambioDTO = model.TipoCambioDTO;

               OrigenRegistroDTO origenRegistroDTO = origenRegistroProxy.ObtenerPorCodigo(new OrigenRegistroDTO() { Codigo = OrigenRegistroConstante.USUARIO });

               tipoCambioDTO.OrigenRegistroId = origenRegistroDTO.Id;
            }
            else
            {
               tipoCambioDTO = tipoCambioProxy.GetById(new TipoCambioDTO() 
                  { TipoCambioId = model.TipoCambioDTO.Id });

               tipoCambioDTO = model.TipoCambioDTO;
            }

            tipoCambioDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(tipoCambioDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return tipoCambioDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TipoCambioDTO tipoCambioDTO = tipoCambioProxy
               .GetById(new TipoCambioDTO() { TipoCambioId = id });

            AuditDTO(tipoCambioDTO);

            tipoCambioProxy.Delete(tipoCambioDTO);

            resultResponse.Message = SR.GetString("MG_tipo_cambio_anulado");
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
      [HttpPost("ListarTipoCambioDia")]
      public ActionResult ListarTipoCambioDia(string jsonModel)
      {

         Logger.InfoStart();

         TipoCambioViewModel model = JsonSerializer.Deserialize<TipoCambioViewModel>(jsonModel);

         model.TipoCambioDiaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.TipoCambioDiaDTO.OrderBy = "tcd.fecha desc";
         model.TipoCambioDiaDTO.TipoCambioId = NumberUtil.WhenValueIsZero(model.TipoCambioDiaDTO.TipoCambioId);

         try
         {
            List<TipoCambioDiaDTO> tipoCambioDiaDTOList = tipoCambioDiaProxy.Search(model.TipoCambioDiaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = tipoCambioDiaDTOList.Count,
               rows = from f in tipoCambioDiaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.TipoCambioDiaId,
                            f.TipoCambioId,
                            f.MonedaId,
                            Fecha = string.Format(StringFormatConstant.DATE, f.Fecha),
                            f.MontoCompra,
                            f.MontoVenta,
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
      [HttpPost("GrabarTipoCambioDia")]
      public ActionResult GrabarTipoCambioDia([FromBody] TipoCambioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TipoCambioDiaDTO tipoCambioDiaDTO = cargarTipoCambioDiaDTO(model);

            int newId = model.TipoCambioDiaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (tipoCambioDiaDTO.DTOState)
            {
               case DTOState.Added:
                  newId = tipoCambioDiaProxy.Insert(tipoCambioDiaDTO);
                  messageResourceKey = "MG_tipo_cambio_agregado";
                  break;

               case DTOState.Modified:
                  tipoCambioDiaProxy.Update(tipoCambioDiaDTO);
                  messageResourceKey = "MG_tipo_cambio_actualizado";
                  break;
            }

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
      private TipoCambioDiaDTO cargarTipoCambioDiaDTO(TipoCambioViewModel model)
      {

         TipoCambioDiaDTO tipoCambioDiaDTO;
         try
         {

            if (model.TipoCambioDiaDTO.DTOState == DTOState.Added)
            {
               tipoCambioDiaDTO = model.TipoCambioDiaDTO;
            }
            else
            {
               tipoCambioDiaDTO = tipoCambioDiaProxy.GetById(new TipoCambioDiaDTO() { TipoCambioDiaId = model.TipoCambioDiaDTO.Id });
               tipoCambioDiaDTO = model.TipoCambioDiaDTO;
            }

            tipoCambioDiaDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(tipoCambioDiaDTO);
         }
         catch (Exception)
         {
            throw;
         }

         return tipoCambioDiaDTO;
      }
   }
}
