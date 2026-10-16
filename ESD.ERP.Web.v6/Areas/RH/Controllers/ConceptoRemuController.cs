using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
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
   public class ConceptoRemuController : AuthorizedController
   {
      private readonly IConceptoRemunerativoProxy conceptoRemunerativoProxy;
      private readonly ITipoConceptoRemunerativoProxy tipoConceptoRemunerativoProxy;
      private readonly IConceptoBaseImponibleProxy conceptoBaseImponibleProxy;
      private readonly IVariableCalculoProxy variableCalculoProxy;
      public ConceptoRemuController(
         IConceptoRemunerativoProxy conceptoRemunerativoProxy,
         ITipoConceptoRemunerativoProxy tipoConceptoRemunerativoProxy,
         IConceptoBaseImponibleProxy conceptoBaseImponibleProxy,
         IVariableCalculoProxy variableCalculoProxy
         )
      {
         this.conceptoRemunerativoProxy = conceptoRemunerativoProxy;
         this.tipoConceptoRemunerativoProxy = tipoConceptoRemunerativoProxy;
         this.conceptoBaseImponibleProxy = conceptoBaseImponibleProxy;
         this.variableCalculoProxy = variableCalculoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ConceptoRemuViewModel conceptoRemuViewModel = new();

         return PartialView(conceptoRemuViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ConceptoRemuViewModel model = JsonSerializer.Deserialize<ConceptoRemuViewModel>(jsonModel);

         model.ConceptoRemunerativoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ConceptoRemunerativoDTO.OrderBy = "cr.nombre";
         model.ConceptoRemunerativoDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);

         try
         {
            List<ConceptoRemunerativoDTO> conceptoRemunerativoDTOList = conceptoRemunerativoProxy.Search(model.ConceptoRemunerativoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = conceptoRemunerativoDTOList.Count,
               rows = from f in conceptoRemunerativoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ConceptoRemunerativoId,
                            f.Codigo,
                            f.Nombre,
                            f.UsaFormula,
                            TipoConceptoRemunerativo = f.TipoConceptoRemunerativoDTO.Nombre,
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
      public ActionResult Crear([FromBody] ConceptoRemuViewModel model)
      {
         Logger.InfoStart();

         ConceptoRemuViewModel conceptoRemuViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(conceptoRemuViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, conceptoRemuViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ConceptoRemuViewModel model)
      {

         Logger.InfoStart();

         ConceptoRemuViewModel conceptoRemuViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(conceptoRemuViewModel, model.ConceptoRemunerativoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, conceptoRemuViewModel);
      }
      private void cargarDataParaEditar(ConceptoRemuViewModel model, int id)
      {

         ConceptoRemunerativoDTO conceptoRemunerativoDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            conceptoRemunerativoDTO = new ConceptoRemunerativoDTO();
         }
         else
         {
            conceptoRemunerativoDTO = conceptoRemunerativoProxy.GetById(new ConceptoRemunerativoDTO() { ConceptoRemunerativoId = id });
         }
         conceptoRemunerativoDTO.EsRegularizacion = conceptoRemunerativoDTO.EsRegularizacion == SiNoConstante.S ? EstadoConstante.ACTIVO : EstadoConstante.INACTIVO;
         model.ConceptoRemunerativoDTO = conceptoRemunerativoDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.TipoConceptoRemunerativoList, conceptoRemunerativoDTO.TipoConceptoRemunerativoId);
      }
      private void cargarListas(ConceptoRemuViewModel model)
      {

         List<TipoConceptoRemunerativoDTO> tipoConceptoRemunerativoDTOList = tipoConceptoRemunerativoProxy.Search(new TipoConceptoRemunerativoDTO() 
         { 
            Estado = EstadoConstante.ACTIVO
         });
         model.TipoConceptoRemunerativoList = WebHelper.ToSelectListItem(tipoConceptoRemunerativoDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("VerificarFormulaHora")]
      public ActionResult VerificarFormulaHora([FromBody] ConceptoRemuViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            VariableCalculoDTO variableCalculoDTO = new() { 
               AlcanceFormulaHora = SiNoConstante.S,
               AlcanceFormula = SiNoConstante.N,
               AlcanceCondicion = SiNoConstante.N
            };
            List<VariableCalculoDTO> variableCalculoDTOList = variableCalculoProxy.Search(variableCalculoDTO);

            FormulaParser formulaParser = new()
            {
               VariableList = variableCalculoDTOList.Select(x => new FormulaVariable
               {
                  Key = "{" + x.Codigo.ToLower() + "}",
                  Type = x.TipoVariableCalculoDTO.Codigo
               }).ToList()
            };

            formulaParser.Parse(model.ConceptoRemunerativoDTO.Formula, string.Empty);

            resultResponse.Message = SR.GetString("RH_formula_correcta");
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
      [HttpPost("VerificarFormula")]
      public ActionResult VerificarFormula([FromBody] ConceptoRemuViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            VariableCalculoDTO variableCalculoDTO = new()
            {
               AlcanceFormulaHora = SiNoConstante.N,
               AlcanceFormula = SiNoConstante.S,
               AlcanceCondicion = SiNoConstante.N,
            };
            List<VariableCalculoDTO> variableCalculoDTOList = variableCalculoProxy.Search(variableCalculoDTO);

            FormulaParser formulaParser = new()
            {
               VariableList = variableCalculoDTOList.Select(x => new FormulaVariable
               {
                  Key = "{" + x.Codigo.ToLower() + "}",
                  Type = x.TipoVariableCalculoDTO.Codigo
               }).ToList()
            };

            formulaParser.Parse(model.ConceptoRemunerativoDTO.Formula, string.Empty);

            if (!string.IsNullOrEmpty(model.ConceptoRemunerativoDTO.Condicion)) {

               variableCalculoDTO = new VariableCalculoDTO()
               {
                  AlcanceFormulaHora = SiNoConstante.N,
                  AlcanceFormula = SiNoConstante.N,
                  AlcanceCondicion = SiNoConstante.S,
               };
               variableCalculoDTOList = variableCalculoProxy.Search(variableCalculoDTO);

               formulaParser = new FormulaParser
               {
                  VariableList = variableCalculoDTOList.Select(x => new FormulaVariable
                  {
                     Key = "{" + x.Codigo.ToLower() + "}",
                     Type = x.TipoVariableCalculoDTO.Codigo
                  }).ToList()
               };

               formulaParser.Parse(string.Empty, model.ConceptoRemunerativoDTO.Condicion);
            }

            resultResponse.Message = SR.GetString("RH_formula_correcta");
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
      public ActionResult Grabar([FromBody] ConceptoRemuViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ConceptoRemunerativoDTO conceptoRemunerativoDTO = cargarConceptoRemunerativoDTO(model);

            int newId = model.ConceptoRemunerativoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = conceptoRemunerativoProxy.Insert(conceptoRemunerativoDTO);
                  messageResourceKey = "RH_concepto_remunerativo_agregado";
                  break;

               case ViewModelEditAction.Edit:
                  conceptoRemunerativoProxy.Update(conceptoRemunerativoDTO);
                  messageResourceKey = "RH_concepto_remunerativo_actualizado";
                  break;
            }

            conceptoRemunerativoDTO = conceptoRemunerativoProxy.GetById(new ConceptoRemunerativoDTO() { ConceptoRemunerativoId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, conceptoRemunerativoDTO.Codigo);
            resultResponse.Data = new
            {
               conceptoRemunerativoDTO.Id,
               conceptoRemunerativoDTO.Codigo
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
      private ConceptoRemunerativoDTO cargarConceptoRemunerativoDTO(ConceptoRemuViewModel model)
      {

         ConceptoRemunerativoDTO conceptoRemunerativoDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               conceptoRemunerativoDTO = model.ConceptoRemunerativoDTO;
               conceptoRemunerativoDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
            }
            else
            {
               conceptoRemunerativoDTO = conceptoRemunerativoProxy.GetById(new ConceptoRemunerativoDTO() { ConceptoRemunerativoId = model.ConceptoRemunerativoDTO.Id });

               model.ConceptoRemunerativoDTO.UsaFormula = conceptoRemunerativoDTO.UsaFormula;

               conceptoRemunerativoDTO = model.ConceptoRemunerativoDTO;
            }

            VariableCalculoDTO variableCalculoDTO = new();
            List<VariableCalculoDTO> variableCalculoDTOList = variableCalculoProxy.Search(variableCalculoDTO);

            FormulaParser formulaParser = null;
            if (!string.IsNullOrEmpty(conceptoRemunerativoDTO.FormulaHora)) {
               formulaParser = new FormulaParser
               {
                  VariableList = variableCalculoDTOList.Where(x => x.AlcanceFormulaHora == SiNoConstante.S &&
                  x.AlcanceFormula == SiNoConstante.N && x.AlcanceCondicion==SiNoConstante.S).Select(x => new FormulaVariable
                  {
                     Key = "{" + x.Codigo.ToLower() + "}",
                     Type = x.TipoVariableCalculoDTO.Codigo
                  }).ToList()
               };

               formulaParser.Parse(conceptoRemunerativoDTO.FormulaHora, string.Empty);
            }

            if (!string.IsNullOrEmpty(conceptoRemunerativoDTO.Formula))
            {
               formulaParser = new FormulaParser
               {
                  VariableList = variableCalculoDTOList.Where(x => x.AlcanceFormulaHora == SiNoConstante.N &&
                  x.AlcanceFormula == SiNoConstante.S && x.AlcanceCondicion == SiNoConstante.N).Select(x => new FormulaVariable
                  {
                     Key = "{" + x.Codigo.ToLower() + "}",
                     Type = x.TipoVariableCalculoDTO.Codigo
                  }).ToList()
               };

               formulaParser.Parse(conceptoRemunerativoDTO.Formula, string.Empty);

               if (!string.IsNullOrEmpty(conceptoRemunerativoDTO.Condicion))
               {
                  formulaParser = new FormulaParser
                  {
                     VariableList = variableCalculoDTOList.Where(x => x.AlcanceCondicion == SiNoConstante.S).Select(x => new FormulaVariable
                     {
                        Key = "{" + x.Codigo.ToLower() + "}",
                        Type = x.TipoVariableCalculoDTO.Codigo
                     }).ToList()
                  };

                  formulaParser.Parse(string.Empty, conceptoRemunerativoDTO.Condicion);
               }
            }

            if (string.IsNullOrEmpty(conceptoRemunerativoDTO.UsaFormula))
               conceptoRemunerativoDTO.UsaFormula = SiNoConstante.S;

            if (string.IsNullOrEmpty(conceptoRemunerativoDTO.FormulaHora))
               conceptoRemunerativoDTO.FormulaHora = string.Empty;

            if (string.IsNullOrEmpty(conceptoRemunerativoDTO.Condicion))
               conceptoRemunerativoDTO.Condicion = string.Empty;

            conceptoRemunerativoDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(conceptoRemunerativoDTO);

            foreach (ConceptoBaseImponibleDTO conceptoBaseImponibleDTO in model.ConceptoBaseImponibleDTOList)
               AuditDTO(conceptoBaseImponibleDTO);

            conceptoRemunerativoDTO.ConceptoBaseImponibleDTOList = model.ConceptoBaseImponibleDTOList;

         }
         catch (Exception)
         {
            throw;
         }

         return conceptoRemunerativoDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ConceptoRemunerativoDTO conceptoRemunerativoDTO = conceptoRemunerativoProxy.GetById(new ConceptoRemunerativoDTO() { ConceptoRemunerativoId = id });

            AuditDTO(conceptoRemunerativoDTO);

            conceptoRemunerativoProxy.Delete(conceptoRemunerativoDTO);

            resultResponse.Message = SR.GetString("RH_concepto_remunerativo_anulado", conceptoRemunerativoDTO.Nombre);
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
      [HttpPost("ListarConceptoBaseImponible")]
      public ActionResult ListarConceptoBaseImponible(string jsonModel)
      {

         Logger.InfoStart();

         ConceptoRemuViewModel model = JsonSerializer.Deserialize<ConceptoRemuViewModel>(jsonModel);

         model.ConceptoBaseImponibleDTO.PageSize = ObtenerMaximoRegistrosBusqueda();

         try
         {
            List<ConceptoBaseImponibleDTO> conceptoBaseImponibleDTOList =
               conceptoBaseImponibleProxy.ListarPorConceptoRemunerativoId(model.ConceptoBaseImponibleDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = conceptoBaseImponibleDTOList.Count,
               rows = from f in conceptoBaseImponibleDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ConceptoBaseImponibleId,
                            f.ConceptoRemunerativoId,
                            f.TipoBaseImponibleId,
                            f.TipoBaseImponibleDTO.Nombre,
                            Estado = f.Estado == EstadoConstante.ACTIVO ? DigitoConstante.UNO : DigitoConstante.CERO,
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
      [HttpPost("ListPopup")]
      public IActionResult ListPopup([FromBody] ConceptoRemuViewModel model)
      {
         if (model == null)
            model = new ConceptoRemuViewModel();


         return PartialView(model);
      }

   }
}
