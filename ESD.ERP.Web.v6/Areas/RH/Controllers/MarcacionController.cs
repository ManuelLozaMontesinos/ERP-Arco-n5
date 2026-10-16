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
   [CheckAndHandleFileResult]
   public class MarcacionController : AuthorizedController
   {
      private readonly IPersonalMarcacionProxy personalMarcacionProxy;
      private readonly IHoraRemunerativaProxy horaRemunerativaProxy;
      private readonly IRelojMarcacionProxy relojMarcacionProxy;
      private readonly IRelojProxy relojProxy;
      private readonly IJornadaLaboralProxy jornadaLaboralProxy;
      private readonly AppHelper appHelper;
      public MarcacionController(
         IPersonalMarcacionProxy personalMarcacionProxy,
         IHoraRemunerativaProxy horaRemunerativaProxy,
         IRelojMarcacionProxy relojMarcacionProxy,
         IRelojProxy relojProxy,
         IJornadaLaboralProxy jornadaLaboralProxy,
         AppHelper appHelper
         )
      {
         this.personalMarcacionProxy = personalMarcacionProxy;
         this.horaRemunerativaProxy = horaRemunerativaProxy;
         this.relojMarcacionProxy = relojMarcacionProxy;
         this.relojProxy = relojProxy;
         this.jornadaLaboralProxy = jornadaLaboralProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         DateTime fechaDesde = DateTime.Today.AddDays(-7);
         if(fechaDesde.Month != DateTime.Today.Month)
            fechaDesde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

         MarcacionViewModel marcacionViewModel = new()
         {
            MenuItemList = new List<MenuItem>() {
               new MenuItem(){ Key = "sincronizar",Content="Sincronizar Marcaciones"},
               new MenuItem(){ MenuItemType = MenuItemType.ExportDataXLS}
            },
            PersonalMarcacionDTO = new PersonalMarcacionDTO()
            {
               FechaDesde = fechaDesde,
               FechaHasta = DateTime.Today
            }
         };
         cargarGridModel(marcacionViewModel);

         return PartialView(marcacionViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         MarcacionViewModel model = JsonSerializer.Deserialize<MarcacionViewModel>(jsonModel);

         try
         {
            if (model.PersonalMarcacionDTO.PersonalDTO == null)
               model.PersonalMarcacionDTO.PersonalDTO = new PersonalDTO();

            model.PersonalMarcacionDTO.PersonalDTO.EmpresaSedeDTO = new EmpresaSedeDTO()
            {
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
            };

            string result = personalMarcacionProxy.ListarMarcacionHoraGrid(model.PersonalMarcacionDTO);

            object jsonData = JsonSerializer.Deserialize(result, typeof(object));

            Logger.InfoFinish();
            return Json(jsonData, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] MarcacionViewModel model)
      {

         Logger.InfoStart();

         MarcacionViewModel marcacionViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(marcacionViewModel, model.PersonalMarcacionDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, marcacionViewModel);
      }
      private void cargarDataParaEditar(MarcacionViewModel model, int id)
      {

         PersonalMarcacionDTO personalMarcacionDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            personalMarcacionDTO = new PersonalMarcacionDTO
            {
               FechaDesde = DateTime.Today,
               FechaHasta = DateTime.Today
            };
         }
         else
         {
            personalMarcacionDTO = personalMarcacionProxy.GetById(new PersonalMarcacionDTO() { PersonalMarcacionId = id });
         }

         model.PersonalMarcacionDTO = personalMarcacionDTO;
         cargarGridModel(model);

         model.MenuItemList = new List<MenuItem>() {
            new MenuItem() { MenuItemType = MenuItemType.New },
            new MenuItem() { MenuItemType = MenuItemType.Delete },
            new MenuItem() { Key="importar", Content="Importar Horas"},
         };
      }
      private void cargarGridModel(MarcacionViewModel model) {
         List<HoraRemunerativaDTO> horaRemunerativaDTOList = horaRemunerativaProxy.Search(new HoraRemunerativaDTO()
         {
            EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
         });

         string colName = ",,Apellidos y Nombres,Fecha,Marcacion,Sin Tol.,Falta,Rem.,Verif.";

         foreach (HoraRemunerativaDTO horaRemunerativaDTO in horaRemunerativaDTOList)
            colName += string.Concat(SeparadorConstante.COMA, horaRemunerativaDTO.VariableCalculoDTO.Codigo);

         colName+=",,";

         string colModel = "["
            + "{\"name\":\"PersonalMarcacionId\",\"hidden\":true},"
            + "{\"name\":\"PersonalId\",\"hidden\":true},"
            + "{\"name\":\"RazonSocial\",\"width\":\"260\",\"align\":\"left\"},"
            + "{\"name\":\"Fecha\",\"width\":\"70\",\"align\":\"rigth\",\"datefmt\":\""+ JavaScriptConstant.GRID_DATE_FORMAT + "\"},"
            + "{\"name\":\"HoraMinuto\",\"width\":\"200\",\"align\":\"left\"},"
            + "{\"name\":\"IgnorarTolerancia\",\"width\":\"60\",\"align\":\"center\",\"editable\":true,\"edittype\":\"checkbox\","
            + "\"formatoptions\":{\"disabled\":true},\"editoptions\":{\"value\":\"1:0\"},\"formatter\":\"checkbox\"},"
            + "{\"name\":\"EsFalta\",\"width\":\"60\",\"align\":\"center\",\"editable\":true,\"edittype\":\"checkbox\","
            + "\"formatoptions\":{\"disabled\":true},\"editoptions\":{\"value\":\"1:0\"},\"formatter\":\"checkbox\"},"
            + "{\"name\":\"EsRemunerado\",\"width\":\"60\",\"align\":\"center\",\"editable\":true,\"edittype\":\"checkbox\","
            + "\"formatoptions\":{\"disabled\":true},\"editoptions\":{\"value\":\"1:0\"},\"formatter\":\"checkbox\"},"
            +"{\"name\":\"Verificado\",\"width\":\"60\",\"align\":\"center\",\"editable\":true,\"edittype\":\"checkbox\","
            + "\"formatoptions\":{\"disabled\":true},\"editoptions\":{\"value\":\"1:0\"},\"formatter\":\"checkbox\"}";
         foreach (HoraRemunerativaDTO horaRemunerativaDTO in horaRemunerativaDTOList) {
            colModel += ",{\"name\":\"" + horaRemunerativaDTO.VariableCalculoDTO.Codigo
               + "\",\"width\":\"55\",\"align\":\"right\",\"editable\":true,"
               + "\"editoptions\":{\"maxlength\":6,\"class\":\"form-control-sm\"}"
               + "}";
         }

         model.HoraList = string.Join(SeparadorConstante.COMA, horaRemunerativaDTOList
            .Select(x => x.VariableCalculoDTO.Codigo + SeparadorConstante.DOS_PUNTOS + x.HoraRemunerativaId.ToString()));

         colModel += ",{\"name\":\"_selM\",\"width\":35,\"align\":\"center\"},";
         colModel += "{\"name\":\"Estado\",\"hidden\":true}";
         colModel += "]";

         model.GridColNames = colName;
         model.GridColModel = colModel;
      }
      [HttpPost("SincronizarEditPopup")]
      public ActionResult SincronizarEditPopup([FromBody] MarcacionViewModel model)
      {
         MarcacionViewModel marcacionViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            DateTime fechaDesde = DateTime.Today.AddDays(-7);
            marcacionViewModel.PersonalMarcacionDTO = new PersonalMarcacionDTO()
            {
               FechaDesde = fechaDesde,
               FechaHasta = DateTime.Today
            };

         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(marcacionViewModel);
      }
      [HttpPost("SincronizarMarcacion")]
      public ActionResult SincronizarMarcacion([FromBody] MarcacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            int usuarioId = ObtenerUsuarioId();

            if (model.PersonalMarcacionDTO.PersonalDTO == null)
               model.PersonalMarcacionDTO.PersonalDTO = new PersonalDTO();

            model.PersonalMarcacionDTO.PersonalDTO.EmpresaSedeDTO = new EmpresaSedeDTO()
            {
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
            };

            model.PersonalMarcacionDTO.UsuarioCreacionId = usuarioId;
            model.PersonalMarcacionDTO.FechaHasta = model.PersonalMarcacionDTO.FechaHasta.AddHours(23).AddMinutes(59).AddSeconds(59);

            personalMarcacionProxy.SincronizarMarcacion(model.PersonalMarcacionDTO);

            resultResponse.Message = SR.GetString("RH_sincronizar_marca_actualizada");
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
      [HttpPost("ExportarEditPopup")]
      public ActionResult ExportarEditPopup([FromBody] MarcacionViewModel model)
      {
         MarcacionViewModel marcacionViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            DateTime fechaDesde = DateTime.Today.AddDays(-7);
            marcacionViewModel.PersonalMarcacionDTO = new PersonalMarcacionDTO()
            {
               FechaDesde = fechaDesde,
               FechaHasta = DateTime.Today
            };

         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(marcacionViewModel);
      }
      [HttpGet("ReportToExcel")]
      public FileContentResult ReportToExcel(string cid,string fechaDesde, string fechaHasta)
      {
         Dictionary<string, string> parameters = new();
         int empresaId = ObtenerEmpresaId(cid);
         parameters.Add("empresaId", empresaId.ToString());

         DateTime dateFrom = Convert.ToDateTime(fechaDesde);
         parameters.Add("fechaDesde", string.Format(StringFormatConstant.DATE_ISO, dateFrom));

         DateTime dateTo = Convert.ToDateTime(fechaHasta);
         parameters.Add("fechaHasta", string.Format(StringFormatConstant.DATE_ISO, dateTo));

         string downloadFileName = string.Concat("Marcaciones_", string.Format(ReportExportNameConstant.DDMMYYY_FORMAT, dateFrom),
            "_", string.Format(ReportExportNameConstant.DDMMYYY_FORMAT, dateTo));

         return appHelper.ReportToExcelData("sp_rhrpt_exportarMarcacion", parameters, downloadFileName);
      }
      [HttpPost("GrabarMarcacion")]
      public ActionResult GrabarMarcacion([FromBody] MarcacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            model.PersonalMarcacionDTO.PersonalDTO = new PersonalDTO() { 
               EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID) }
            };

            AuditDTO(model.PersonalMarcacionDTO);
            foreach (PersonalMarcacionHoraDTO personalMarcacionHoraDTO in model.PersonalMarcacionHoraDTOList)
               AuditDTO(personalMarcacionHoraDTO);

            personalMarcacionProxy.ActualizarHoraLote(model.PersonalMarcacionDTO);

            resultResponse.Message = SR.GetString("RH_personal_marcacion_actualizada");
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
      [HttpPost("AgregarPersonalEditPopup")]
      public IActionResult AgregarPersonalEditPopup([FromBody] MarcacionViewModel model)
      {
         model.PersonalMarcacionDTO.EsFalta = EstadoConstante.INACTIVO;
         model.PersonalMarcacionDTO.EsRemunerado = EstadoConstante.ACTIVO;
         return PartialView(model);
      }
      [HttpPost("AgregarPersonal")]
      public ActionResult AgregarPersonal([FromBody] MarcacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.PersonalMarcacionDTO);

            model.PersonalMarcacionDTO.HoraMinuto = string.Empty;
            model.PersonalMarcacionDTO.IgnorarTolerancia = SiNoConstante.N;

            if (model.PersonalMarcacionDTO.EsFalta == SiNoConstante.S)
            {
               RelojDTO relojDTO = relojProxy.SingleSearch(new RelojDTO() {
                  EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
                  EsCentral = SiNoConstante.S
               });

               DateTime fecha = model.PersonalMarcacionDTO.Fecha;

               model.PersonalMarcacionDTO.PersonalDTO = new PersonalDTO()
               {
                  EmpresaSedeDTO = new EmpresaSedeDTO()
                  {
                     EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
                  }
               };
               model.PersonalMarcacionDTO.FechaDesde = fecha;
               model.PersonalMarcacionDTO.FechaHasta = fecha;

               JornadaLaboralDTO jornadaLaboralDTO = jornadaLaboralProxy.SingleSearch(new JornadaLaboralDTO()
               {
                  EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
                  DiaDTO = new DiaDTO() { Codigo = ((int) fecha.DayOfWeek).ToString()}
               });

               decimal horaIngreso = Convert.ToDecimal(jornadaLaboralDTO.HoraIngreso.Replace(SeparadorConstante.DOS_PUNTOS, SeparadorConstante.PUNTO));

               int horas = Convert.ToInt32(Math.Floor(horaIngreso));
               int minutos = (horas * 60) + Convert.ToInt32((horaIngreso - horas) * 100);

               fecha = fecha.AddMinutes(minutos);

               model.PersonalMarcacionDTO.RelojMarcacionDTOList = new List<RelojMarcacionDTO>();
               RelojMarcacionDTO relojMarcacionDTO = new()
               {
                  RelojId = relojDTO.Id,
                  PersonalId = model.PersonalMarcacionDTO.PersonalId,
                  Fecha = model.PersonalMarcacionDTO.Fecha,
                  FechaFinal = fecha,
                  Observacion = string.Empty
               };
               AuditDTO(relojMarcacionDTO);
               model.PersonalMarcacionDTO.RelojMarcacionDTOList.Add(relojMarcacionDTO);

               decimal horaSalida = jornadaLaboralDTO.CantidadHoras;

               horas = Convert.ToInt32(Math.Floor(horaSalida));
               minutos = (horas * 60) + Convert.ToInt32((horaSalida - horas)*100);

               fecha = fecha.AddMinutes(minutos);

               relojMarcacionDTO = new RelojMarcacionDTO
               {
                  RelojId = relojDTO.Id,
                  PersonalId = model.PersonalMarcacionDTO.PersonalId,
                  Fecha = model.PersonalMarcacionDTO.Fecha,
                  FechaFinal = fecha,
                  Observacion = string.Empty
               };

               AuditDTO(relojMarcacionDTO);
               model.PersonalMarcacionDTO.RelojMarcacionDTOList.Add(relojMarcacionDTO);
            }

            personalMarcacionProxy.Insert(model.PersonalMarcacionDTO);

            resultResponse.Message = SR.GetString("RH_personal_marcacion_agregada");
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
      [HttpPost("EditPopup")]
      public IActionResult EditPopup([FromBody] MarcacionViewModel model)
      {
         List<RelojDTO> relojDTOList = relojProxy.Search(new RelojDTO() { 
            EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
            Estado = EstadoConstante.ACTIVO 
         });
         model.RelojString = string.Join(SeparadorConstante.PUNTO_COMA, relojDTOList.Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.Nombre));

         return PartialView(model);
      }
      [HttpPost("ListarRelojMarcacion")]
      public ActionResult ListarRelojMarcacion(string jsonModel)
      {

         Logger.InfoStart();

         MarcacionViewModel model = JsonSerializer.Deserialize<MarcacionViewModel>(jsonModel);

         try
         {
            model.RelojMarcacionDTO.OrderBy = "rm.fecha_final";
            List<RelojMarcacionDTO> relojMarcacionDTOList = relojMarcacionProxy.Search(model.RelojMarcacionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = relojMarcacionDTOList.Count,
               rows = from f in relojMarcacionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.RelojMarcacionId,
                            f.RelojId,
                            FechaOriginal = string.Format(StringFormatConstant.DATE_TIME, f.FechaOriginal),
                            FechaFinal = string.Format(StringFormatConstant.DATE_TIME, f.FechaFinal),
                            f.Observacion,
                            Modificada = f.FechaOriginal != f.FechaFinal ? SiNoConstante.S: SiNoConstante.N,
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
      [HttpPost("GrabarRelojMarcacion")]
      public ActionResult GrabarRelojMarcacion([FromBody] MarcacionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            RelojMarcacionDTO relojMarcacionDTO = model.RelojMarcacionDTO;

            switch (relojMarcacionDTO.DTOState)
            {
               case DTOState.Added:
                  relojMarcacionProxy.Insert(relojMarcacionDTO);
                  break;

               case DTOState.Modified:
                  relojMarcacionProxy.Update(relojMarcacionDTO);
                  break;
            }

            if (model.PersonalMarcacionDTO.PersonalDTO == null)
               model.PersonalMarcacionDTO.PersonalDTO = new PersonalDTO();

            model.PersonalMarcacionDTO.PersonalDTO.EmpresaSedeDTO = new EmpresaSedeDTO()
            {
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
            };

            model.PersonalMarcacionDTO.UsuarioCreacionId = ObtenerUsuarioId();
            personalMarcacionProxy.SincronizarMarcacion(model.PersonalMarcacionDTO);

            string result = personalMarcacionProxy.ListarMarcacionHoraGrid(model.PersonalMarcacionDTO);

            object jsonData = JsonSerializer.Deserialize(result, typeof(object));

            resultResponse.Message = SR.GetString("RH_personal_marcacion_actualizada");

            resultResponse.Data = jsonData;
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
      [HttpPost("EliminarRelojMarcacion")]
      public ActionResult EliminarRelojMarcacion([FromBody] MarcacionViewModel model)
      {

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.RelojMarcacionDTO.DTOState = DTOState.Deleted;

            relojMarcacionProxy.Delete(model.RelojMarcacionDTO);

            resultResponse.Message = SR.GetString("RH_personal_marcacion_anulada");

            if (model.PersonalMarcacionDTO.PersonalDTO == null)
               model.PersonalMarcacionDTO.PersonalDTO = new PersonalDTO();

            model.PersonalMarcacionDTO.PersonalDTO.EmpresaSedeDTO = new EmpresaSedeDTO()
            {
               EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
            };

            model.PersonalMarcacionDTO.UsuarioCreacionId = ObtenerUsuarioId();
            personalMarcacionProxy.SincronizarMarcacion(model.PersonalMarcacionDTO);

            string result = personalMarcacionProxy.ListarMarcacionHoraGrid(model.PersonalMarcacionDTO);

            object jsonData = JsonSerializer.Deserialize(result, typeof(object));

            resultResponse.Data = jsonData;
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
