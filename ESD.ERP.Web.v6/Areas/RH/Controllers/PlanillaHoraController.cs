using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.RH.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.RH.Controllers
{
   [Area("RH")]
   [Route("[controller]")]
   [CheckAndHandleFileResult]
   public class PlanillaHoraController : AuthorizedController
   {
      private readonly IPlanillaProxy planillaProxy;
      private readonly IMesProxy mesProxy;
      private readonly IPlanillaProcesoProxy planillaProcesoProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IPlanillaProcesoHoraProxy planillaProcesoHoraProxy;
      private readonly IHoraRemunerativaProxy horaRemunerativaProxy;
      private readonly IPersonalMarcacionProxy personalMarcacionProxy;
      public PlanillaHoraController(
         IPlanillaProxy planillaProxy,
         IMesProxy mesProxy,
         IPlanillaProcesoProxy planillaProcesoProxy,
         IMonedaProxy monedaProxy,
         IPlanillaProcesoHoraProxy planillaProcesoHoraProxy,
         IHoraRemunerativaProxy horaRemunerativaProxy,
         IPersonalMarcacionProxy personalMarcacionProxy
         )
      {
         this.planillaProxy = planillaProxy;
         this.mesProxy = mesProxy;
         this.planillaProcesoProxy = planillaProcesoProxy;
         this.monedaProxy = monedaProxy;
         this.planillaProcesoHoraProxy = planillaProcesoHoraProxy;
         this.horaRemunerativaProxy = horaRemunerativaProxy;
         this.personalMarcacionProxy = personalMarcacionProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         PlanillaHoraViewModel planillaHoraViewModel = new PlanillaHoraViewModel();

         cargarListas(planillaHoraViewModel);

         planillaHoraViewModel.MenuItemList = new List<MenuItem>() {
            new MenuItem() { MenuItemType = MenuItemType.Edit}
         };

         return PartialView(planillaHoraViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         PlanillaHoraViewModel model = JsonSerializer.Deserialize<PlanillaHoraViewModel>(jsonModel);

         model.PlanillaProcesoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PlanillaProcesoDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.PlanillaProcesoDTO.OrderBy = "m.nombre,p.nombre";

         try
         {
            List<PlanillaProcesoDTO> planillaProcesoDTOList = planillaProcesoProxy.Search(model.PlanillaProcesoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = planillaProcesoDTOList.Count,
               rows = from f in planillaProcesoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PlanillaProcesoId,
                            PlanillaNombre = f.PlanillaDTO.Nombre,
                            MesNombre = f.MesDTO.Nombre,
                            MonedaNombre = f.MonedaDTO.Nombre,
                            f.ProcesoAnual,
                            FechaDesde = f.FechaDesde.ToShortDateString(),
                            FechaHasta = f.FechaHasta.ToShortDateString(),
                            FechaProceso = f.FechaProceso == null ? string.Empty : f.FechaProceso.Value.ToShortDateString(),
                            EstadoNombre = f.EstadoPlanillaProcesoDTO.Nombre,
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
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] PlanillaHoraViewModel model)
      {

         Logger.InfoStart();

         PlanillaHoraViewModel planillaHoraViewModel = new PlanillaHoraViewModel
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(planillaHoraViewModel, model.PlanillaProcesoDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, planillaHoraViewModel);
      }
      private void cargarDataParaEditar(PlanillaHoraViewModel model, int id)
      {

         PlanillaProcesoDTO planillaProcesoDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {
            planillaProcesoDTO = new PlanillaProcesoDTO
            {
               FechaDesde = DateTime.Today,
               FechaHasta = DateTime.Today,
               MonedaDTO = new MonedaDTO() { EsLocal = SiNoConstante.S}
            };
         }
         else
         {
            planillaProcesoDTO = planillaProcesoProxy.GetById(new PlanillaProcesoDTO() { PlanillaProcesoId = id });
         }

         model.PlanillaProcesoDTO = planillaProcesoDTO;
         cargarListas(model);
         cargarGridModel(model);

         WebHelper.SelectedListItemSetValue(model.PlanillaList, planillaProcesoDTO.PlanillaId);
         WebHelper.SelectedListItemSetValue(model.MesList, planillaProcesoDTO.MesId);
         WebHelper.SelectedListItemSetValue(model.MonedaList, planillaProcesoDTO.MonedaId);

         model.MenuItemList = new List<MenuItem>() {
            new MenuItem() { MenuItemType = MenuItemType.New },
            new MenuItem() { MenuItemType = MenuItemType.Delete },
            new MenuItem() { Key="importar", Content="Importar Horas"},
         };
      }
      private void cargarGridModel(PlanillaHoraViewModel model) {
         List<HoraRemunerativaDTO> horaRemunerativaDTOList = horaRemunerativaProxy.Search(new HoraRemunerativaDTO()
         {
            EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
         });

         string colName = ",Apellidos y Nombres";

         foreach (HoraRemunerativaDTO horaRemunerativaDTO in horaRemunerativaDTOList)
            colName += string.Concat(SeparadorConstante.COMA, horaRemunerativaDTO.VariableCalculoDTO.Codigo);

         colName+=",";

         string colModel = "["
            + "{\"name\":\"PersonalId\",\"hidden\":true},"
            + "{\"name\":\"RazonSocial\",\"width\":\"200\",\"align\":\"left\"}";

         foreach (HoraRemunerativaDTO horaRemunerativaDTO in horaRemunerativaDTOList) {
            colModel += ",{\"name\":\"" + horaRemunerativaDTO.VariableCalculoDTO.Codigo
               + "\",\"width\":\"70\",\"align\":\"right\",\"editable\":true,\"formatter\":\"number\","
               + "\"editoptions\":{\"maxlength\":6,\"class\":\"form-control-sm\"}"
               + "}";
         }

         model.HoraList = string.Join(SeparadorConstante.COMA, horaRemunerativaDTOList
            .Select(x => x.VariableCalculoDTO.Codigo + SeparadorConstante.DOS_PUNTOS + x.HoraRemunerativaId.ToString()));

         colModel += ",{\"name\":\"Estado\",\"hidden\":true}";
         colModel += "]";

         model.GridColNames = colName;
         model.GridColModel = colModel;
      }
      private void cargarListas(PlanillaHoraViewModel model)
      {

         List<PlanillaDTO> planillaDTOList = planillaProxy.Search(new PlanillaDTO()
         {
            EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
         });

         model.PlanillaList = WebHelper.ToSelectListItem(planillaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MesDTO> mesDTOList = mesProxy.Search(new MesDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.MesList = WebHelper.ToSelectListItem(mesDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MonedaDTO> monedaDTOList = monedaProxy.Search(new MonedaDTO()
         {
            Estado = EstadoConstante.ACTIVO
         });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

      }
      [HttpPost("ListarPlanillaProcesoHora")]
      public ActionResult ListarPlanillaProcesoHora(string jsonModel)
      {

         Logger.InfoStart();

         PlanillaHoraViewModel model = JsonSerializer.Deserialize<PlanillaHoraViewModel>(jsonModel);

         model.PlanillaProcesoHoraDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.PlanillaProcesoHoraDTO.PlanillaProcesoDTO = new PlanillaProcesoDTO()
         {
            PeriodoEmpresaSedeDTO = new PeriodoEmpresaSedeDTO()
            {
               EmpresaSedeId = ObtenerEmpresaId(model.ViewModelState.CID)
            }
         };

         try
         {
            string result = planillaProcesoHoraProxy.ListarHoraGrid(model.PlanillaProcesoHoraDTO);

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
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] PlanillaHoraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            int usuarioId = ObtenerUsuarioId();

            foreach (PlanillaProcesoHoraDTO planillaProcesoHoraDTO in model.PlanillaProcesoHoraDTOList) {
               planillaProcesoHoraDTO.UsuarioCreacionId = usuarioId;
            }

            planillaProcesoHoraProxy.ActualizarLote(model.PlanillaProcesoHoraDTOList);

            resultResponse.Message = SR.GetString("RH_planilla_proceso_hora_actualizada");
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
      [HttpPost("ImportarEditPopup")]
      public IActionResult ImportarEditPopup([FromBody] PlanillaHoraViewModel model)
      {
         return PartialView(model);
      }
      [HttpGet("DescargarFormato")]
      public FileContentResult DescargarFormato(string format, string cid)
      {
         byte[] result = null;
         List<HoraRemunerativaDTO> horaRemunerativaDTOList = horaRemunerativaProxy.Search(new HoraRemunerativaDTO()
         {
            EmpresaId = ObtenerEmpresaId(cid)
         });

         int col = 1;
         using (ExcelPackage package = new ExcelPackage())
         {
            ExcelWorksheet excelWorksheet = package.Workbook.Worksheets.Add("Data");
            excelWorksheet.Cells[1, col].Value = "Codigo";
            excelWorksheet.Cells[1, col].Style.Font.Bold = true;
            excelWorksheet.Column(col).Style.Numberformat.Format = "@";

            col++;
            foreach (HoraRemunerativaDTO horaRemunerativaDTO in horaRemunerativaDTOList) {
               excelWorksheet.Cells[1, col].Value = horaRemunerativaDTO.VariableCalculoDTO.Codigo;
               excelWorksheet.Cells[1, col].Style.Font.Bold = true;
               excelWorksheet.Column(col).Style.Numberformat.Format = "0.00";
               col++;
            }

            result = package.GetAsByteArray();
         }

         FileContentResult fileContentResult = new FileContentResult(result, MediaTypeNames.Application.Octet)
         {
            FileDownloadName = string.Concat("FormatoHora", ReportExportExtensionConstant.EXCELX)
         };

         return fileContentResult;
      }
      [HttpPost("SubirArchivoHora")]
      public ActionResult SubirArchivoHora(int? chunks, int? chunk, string name, string planillaProcesoId)
      {
         var fileUpload = Request.Form.Files[0];
         chunk = chunk ?? 0;
         chunks = chunks ?? 0;

         List<HoraRemunerativaDTO> horaRemunerativaDTOList = horaRemunerativaProxy.Search(new HoraRemunerativaDTO()
         {
            EmpresaId = ObtenerEmpresaId()
         });

         ExcelPackage package = new ExcelPackage(fileUpload.OpenReadStream());
         ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

         Dictionary<string,int> horaList = horaRemunerativaDTOList.ToDictionary(x => x.VariableCalculoDTO.Codigo, x => x.HoraRemunerativaId);

         List<PlanillaProcesoHoraDTO> planillaProcesoHoraDTOList = new List<PlanillaProcesoHoraDTO>();
         StringBuilder sb = new StringBuilder();

         bool cancel = false;

         if (workSheet.Dimension.End.Row < 1)
         {
            sb.AppendLine("Error General : La información del Archivo debe estar de acuerdo al Formato proporcionado");
            cancel = true;
         }

         if(!cancel)
            if (workSheet.Dimension.End.Column-1 > horaList.Count)
            {
               sb.AppendLine("Error General : Las columnas no estan de acuerdo al Formato proporcionado");
               cancel = true;
            }

         if (!cancel)
            for (int col = 2; col <= workSheet.Dimension.End.Column; col++) {
               ExcelRange excelRange = workSheet.Cells[1, col];
               if (!horaList.ContainsKey(excelRange.Text)) {
                  sb.AppendLine("Error General : Las columnas no estan de acuerdo al Formato proporcionado");
                  cancel = true;
                  break;
               }
            }

         if (!cancel)
            for (int row = 2; row <= workSheet.Dimension.End.Row; row++)
            {
               bool rowCancel = false;
               PlanillaProcesoHoraDTO planillaProcesoHoraDTO = null;

               if (workSheet.Dimension.End.Column-1 > horaList.Count)
               {
                  sb.AppendLine("Fila "+ row.ToString() + " : La Cantidad de columnas no esta de acuerdo al Formato proporcionado");
                  break;
               }
            
               ExcelRange excelRange = workSheet.Cells[row, 1];
               object firstCellValue = excelRange.Text;
               if (firstCellValue == null || string.IsNullOrEmpty(firstCellValue.ToString()))
               {
                  sb.AppendLine("Fila " + row.ToString() + " : La celda [" + excelRange.Address + "] debe tener un valor de texto o no debe estar vacía");
                  break;
               }

               for (int col = 2; col <= workSheet.Dimension.End.Column; col++)
               {
                  excelRange = workSheet.Cells[row, col];
                  object cellValue = excelRange.Text;

                  if (cellValue == null || string.IsNullOrEmpty(cellValue.ToString()))
                  {
                     sb.AppendLine("Fila " + row.ToString() + " : La celda [" + excelRange.Address + "] debe tener un valor numérico o no debe estar vacía");
                     rowCancel = true;
                     break;
                  }

                  decimal value = decimal.Zero;
                  bool result = decimal.TryParse(cellValue.ToString(), out value);

                  if (!result)
                  {
                     sb.AppendLine("Fila " + row.ToString() + " : La celda [" + excelRange.Address + "] debe tener un valor numérico o no debe estar vacía");
                     rowCancel = true;
                     break;
                  }

                  if (value == decimal.Zero) {
                     rowCancel = true;
                     break;
                  }

                  if (rowCancel)
                     break;

                  planillaProcesoHoraDTO = new PlanillaProcesoHoraDTO()
                  {
                     PersonalDTO = new PersonalDTO() { Codigo = firstCellValue.ToString() },
                     PlanillaProcesoId = Convert.ToInt32(planillaProcesoId),
                     HoraRemunerativaId = horaList[workSheet.Cells[1, col].Text],
                     Cantidad = value,
                     Estado = EstadoConstante.ACTIVO
                  };

                  AuditDTO(planillaProcesoHoraDTO);

                  planillaProcesoHoraDTOList.Add(planillaProcesoHoraDTO);
               }
            }

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            if (sb.Length > 0)
               resultResponse.Data = sb.ToString();
            else {
               planillaProcesoHoraProxy.ActualizarLotePorPersonal(planillaProcesoHoraDTOList);

               resultResponse.Message = SR.GetString("RH_planilla_proceso_hora_actualizada");
            }
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
      [HttpPost("PersonalHoraListPopup")]
      public IActionResult PersonalHoraListPopup([FromBody] PlanillaHoraViewModel model)
      {
         model.PersonalMarcacionDTO.PersonalDTO = new PersonalDTO()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = ObtenerEmpresaId() }
         };

         return PartialView(model);
      }
      [HttpPost("ListarPersonalConHoras")]
      public ActionResult ListarPersonalConHoras(string jsonModel)
      {

         Logger.InfoStart();

         PlanillaHoraViewModel model = JsonSerializer.Deserialize<PlanillaHoraViewModel>(jsonModel);

         try
         {
            List<PersonalDTO> personalDTOList = personalMarcacionProxy.ListarPersonalConHoras(model.PersonalMarcacionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = personalDTOList.Count,
               rows = from f in personalDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.PersonalId,
                            f.Codigo,
                            TipoDocumentoIdentidad = f.ProveedorDTO.TipoDocumentoIdentidadDTO.Abreviatura,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            f.ProveedorDTO.RazonSocial
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
      [HttpPost("GrabarListaPersonaId")]
      public ActionResult GrabarListaPersonaId([FromBody] PlanillaHoraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            int usuarioId = ObtenerUsuarioId();

            model.PlanillaProcesoHoraDTO.UsuarioCreacionId = usuarioId;

            planillaProcesoHoraProxy.ActualizarListaPersonalId(model.PlanillaProcesoHoraDTO);

            resultResponse.Message = SR.GetString("RH_planilla_proceso_hora_actualizada");
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
      [HttpPost("ActualizarDesdeMarcacion")]
      public ActionResult ActualizarDesdeMarcacion([FromBody] PlanillaHoraViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            int usuarioId = ObtenerUsuarioId();

            model.PlanillaProcesoHoraDTO.UsuarioCreacionId = usuarioId;

            planillaProcesoHoraProxy.ActualizarDesdeMarcacion(model.PlanillaProcesoHoraDTO);

            resultResponse.Message = SR.GetString("RH_planilla_proceso_hora_actualizada");
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
