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
   public class VariableCalcController : AuthorizedController
   {
      private readonly IVariableCalculoProxy variableCalculoProxy;
      private readonly ITipoDatoProxy tipoDatoProxy;
      private readonly ITipoVariableCalculoProxy tipoVariableCalculoProxy;
      public VariableCalcController(
         IVariableCalculoProxy variableCalculoProxy,
         ITipoDatoProxy tipoDatoProxy,
         ITipoVariableCalculoProxy tipoVariableCalculoProxy
         )
      {
         this.variableCalculoProxy = variableCalculoProxy;
         this.tipoDatoProxy = tipoDatoProxy;
         this.tipoVariableCalculoProxy = tipoVariableCalculoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         VariableCalcViewModel variableCalcViewModel = new();

         return PartialView(variableCalcViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         VariableCalcViewModel model = JsonSerializer.Deserialize<VariableCalcViewModel>(jsonModel);

         model.VariableCalculoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.VariableCalculoDTO.OrderBy = "vc.nombre";

         try
         {
            List<VariableCalculoDTO> variableCalculoDTOList = variableCalculoProxy.Search(model.VariableCalculoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = variableCalculoDTOList.Count,
               rows = from f in variableCalculoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.VariableCalculoId,
                            Codigo = string.Concat("{",f.Codigo,"}"),
                            f.Nombre,
                            TipoDatoNombre = f.TipoDatoDTO.Nombre,
                            Valor = buildStringValue(f),
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
      private string buildStringValue(VariableCalculoDTO variableCalculoDTO) {

         if(variableCalculoDTO.TipoVariableCalculoDTO.Codigo != TipoVariableCalculoConstante.VARIABLE)
            return string.Empty;

         string newValue = string.Empty;

         switch (variableCalculoDTO.TipoDatoDTO.Codigo) {
            case TipoDatoConstante.CADENA:
               newValue = variableCalculoDTO.ValorCadenaVerdad;
               break;

            case TipoDatoConstante.DECIMAL:
               newValue = variableCalculoDTO.ValorNumericoVerdad.ToString();
               break;

            case TipoDatoConstante.ENTERO:
               newValue = Convert.ToInt32(variableCalculoDTO.ValorNumericoVerdad).ToString();
               break;

         }

         return newValue;
      }
      [HttpPost("Crear")]
      public ActionResult Crear([FromBody] VariableCalcViewModel model)
      {
         Logger.InfoStart();

         VariableCalcViewModel variableCalcViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(variableCalcViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, variableCalcViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] VariableCalcViewModel model)
      {

         Logger.InfoStart();

         VariableCalcViewModel variableCalcViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(variableCalcViewModel, model.VariableCalculoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, variableCalcViewModel);
      }
      private void cargarDataParaEditar(VariableCalcViewModel model, int id)
      {

         VariableCalculoDTO variableCalculoDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            variableCalculoDTO = new VariableCalculoDTO();

            TipoDatoDTO tipoDatoDTO = tipoDatoProxy.ObtenerPorCodigo(new TipoDatoDTO() { Codigo = TipoDatoConstante.DECIMAL });
            variableCalculoDTO.TipoDatoId = tipoDatoDTO.Id;
            model.TipoDato = tipoDatoDTO.Codigo;
            model.TieneCondicion = SiNoConstante.N;
         }
         else
         {
            variableCalculoDTO = variableCalculoProxy.GetById(new VariableCalculoDTO() { VariableCalculoId = id });
            model.TipoDato = variableCalculoDTO.TipoDatoDTO.Codigo;
            model.TieneCondicion = string.IsNullOrEmpty(variableCalculoDTO.Condicion) ? SiNoConstante.N : SiNoConstante.S;
         }

         model.VariableCalculoDTO = variableCalculoDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.TipoDatoList, variableCalculoDTO.TipoDatoId);
      }
      private void cargarListas(VariableCalcViewModel model)
      {

         List<TipoDatoDTO> tipoDatoDTOList = tipoDatoProxy.Search(new TipoDatoDTO() 
         { 
            TipoDatoList = new List<string>() { TipoDatoConstante.CADENA,TipoDatoConstante.DECIMAL,TipoDatoConstante.ENTERO},
            Estado = EstadoConstante.ACTIVO
         });
         model.TipoDatoList = WebHelper.ToSelectListItem(tipoDatoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpGet("ObtenerTipoDato")]
      public JsonResult ObtenerTipoDato(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            TipoDatoDTO tipoDatoDTO = tipoDatoProxy.GetById(new TipoDatoDTO() { TipoDatoId = id });

            resultResponse.Data = new
            {
               tipoDatoDTO.Codigo
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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] VariableCalcViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            VariableCalculoDTO variableCalculoDTO = cargarVariableCalculoDTO(model);

            int newId = model.VariableCalculoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = variableCalculoProxy.Insert(variableCalculoDTO);
                  messageResourceKey = "RH_variable_calculo_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  variableCalculoProxy.Update(variableCalculoDTO);
                  messageResourceKey = "RH_variable_calculo_actualizada";
                  break;
            }

            variableCalculoDTO = variableCalculoProxy.GetById(new VariableCalculoDTO() { VariableCalculoId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, variableCalculoDTO.Codigo);
            resultResponse.Data = new
            {
               variableCalculoDTO.Id
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
      private VariableCalculoDTO cargarVariableCalculoDTO(VariableCalcViewModel model)
      {

         VariableCalculoDTO variableCalculoDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               variableCalculoDTO = model.VariableCalculoDTO;

               TipoVariableCalculoDTO tipoVariableCalculoDTO = tipoVariableCalculoProxy.ObtenerPorCodigo(new TipoVariableCalculoDTO() 
                  { Codigo = TipoVariableCalculoConstante.VARIABLE });
               variableCalculoDTO.TipoVariableCalculoId = tipoVariableCalculoDTO.Id;

            }
            else
            {
               variableCalculoDTO = variableCalculoProxy.GetById(new VariableCalculoDTO() { VariableCalculoId = model.VariableCalculoDTO.Id });

               variableCalculoDTO = model.VariableCalculoDTO;
            }

            if (string.IsNullOrEmpty(variableCalculoDTO.Condicion))
               variableCalculoDTO.Condicion = string.Empty;

            if (string.IsNullOrEmpty(variableCalculoDTO.TablaOrigen))
               variableCalculoDTO.TablaOrigen = string.Empty;

            if (string.IsNullOrEmpty(variableCalculoDTO.TablaCondicion))
               variableCalculoDTO.TablaCondicion = string.Empty;

            if (string.IsNullOrEmpty(variableCalculoDTO.TablaAgrupacion))
               variableCalculoDTO.TablaAgrupacion = string.Empty;

            if (string.IsNullOrEmpty(variableCalculoDTO.EsSubconsulta))
               variableCalculoDTO.EsSubconsulta = SiNoConstante.N;

            if (string.IsNullOrEmpty(variableCalculoDTO.SubConsultaAlias))
               variableCalculoDTO.SubConsultaAlias = string.Empty;

            if (string.IsNullOrEmpty(variableCalculoDTO.SubConsultaTipo))
               variableCalculoDTO.SubConsultaTipo = LetraConstante.I;

            if (string.IsNullOrEmpty(variableCalculoDTO.SubConsultaColumna))
               variableCalculoDTO.SubConsultaColumna = string.Empty;

            if (string.IsNullOrEmpty(variableCalculoDTO.SubConsultaEnlace))
               variableCalculoDTO.SubConsultaEnlace = string.Empty;

            if (string.IsNullOrEmpty(variableCalculoDTO.AlcanceFormulaHora))
               variableCalculoDTO.AlcanceFormulaHora = SiNoConstante.N;

            if (string.IsNullOrEmpty(variableCalculoDTO.AlcanceFormula))
               variableCalculoDTO.AlcanceFormula = SiNoConstante.S;

            if (string.IsNullOrEmpty(variableCalculoDTO.AlcanceCondicion))
               variableCalculoDTO.AlcanceCondicion = SiNoConstante.N;

            variableCalculoDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(variableCalculoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return variableCalculoDTO;
      }
      [HttpPost("ListPopup")]
      public IActionResult ListPopup([FromBody] VariableCalcViewModel model)
      {
         return PartialView(model);
      }

   }
}
