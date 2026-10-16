using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.IV.Controllers
{
   [Area("IV")]
   [Route("[controller]")]
   public class VolumenExistenController : AuthorizedController
   {
      private readonly IEmpresaSedeProxy empresaSedeProxy;
      private readonly IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy;
      private readonly IAlmacenVolumenProductoProxy almacenVolumenProductoProxy;
      private readonly IDataImportItemProxy dataImportItemProxy;
      private readonly IProductoProxy productoProxy;
      private readonly AppHelper appHelper;
      public VolumenExistenController(
         IEmpresaSedeProxy empresaSedeProxy,
         IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy,
         IAlmacenVolumenProductoProxy almacenVolumenProductoProxy,
         IDataImportItemProxy dataImportItemProxy,
         IProductoProxy productoProxy,
         AppHelper appHelper
         )
      {
         this.empresaSedeProxy = empresaSedeProxy;
         this.periodoEmpresaSedeProxy = periodoEmpresaSedeProxy;
         this.almacenVolumenProductoProxy = almacenVolumenProductoProxy;
         this.dataImportItemProxy = dataImportItemProxy;
         this.productoProxy = productoProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         VolumenExistenViewModel volumenExistenViewModel = new();

         cargarListas(volumenExistenViewModel);

         volumenExistenViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.ExportXLS},
            new MenuItem() { MenuItemType = MenuItemType.ImportXLS}
         };

         return PartialView(volumenExistenViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         VolumenExistenViewModel model = JsonSerializer.Deserialize<VolumenExistenViewModel>(jsonModel);

         model.AlmacenVolumenProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.AlmacenVolumenProductoDTO.OrderBy = "p.producto";

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            model.AlmacenVolumenProductoDTO.ProductoDTO.Periodo = periodoEmpresaSedeDTO.PeriodoDTO.Codigo;

            string tipoProducto = string.Empty;
            if (model.AlmacenVolumenProductoDTO.EmpresaSedeId != 0)
               tipoProducto = TipoProductoConstante.PRODUCTO;
            else
               tipoProducto = DigitoConstante.CERO;

            model.AlmacenVolumenProductoDTO.ProductoDTO.FamiliaProductoDTO = new FamiliaProductoDTO()
            {
               ClaseProductoDTO = new ClaseProductoDTO() { GrupoProductoDTO = new GrupoProductoDTO()
               {
                  TipoProductoDTO = new TipoProductoDTO() { Codigo = tipoProducto }
               }}
            };

            List<AlmacenVolumenProductoDTO> almacenVolumenProductoDTOList = almacenVolumenProductoProxy.ListarPorSedeId(model.AlmacenVolumenProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = almacenVolumenProductoDTOList.Count,
               rows = from f in almacenVolumenProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.AlmacenVolumenProductoId,
                            f.EmpresaSedeId,
                            f.ProductoId,
                            Producto = f.ProductoDTO.Codigo,
                            f.ProductoDTO.Descripcion,
                            UnidadMedida = f.ProductoDTO.UnidadMedidaDTO.Abreviatura,
                            f.StockMinimo,
                            Estado = EstadoConstante.ACTIVO,
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
      private void cargarListas(VolumenExistenViewModel model)
      {

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();
         List<EmpresaSedeDTO> empresaSedeDTOList = empresaSedeProxy.Search(new EmpresaSedeDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            Estado = EstadoConstante.ACTIVO
         });

         model.EmpresaSedeList = WebHelper.ToSelectListItem(empresaSedeDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

      }
      private void cargarListaPeriodoEmpresSede(VolumenExistenViewModel model)
      {

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();
         List<PeriodoEmpresaSedeDTO> periodoEmpresaSedeDTOList = periodoEmpresaSedeProxy.Search(new PeriodoEmpresaSedeDTO()
         {
            EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
            PeriodoDTO = periodoEmpresaSedeDTO.PeriodoDTO,
            PeriodoId = periodoEmpresaSedeDTO.PeriodoId,
            Estado = EstadoConstante.ACTIVO
         });

         model.PeriodoEmpresaSedeList = WebHelper.ToSelectListItem(periodoEmpresaSedeDTOList, x => x.Id.ToString(), x => x.EmpresaSedeDTO.Nombre, 
            SelectListFirstOptionType.Select);

      }
      [HttpPost("GrabarAlmacenVolumenProducto")]
      public ActionResult GrabarAlmacenVolumenProducto([FromBody] VolumenExistenViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AlmacenVolumenProductoDTO almacenVolumenProductoDTO = cargarAlmacenVolumenProductoDTO(model);

            int newId = model.AlmacenVolumenProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (almacenVolumenProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = almacenVolumenProductoProxy.Insert(almacenVolumenProductoDTO);
                  messageResourceKey = "IV_producto_actualizado";
                  break;

               case DTOState.Modified:
                  almacenVolumenProductoProxy.Update(almacenVolumenProductoDTO);
                  messageResourceKey = "IV_producto_actualizado";
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
      private AlmacenVolumenProductoDTO cargarAlmacenVolumenProductoDTO(VolumenExistenViewModel model)
      {

         AlmacenVolumenProductoDTO almacenVolumenProductoDTO;
         try
         {

            almacenVolumenProductoDTO = model.AlmacenVolumenProductoDTO;

            if (almacenVolumenProductoDTO.Id == 0)
               almacenVolumenProductoDTO.DTOState = DTOState.Added;

            almacenVolumenProductoDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(almacenVolumenProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return almacenVolumenProductoDTO;
      }
      [HttpGet("ReportToExcel")]
      public FileContentResult ReportToExcel(string cid, string empresaSedeId)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "empresaSedeId", empresaSedeId },
            { "periodo", periodoEmpresaSedeDTO.PeriodoDTO.Codigo.ToString() }
         };

         return appHelper.ReportToExcelData("sp_rptvt_volumenExistencia", parameters, "VolumenExistencias");
      }
      [HttpPost("ImportarEditPopup")]
      public IActionResult ImportarEditPopup([FromBody] VolumenExistenViewModel model)
      {
         List<DataImportItemDTO> dataImportItemDTOList = dataImportItemProxy.Search(new DataImportItemDTO()
         {
            OrderBy = "orden",
            DataImportDTO = new DataImportDTO() { Codigo = DataImportConstant.VOLUMEN_EXISTENCIAS }
         });

         if (model == null)
            model = new VolumenExistenViewModel();

         model.DataImportItemDTOList = dataImportItemDTOList;

         return PartialView(model);
      }
      [HttpPost("SubirVolumenExistencia")]
      public ActionResult SubirVolumenExistencia(int? chunks, int? chunk, string name, string empresaSedeId)
      {
         var fileUpload = Request.Form.Files[0];
         chunk ??= 0;
         chunks ??= 0;

         List<DataImportItemDTO> dataImportItemDTOList = dataImportItemProxy.Search(new DataImportItemDTO()
         {
            OrderBy = "orden",
            DataImportDTO = new DataImportDTO()
            {
               Codigo = DataImportConstant.VOLUMEN_EXISTENCIAS
            }
         });

         ExcelPackage package = new(fileUpload.OpenReadStream());
         ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

         List<AlmacenVolumenProductoDTO> almacenVolumenProductoDTOList = new();
         StringBuilder sb = new();

         bool cancel = false;

         if (workSheet.Dimension.End.Row < 1)
         {
            sb.AppendLine(SR.GetString("GEN_upload_data_diferente_estructura"));
            cancel = true;
         }

         if (!cancel)
            if (workSheet.Dimension.End.Column != dataImportItemDTOList.Count)
            {
               sb.AppendLine(SR.GetString("GEN_upload_columna_diferente_estructura"));
               cancel = true;
            }

         if (!cancel)
         {
            int col = 1;
            foreach (DataImportItemDTO dataImportItemDTO in dataImportItemDTOList)
            {
               ExcelRange excelRange = workSheet.Cells[1, col];
               if (dataImportItemDTO.Columna != excelRange.Text)
               {
                  sb.AppendLine(SR.GetString("GEN_upload_columna_diferente_estructura"));
                  cancel = true;
                  break;
               }
               col++;
            }
         }

         if (!cancel)
         {
            EmpresaSedeDTO empresaSedeDTO = empresaSedeProxy.GetById(new EmpresaSedeDTO() { EmpresaSedeId = Convert.ToInt32(empresaSedeId) });

            List<AlmacenVolumenProductoDTO> almacenVolumenProductoDTOListTemp = almacenVolumenProductoProxy.
               Search(new AlmacenVolumenProductoDTO() { EmpresaSedeId = empresaSedeDTO.EmpresaSedeId });

            List<string> productoCodigoList = new();

            for (int row = 2; row <= workSheet.Dimension.End.Row; row++)
            {

               AlmacenVolumenProductoDTO almacenVolumenProductoDTO = new() {
                  EmpresaSedeId = empresaSedeDTO.Id,
                  Estado = EstadoConstante.ACTIVO,
                  DTOState = DTOState.Unchanged 
               };

               if (workSheet.Dimension.End.Column - 1 > dataImportItemDTOList.Count)
               {
                  sb.AppendLine(SR.GetString("GEN_upload_fila_columna_diferente_estructura", row));
                  break;
               }

               ExcelRange excelRange = workSheet.Cells[row, 1];
               object cellValue = excelRange.Text;
               if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
               {
                  int value = 0;
                  int.TryParse(cellValue.ToString(), out value);
                  almacenVolumenProductoDTO.AlmacenVolumenProductoId = value;
                  if (almacenVolumenProductoDTO.AlmacenVolumenProductoId != 0)
                  {
                     AlmacenVolumenProductoDTO almacenVolumenProductoDTOTemp = almacenVolumenProductoDTOListTemp.Where(x => x.AlmacenVolumenProductoId == value).FirstOrDefault();
                     almacenVolumenProductoDTO.ProductoId = almacenVolumenProductoDTOTemp.ProductoId;
                     almacenVolumenProductoDTO.ProductoDTO = almacenVolumenProductoDTOTemp.ProductoDTO;
                     almacenVolumenProductoDTO.DTOState = DTOState.Modified;
                  }
               }

               if (almacenVolumenProductoDTO.AlmacenVolumenProductoId == 0)
               {
                  excelRange = workSheet.Cells[row, 2];
                  cellValue = excelRange.Text;
                  if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
                  {
                     int value = 0;
                     int.TryParse(cellValue.ToString(), out value);
                     almacenVolumenProductoDTO.ProductoId = value;
                     if (almacenVolumenProductoDTO.ProductoId != 0)
                     {
                        AlmacenVolumenProductoDTO almacenVolumenProductoDTOTemp = almacenVolumenProductoDTOListTemp.Where(x => x.ProductoId == value).FirstOrDefault();
                        if (almacenVolumenProductoDTOTemp != null)
                        {
                           almacenVolumenProductoDTO.AlmacenVolumenProductoId = almacenVolumenProductoDTOTemp.AlmacenVolumenProductoId;
                           almacenVolumenProductoDTO.ProductoId = almacenVolumenProductoDTOTemp.ProductoId;
                           almacenVolumenProductoDTO.ProductoDTO = almacenVolumenProductoDTOTemp.ProductoDTO;
                           almacenVolumenProductoDTO.DTOState = DTOState.Modified;
                        }
                     }
                  }
               }

               if (almacenVolumenProductoDTO.ProductoId == 0)
               {
                  excelRange = workSheet.Cells[row, 3];
                  cellValue = excelRange.Text;
                  if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
                  {
                     AlmacenVolumenProductoDTO almacenVolumenProductoDTOTemp = almacenVolumenProductoDTOListTemp.Where(x => x.ProductoDTO.Codigo == cellValue.ToString()).FirstOrDefault();
                     if (almacenVolumenProductoDTOTemp != null)
                     {
                        almacenVolumenProductoDTO.AlmacenVolumenProductoId = almacenVolumenProductoDTOTemp.AlmacenVolumenProductoId;
                        almacenVolumenProductoDTO.ProductoId = almacenVolumenProductoDTOTemp.ProductoId;
                        almacenVolumenProductoDTO.ProductoDTO = almacenVolumenProductoDTOTemp.ProductoDTO;
                        almacenVolumenProductoDTO.DTOState = DTOState.Modified;
                     }
                     else
                     {
                        productoCodigoList.Add(cellValue.ToString());
                        almacenVolumenProductoDTO.ProductoDTO = new ProductoDTO() { Codigo = cellValue.ToString() };
                     }

                  }
               }

               excelRange = workSheet.Cells[row, 6];
               cellValue = excelRange.Text;
               if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
               {
                  decimal value = 0;
                  decimal.TryParse(cellValue.ToString(), out value);
                  almacenVolumenProductoDTO.StockMinimo = value;
               }

               AuditDTO(almacenVolumenProductoDTO);

               almacenVolumenProductoDTOList.Add(almacenVolumenProductoDTO);
            }

            if (productoCodigoList.Count > 0)
            {

               List<ProductoDTO> productoDTOList = productoProxy.Search(new ProductoDTO()
               {
                  FamiliaProductoDTO = new FamiliaProductoDTO()
                  {
                     ClaseProductoDTO = new ClaseProductoDTO()
                     {
                        GrupoProductoDTO = new GrupoProductoDTO
                        {
                           TipoProductoDTO = new TipoProductoDTO() { EmpresaId = empresaSedeDTO.EmpresaId }
                        }
                     }
                  },
                  ProductoList = productoCodigoList
               });

               foreach (AlmacenVolumenProductoDTO almacenVolumenProductoDTO in almacenVolumenProductoDTOList)
               {
                  if (almacenVolumenProductoDTO.DTOState != DTOState.Unchanged)
                     continue;

                  ProductoDTO productoDTO = productoDTOList.Where(x => x.Codigo == almacenVolumenProductoDTO.ProductoDTO.Codigo).FirstOrDefault();
                  if (productoDTO != null)
                  {
                     almacenVolumenProductoDTO.ProductoId = productoDTO.ProductoId;
                     almacenVolumenProductoDTO.ProductoDTO = productoDTO;
                     almacenVolumenProductoDTO.DTOState = DTOState.Modified;
                  }

               }
            }
         }

         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            if (sb.Length > 0)
               resultResponse.Data = sb.ToString();
            else
            {
               almacenVolumenProductoProxy.ActualizarLote(almacenVolumenProductoDTOList);

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
      [Route("StockMinimoListPopup")]
      public IActionResult StockMinimoListPopup([FromBody] VolumenExistenViewModel model)
      {

         if (model == null)
            model = new VolumenExistenViewModel();

         cargarListaPeriodoEmpresSede(model);
         cargarGridModel(model);

         model.ModalEditId = GetModalEditPanelId();

         return PartialView(model);
      }
      private void cargarGridModel(VolumenExistenViewModel model)
      {
         string colName = ",Código,Descripción,U.M.,,Proveedor,,,,Stock Min.,Saldo";

         string colModel = "["
            + "{\"name\":\"ProductoId\",\"hidden\":true},"
            + "{\"name\":\"Codigo\",\"width\":\"80\",\"align\":\"left\"},"
            + "{\"name\":\"Descripcion\",\"width\":\"250\",\"align\":\"left\"},"
            + "{\"name\":\"UnidadMedidaNombre\",\"width\":\"45\",\"align\":\"left\"},"
            + "{\"name\":\"ProveedorId\",\"hidden\":true},"
            + "{\"name\":\"RazonSocial\",\"width\":\"200\",\"align\":\"left\"},"
            + "{\"name\":\"PesoNeto\",\"hidden\":true},"
            + "{\"name\":\"PesoBruto\",\"hidden\":true},"
            + "{\"name\":\"MasterPack\",\"hidden\":true},"
            + "{\"name\":\"StockMinimo\",\"width\":\"80\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"},"
            + "{\"name\":\"Saldo\",\"width\":\"80\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"}";

         colModel += "]";

         model.GridColNames = colName;
         model.GridColModel = colModel;
      }
      [HttpPost("ListarStockMinimoListPopup")]
      public ActionResult ListarStockMinimoListPopup(string jsonModel)
      {

         Logger.InfoStart();

         VolumenExistenViewModel model = JsonSerializer.Deserialize<VolumenExistenViewModel>(jsonModel);

         model.AlmacenVolumenProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();

         try
         {
            string result = almacenVolumenProductoProxy.ListarStockMinimoGrid(model.AlmacenVolumenProductoDTO);

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
   }
}
