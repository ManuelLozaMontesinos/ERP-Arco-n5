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
   public class TipoRemuneraController : AuthorizedController
   {
      private readonly ITipoRemuneracionProxy tipoRemuneracionProxy;
      private readonly ITipoVariableCalculoProxy tipoVariableCalculoProxy;
      private readonly ITipoDatoProxy tipoDatoProxy;
      public TipoRemuneraController(
         ITipoRemuneracionProxy tipoRemuneracionProxy,
         ITipoVariableCalculoProxy tipoVariableCalculoProxy,
         ITipoDatoProxy tipoDatoProxy
         )
      {
         this.tipoRemuneracionProxy = tipoRemuneracionProxy;
         this.tipoVariableCalculoProxy = tipoVariableCalculoProxy;
         this.tipoDatoProxy = tipoDatoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         TipoRemuneraViewModel tipoRemuneraViewModel = new();

         return PartialView(tipoRemuneraViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         TipoRemuneraViewModel model = JsonSerializer.Deserialize<TipoRemuneraViewModel>(jsonModel);

         model.TipoRemuneracionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.TipoRemuneracionDTO.OrderBy = "tr.nombre";

         try
         {
            List<TipoRemuneracionDTO> tipoRemuneracionDTOList = tipoRemuneracionProxy.Search(model.TipoRemuneracionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = tipoRemuneracionDTOList.Count,
               rows = from f in tipoRemuneracionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.TipoRemuneracionId,
                            f.Nombre,
                            Variable = f.VariableCalculoDTO.Codigo,
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
      public ActionResult Crear([FromBody] TipoRemuneraViewModel model)
      {
         Logger.InfoStart();

         TipoRemuneraViewModel tipoRemuneraViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(tipoRemuneraViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, tipoRemuneraViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] TipoRemuneraViewModel model)
      {

         Logger.InfoStart();

         TipoRemuneraViewModel tipoRemuneraViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(tipoRemuneraViewModel, model.TipoRemuneracionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, tipoRemuneraViewModel);
      }
      private void cargarDataParaEditar(TipoRemuneraViewModel model, int id)
      {

         TipoRemuneracionDTO tipoRemuneracionDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            tipoRemuneracionDTO = new TipoRemuneracionDTO()
            {
               VariableCalculoDTO = new VariableCalculoDTO()
            };
         }
         else
         {
            tipoRemuneracionDTO = tipoRemuneracionProxy.GetById(new TipoRemuneracionDTO() { TipoRemuneracionId = id });
         }

         model.TipoRemuneracionDTO = tipoRemuneracionDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] TipoRemuneraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TipoRemuneracionDTO tipoRemuneracionDTO = cargarTipoRemuneracionDTO(model);

            int newId = model.TipoRemuneracionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = tipoRemuneracionProxy.Insert(tipoRemuneracionDTO);
                  messageResourceKey = "RH_tipo_remunera_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  tipoRemuneracionProxy.Update(tipoRemuneracionDTO);
                  messageResourceKey = "RH_tipo_remunera_actualizado";
                  break;
            }

            tipoRemuneracionDTO = tipoRemuneracionProxy.GetById(new TipoRemuneracionDTO() { TipoRemuneracionId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, tipoRemuneracionDTO.Nombre);
            resultResponse.Data = new
            {
               tipoRemuneracionDTO.Id
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
      private TipoRemuneracionDTO cargarTipoRemuneracionDTO(TipoRemuneraViewModel model)
      {

         TipoRemuneracionDTO tipoRemuneracionDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               tipoRemuneracionDTO = model.TipoRemuneracionDTO;

               TipoVariableCalculoDTO tipoVariableCalculoDTO = tipoVariableCalculoProxy.ObtenerPorCodigo(new TipoVariableCalculoDTO()
               {
                  Codigo = TipoVariableCalculoConstante.CAMPO_GENERAL 
               });

               tipoRemuneracionDTO.VariableCalculoDTO.TipoVariableCalculoId = tipoVariableCalculoDTO.Id;

               TipoDatoDTO tipoDatoDTO = tipoDatoProxy.ObtenerPorCodigo(new TipoDatoDTO()
               {
                  Codigo = TipoDatoConstante.CADENA
               });
               tipoRemuneracionDTO.VariableCalculoDTO.TipoDatoId = tipoDatoDTO.Id;
            }
            else
            {
               tipoRemuneracionDTO = tipoRemuneracionProxy.GetById(new TipoRemuneracionDTO() { TipoRemuneracionId = model.TipoRemuneracionDTO.Id });

               model.TipoRemuneracionDTO.VariableCalculoId = tipoRemuneracionDTO.VariableCalculoId;
               model.TipoRemuneracionDTO.VariableCalculoDTO.VariableCalculoId = tipoRemuneracionDTO.VariableCalculoDTO.VariableCalculoId;
               model.TipoRemuneracionDTO.VariableCalculoDTO.TipoVariableCalculoId = tipoRemuneracionDTO.VariableCalculoDTO.TipoVariableCalculoId;
               model.TipoRemuneracionDTO.VariableCalculoDTO.TipoDatoId = tipoRemuneracionDTO.VariableCalculoDTO.TipoDatoId;
               tipoRemuneracionDTO = model.TipoRemuneracionDTO;
            }

            tipoRemuneracionDTO.VariableCalculoDTO.Condicion = string.Empty;
            tipoRemuneracionDTO.VariableCalculoDTO.ValorCadenaFalso = string.Empty;
            tipoRemuneracionDTO.VariableCalculoDTO.TablaOrigen = string.Empty;
            tipoRemuneracionDTO.VariableCalculoDTO.TablaCondicion = string.Empty;
            tipoRemuneracionDTO.VariableCalculoDTO.TablaAgrupacion = string.Empty;
            tipoRemuneracionDTO.VariableCalculoDTO.EsSubconsulta = SiNoConstante.N;
            tipoRemuneracionDTO.VariableCalculoDTO.SubConsultaAlias = string.Empty;
            tipoRemuneracionDTO.VariableCalculoDTO.SubConsultaTipo = LetraConstante.I;
            tipoRemuneracionDTO.VariableCalculoDTO.SubConsultaColumna = string.Empty;
            tipoRemuneracionDTO.VariableCalculoDTO.SubConsultaEnlace = string.Empty;
            tipoRemuneracionDTO.VariableCalculoDTO.AlcanceFormulaHora = SiNoConstante.N;
            tipoRemuneracionDTO.VariableCalculoDTO.AlcanceFormula = SiNoConstante.S;
            tipoRemuneracionDTO.VariableCalculoDTO.AlcanceCondicion = SiNoConstante.N;

            tipoRemuneracionDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(tipoRemuneracionDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return tipoRemuneracionDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            TipoRemuneracionDTO tipoRemuneracionDTO = tipoRemuneracionProxy.GetById(new TipoRemuneracionDTO() { TipoRemuneracionId = id });

            AuditDTO(tipoRemuneracionDTO);

            tipoRemuneracionProxy.Delete(tipoRemuneracionDTO);

            resultResponse.Message = SR.GetString("RH_tipo_remunera_anulado", tipoRemuneracionDTO.Nombre);
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
