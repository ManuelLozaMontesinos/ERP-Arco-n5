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
   public class HoraRemunerativaController : AuthorizedController
   {
      private readonly IHoraRemunerativaProxy horaRemunerativaProxy;
      private readonly ITipoVariableCalculoProxy tipoVariableCalculoProxy;
      private readonly ITipoDatoProxy tipoDatoProxy;
      public HoraRemunerativaController(
         IHoraRemunerativaProxy horaRemunerativaProxy,
         ITipoVariableCalculoProxy tipoVariableCalculoProxy,
         ITipoDatoProxy tipoDatoProxy
         )
      {
         this.horaRemunerativaProxy = horaRemunerativaProxy;
         this.tipoVariableCalculoProxy = tipoVariableCalculoProxy;
         this.tipoDatoProxy = tipoDatoProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         HoraRemunerativaViewModel horaRemunerativaViewModel = new();

         return PartialView(horaRemunerativaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         HoraRemunerativaViewModel model = JsonSerializer.Deserialize<HoraRemunerativaViewModel>(jsonModel);

         model.HoraRemunerativaDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.HoraRemunerativaDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
         model.HoraRemunerativaDTO.OrderBy = "hr.hora_rem";

         try
         {
            List<HoraRemunerativaDTO> horaRemunerativaDTOList = horaRemunerativaProxy.Search(model.HoraRemunerativaDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = horaRemunerativaDTOList.Count,
               rows = from f in horaRemunerativaDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.HoraRemunerativaId,
                            f.Codigo,
                            f.Nombre,
                            f.Abreviatura,
                            Valor = f.VariableCalculoDTO.ValorNumericoVerdad,
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
      public ActionResult Crear([FromBody] HoraRemunerativaViewModel model)
      {
         Logger.InfoStart();

         HoraRemunerativaViewModel horaRemunerativaViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(horaRemunerativaViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, horaRemunerativaViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] HoraRemunerativaViewModel model)
      {

         Logger.InfoStart();

         HoraRemunerativaViewModel horaRemunerativaViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(horaRemunerativaViewModel, model.HoraRemunerativaDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, horaRemunerativaViewModel);
      }
      private void cargarDataParaEditar(HoraRemunerativaViewModel model, int id)
      {

         HoraRemunerativaDTO horaRemunerativaDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            horaRemunerativaDTO = new HoraRemunerativaDTO()
            {
               VariableCalculoDTO = new VariableCalculoDTO()
            };
         }
         else
         {
            horaRemunerativaDTO = horaRemunerativaProxy.GetById(new HoraRemunerativaDTO() { HoraRemunerativaId = id });
         }

         model.HoraRemunerativaDTO = horaRemunerativaDTO;
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] HoraRemunerativaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            HoraRemunerativaDTO horaRemunerativaDTO = cargarHoraRemunerativaDTO(model);

            int newId = model.HoraRemunerativaDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = horaRemunerativaProxy.Insert(horaRemunerativaDTO);
                  messageResourceKey = "RH_hora_rem_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  horaRemunerativaProxy.Update(horaRemunerativaDTO);
                  messageResourceKey = "RH_hora_rem_actualizada";
                  break;
            }

            horaRemunerativaDTO = horaRemunerativaProxy.GetById(new HoraRemunerativaDTO() { HoraRemunerativaId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey, horaRemunerativaDTO.Nombre);
            resultResponse.Data = new
            {
               horaRemunerativaDTO.Id
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
      private HoraRemunerativaDTO cargarHoraRemunerativaDTO(HoraRemunerativaViewModel model)
      {

         HoraRemunerativaDTO horaRemunerativaDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               horaRemunerativaDTO = model.HoraRemunerativaDTO;
               horaRemunerativaDTO.EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID);
               TipoVariableCalculoDTO tipoVariableCalculoDTO = tipoVariableCalculoProxy.ObtenerPorCodigo(new TipoVariableCalculoDTO()
               {
                  Codigo = TipoVariableCalculoConstante.CAMPO_HORA
               });

               horaRemunerativaDTO.VariableCalculoDTO.TipoVariableCalculoId = tipoVariableCalculoDTO.Id;

               TipoDatoDTO tipoDatoDTO = tipoDatoProxy.ObtenerPorCodigo(new TipoDatoDTO()
               {
                  Codigo = TipoDatoConstante.DECIMAL
               });
               horaRemunerativaDTO.VariableCalculoDTO.TipoDatoId = tipoDatoDTO.Id;
            }
            else
            {
               horaRemunerativaDTO = horaRemunerativaProxy.GetById(new HoraRemunerativaDTO() { HoraRemunerativaId = model.HoraRemunerativaDTO.Id });

               model.HoraRemunerativaDTO.VariableCalculoId = horaRemunerativaDTO.VariableCalculoId;
               model.HoraRemunerativaDTO.VariableCalculoDTO.VariableCalculoId = horaRemunerativaDTO.VariableCalculoDTO.VariableCalculoId;
               model.HoraRemunerativaDTO.VariableCalculoDTO.TipoVariableCalculoId = horaRemunerativaDTO.VariableCalculoDTO.TipoVariableCalculoId;
               model.HoraRemunerativaDTO.VariableCalculoDTO.TipoDatoId = horaRemunerativaDTO.VariableCalculoDTO.TipoDatoId;
               horaRemunerativaDTO = model.HoraRemunerativaDTO;
            }

            if (string.IsNullOrEmpty(model.HoraRemunerativaDTO.Abreviatura))
               model.HoraRemunerativaDTO.Abreviatura = string.Empty;

            horaRemunerativaDTO.VariableCalculoDTO.Condicion = string.Empty;
            horaRemunerativaDTO.VariableCalculoDTO.ValorCadenaVerdad = string.Empty;
            horaRemunerativaDTO.VariableCalculoDTO.ValorCadenaFalso = string.Empty;
            horaRemunerativaDTO.VariableCalculoDTO.TablaOrigen = string.Empty;
            horaRemunerativaDTO.VariableCalculoDTO.TablaCondicion = string.Empty;
            horaRemunerativaDTO.VariableCalculoDTO.TablaAgrupacion = string.Empty;
            horaRemunerativaDTO.VariableCalculoDTO.EsSubconsulta = SiNoConstante.N;
            horaRemunerativaDTO.VariableCalculoDTO.SubConsultaAlias = string.Empty;
            horaRemunerativaDTO.VariableCalculoDTO.SubConsultaTipo = LetraConstante.I;
            horaRemunerativaDTO.VariableCalculoDTO.SubConsultaColumna = string.Empty;
            horaRemunerativaDTO.VariableCalculoDTO.SubConsultaEnlace = string.Empty;
            horaRemunerativaDTO.VariableCalculoDTO.AlcanceFormulaHora = SiNoConstante.S;
            horaRemunerativaDTO.VariableCalculoDTO.AlcanceFormula = SiNoConstante.N;
            horaRemunerativaDTO.VariableCalculoDTO.AlcanceCondicion = SiNoConstante.S;

            horaRemunerativaDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(horaRemunerativaDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return horaRemunerativaDTO;
      }
      [HttpGet("Eliminar")]
      public JsonResult Eliminar(int id)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            HoraRemunerativaDTO horaRemunerativaDTO = horaRemunerativaProxy.GetById(new HoraRemunerativaDTO() { HoraRemunerativaId = id });

            AuditDTO(horaRemunerativaDTO);

            horaRemunerativaProxy.Delete(horaRemunerativaDTO);

            resultResponse.Message = SR.GetString("RH_hora_rem_anulada", horaRemunerativaDTO.Nombre);
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
