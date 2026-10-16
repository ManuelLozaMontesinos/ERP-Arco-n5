using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Service.DTO;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   [CheckAndHandleFileResult]
   public class ListaPrecioController : AuthorizedController
   {
      private readonly IListaPrecioProxy listaPrecioProxy;
      private readonly IEmpresaSedeProxy empresaSedeProxy;
      private readonly IImpuestoTasaProxy impuestoTasaProxy;
      private readonly IMonedaProxy monedaProxy;
      private readonly IListaPrecioProductoProxy listaPrecioProductoProxy;
      private readonly IListaPrecioProductoPresentacionProxy listaPrecioProductoPresentacionProxy;
      private readonly IProductoPresentacionVentaProxy productoPresentacionVentaProxy;
      private readonly IDataImportItemProxy dataImportItemProxy;
      private readonly IProductoProxy productoProxy;
      private readonly IListaPrecioProveedorProxy listaPrecioProveedorProxy;
      private readonly IListaPrecioProveedorProductoProxy listaPrecioProveedorProductoProxy;
      private readonly IAlmacenProxy almacenProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly IComboProductoProxy comboProductoProxy;
      private readonly IListaPrecioVendedorProxy listaPrecioVendedorProxy;
      private readonly IVendedorProxy vendedorProxy;
      private readonly AppHelper appHelper;
      public ListaPrecioController(
         IListaPrecioProxy listaPrecioProxy,
         IEmpresaSedeProxy empresaSedeProxy,
         IImpuestoTasaProxy impuestoTasaProxy,
         IMonedaProxy monedaProxy,
         IListaPrecioProductoProxy listaPrecioProductoProxy,
         IListaPrecioProductoPresentacionProxy listaPrecioProductoPresentacionProxy,
         IProductoPresentacionVentaProxy productoPresentacionVentaProxy,
         IDataImportItemProxy dataImportItemProxy,
         IProductoProxy productoProxy,
         IListaPrecioProveedorProxy listaPrecioProveedorProxy,
         IListaPrecioProveedorProductoProxy listaPrecioProveedorProductoProxy,
         IAlmacenProxy almacenProxy,
         IMesContableProxy mesContableProxy,
         IComboProductoProxy comboProductoProxy,
         IListaPrecioVendedorProxy listaPrecioVendedorProxy,
         IVendedorProxy vendedorProxy,
         AppHelper appHelper
         )
      {
         this.listaPrecioProxy = listaPrecioProxy;
         this.empresaSedeProxy = empresaSedeProxy;
         this.impuestoTasaProxy = impuestoTasaProxy;
         this.monedaProxy = monedaProxy;
         this.listaPrecioProductoProxy = listaPrecioProductoProxy;
         this.listaPrecioProductoPresentacionProxy = listaPrecioProductoPresentacionProxy;
         this.productoPresentacionVentaProxy = productoPresentacionVentaProxy;
         this.dataImportItemProxy = dataImportItemProxy;
         this.productoProxy = productoProxy;
         this.listaPrecioProveedorProxy = listaPrecioProveedorProxy;
         this.listaPrecioProveedorProductoProxy = listaPrecioProveedorProductoProxy;
         this.almacenProxy = almacenProxy;
         this.mesContableProxy = mesContableProxy;
         this.comboProductoProxy = comboProductoProxy;
         this.listaPrecioVendedorProxy = listaPrecioVendedorProxy;
         this.vendedorProxy = vendedorProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         return PartialView(new ListaPrecioViewModel());
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ListaPrecioViewModel model = JsonSerializer.Deserialize<ListaPrecioViewModel>(jsonModel);

         model.ListaPrecioDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ListaPrecioDTO.OrderBy = "lp.nombre";

         model.ListaPrecioDTO.EmpresaSedeDTO = new EmpresaSedeDTO()
         {
            EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID)
         };

         try
         {
            List<ListaPrecioDTO> listaPrecioDTOList = listaPrecioProxy.Search(model.ListaPrecioDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = listaPrecioDTOList.Count,
               rows = from f in listaPrecioDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ListaPrecioId,
                            EmpresaSedeNombre = f.EmpresaSedeDTO.Nombre,
                            f.Nombre,
                            MonedaNombre = f.MonedaDTO.Nombre,

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
      public ActionResult Crear([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ListaPrecioViewModel listaPrecioViewModel = new()
         {
            EditAction = ViewModelEditAction.New,
            IsNew = true
         };

         try
         {
            cargarDataParaEditar(listaPrecioViewModel, 0);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, listaPrecioViewModel);
      }
      [HttpPost("Editar")]
      public ActionResult Editar([FromBody] ListaPrecioViewModel model)
      {

         Logger.InfoStart();

         ListaPrecioViewModel listaPrecioViewModel = new()
         {
            EditAction = ViewModelEditAction.Edit
         };

         try
         {
            cargarDataParaEditar(listaPrecioViewModel, model.ListaPrecioDTO.Id);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         Logger.InfoFinish();

         return PartialView(PartialViewNameConstant.CRUD, listaPrecioViewModel);
      }
      private void cargarDataParaEditar(ListaPrecioViewModel model, int id)
      {

         ListaPrecioDTO listaPrecioDTO = null;
         if (model.EditAction == ViewModelEditAction.New)
         {

            listaPrecioDTO = new ListaPrecioDTO
            {
               ImpuestoTasaDTO = new ImpuestoTasaDTO()
            };

            MonedaDTO monedaDTO = monedaProxy.ObtenerPorCodigo(new MonedaDTO()
            { Codigo = MonedaConstante.DEFAULT });
            listaPrecioDTO.MonedaId = monedaDTO.Id;
         }
         else
         {
            listaPrecioDTO = listaPrecioProxy.GetById(new ListaPrecioDTO() { ListaPrecioId = id });
         }

         model.ListaPrecioDTO = listaPrecioDTO;

         cargarListas(model);

         WebHelper.SelectedListItemSetValue(model.EmpresaSedeList, listaPrecioDTO.EmpresaSedeId);
         WebHelper.SelectedListItemSetValue(model.ImpuestoTasaList, listaPrecioDTO.ImpuestoTasaId);
         WebHelper.SelectedListItemSetValue(model.MonedaList, listaPrecioDTO.MonedaId);

         model.MenuItemList = new List<MenuItem>() { 
            new MenuItem(){ MenuItemType = MenuItemType.New },
            new MenuItem(){ MenuItemType = MenuItemType.Edit},
            new MenuItem(){ MenuItemType = MenuItemType.Save},
            new MenuItem(){ MenuItemType = MenuItemType.Cancel},
            new MenuItem(){ Key="presentacion", Content="Presentaciones"},
            new MenuItem(){ Key="calcular_precio", Content="Calcular Precios"},
            new MenuItem(){ MenuItemType = MenuItemType.ImportXLS},
            new MenuItem(){ MenuItemType = MenuItemType.ExportXLS}
         };
      }
      private void cargarListas(ListaPrecioViewModel model)
      {
         List<EmpresaSedeDTO> empresaSedeDTOList = empresaSedeProxy.Search(new EmpresaSedeDTO()
         {
            EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
            Estado = EstadoConstante.ACTIVO,
         });
         model.EmpresaSedeList = WebHelper.ToSelectListItem(empresaSedeDTOList, x => x.Id.ToString(), x => x.Nombre, 
            SelectListFirstOptionType.Select);

         List<ImpuestoTasaDTO> impuestoTasaDTOList = impuestoTasaProxy.Search(new ImpuestoTasaDTO()
         {
            Estado = EstadoConstante.ACTIVO,
         });
         model.ImpuestoTasaList = WebHelper.ToSelectListItem(impuestoTasaDTOList, x => x.Id.ToString(), x => x.ImpuestoNombreTasa, 
            SelectListFirstOptionType.Select);

         cargarListaMoneda(model);
      }
      private void cargarListaMoneda(ListaPrecioViewModel model)
      {
         List<MonedaDTO> monedaDTOList = monedaProxy.
         Search(new MonedaDTO()
         {
            Estado = EstadoConstante.ACTIVO,
         });
         model.MonedaList = WebHelper.ToSelectListItem(monedaDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         if(model.SelectedItemList != null)
         {
            SelectedItem selectedItem = model.SelectedItemList.Where(x => x.Name == "Moneda").FirstOrDefault();
            if (selectedItem != null)
            {
               MonedaDTO monedaDTO = monedaDTOList.Where(x => x.Codigo == selectedItem.Value).FirstOrDefault();
               if (monedaDTO != null) {
                  WebHelper.SelectedListItemSetValue(model.MonedaList, monedaDTO.Id);
                  model.ListaPrecioProductoDTO.ListaPrecioDTO = new ListaPrecioDTO() { MonedaId = monedaDTO.Id };
               }
            }
            selectedItem = model.SelectedItemList.Where(x => x.Name == "MonedaId").FirstOrDefault();
            if (selectedItem != null)
            {
               WebHelper.SelectedListItemSetValue(model.MonedaList, Convert.ToInt32(selectedItem.Value));

               if(model.ListaPrecioProductoDTO != null)
                  model.ListaPrecioProductoDTO.ListaPrecioDTO = new ListaPrecioDTO() { MonedaId = Convert.ToInt32(selectedItem.Value) };

               if (model.ListaPrecioProveedorProductoDTO != null)
                  model.ListaPrecioProveedorProductoDTO.ListaPrecioProductoDTO.ListaPrecioDTO = new ListaPrecioDTO() { MonedaId = Convert.ToInt32(selectedItem.Value) };
            }

         }
      }
      [HttpGet("ObtenerImpuestoTasa")]
      public JsonResult ObtenerImpuestoTasa(int id)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            ImpuestoTasaDTO impuestoTasaDTO = impuestoTasaProxy.GetById(new ImpuestoTasaDTO() { ImpuestoTasaId = id });

            resultResponse.Data = new
            {
               impuestoTasaDTO.Tasa
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
      public ActionResult Grabar([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ListaPrecioDTO listaPrecioDTO = cargarListaPrecioDTO(model);

            int newId = model.ListaPrecioDTO.Id;
            string messageResourceKey = string.Empty;
            switch (model.EditAction)
            {
               case ViewModelEditAction.New:
                  newId = listaPrecioProxy.Insert(listaPrecioDTO);
                  messageResourceKey = "VT_lista_precio_agregada";
                  break;

               case ViewModelEditAction.Edit:
                  listaPrecioProxy.Update(listaPrecioDTO);
                  messageResourceKey = "VT_lista_precio_actualizada";
                  break;
            }

            listaPrecioDTO = listaPrecioProxy.GetById(new ListaPrecioDTO() { ListaPrecioId = newId });

            resultResponse.Message = SR.GetString(messageResourceKey);
            resultResponse.Data = new
            {
               listaPrecioDTO.Id
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
      private ListaPrecioDTO cargarListaPrecioDTO(ListaPrecioViewModel model)
      {

         ListaPrecioDTO listaPrecioDTO;
         try
         {

            if (model.EditAction == ViewModelEditAction.New)
            {
               listaPrecioDTO = model.ListaPrecioDTO;
            }
            else
            {
               listaPrecioDTO = listaPrecioProxy.GetById(new ListaPrecioDTO(){ ListaPrecioId = model.ListaPrecioDTO.Id });

               listaPrecioDTO = model.ListaPrecioDTO;
            }

            listaPrecioDTO = model.ListaPrecioDTO;
            listaPrecioDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(listaPrecioDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return listaPrecioDTO;
      }
      [HttpPost("Eliminar")]
      public ActionResult Eliminar([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ListaPrecioDTO.DTOState = DTOState.Modified;

            AuditDTO(model.ListaPrecioDTO);

            listaPrecioProxy.Delete(model.ListaPrecioDTO);

            resultResponse.Message = SR.GetString("VT_lista_precio_anulada");
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
      [HttpPost("ListarListaPrecioProducto")]
      public ActionResult ListarListaPrecioProducto(string jsonModel)
      {

         Logger.InfoStart();

         ListaPrecioViewModel model = JsonSerializer.Deserialize<ListaPrecioViewModel>(jsonModel);

         model.ListaPrecioProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ListaPrecioProductoDTO.OrderBy = "p.descripcion";


         try
         {
            List<ListaPrecioProductoDTO> listaPrecioProductoDTOList = listaPrecioProductoProxy.Search(model.ListaPrecioProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = listaPrecioProductoDTOList.Count,
               rows = from f in listaPrecioProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ListaPrecioProductoId,
                            f.ListaPrecioId,
                            f.ProductoId,
                            f.ProductoDTO.Codigo,
                            f.ProductoDTO.Descripcion,
                            f.ProductoDTO.FamiliaProductoDTO.ClaseProductoDTO.GrupoProductoDTO.EsCombo,
                            f.ProductoDTO.TipoIgvId,
                            TipoIgv = f.ProductoDTO.TipoIgvDTO.Codigo,
                            f.ProductoDTO.TienePresentacionVenta,
                            f.ValorUnitario,
                            f.PrecioUnitario,
                            PermiteDescuento = f.PermiteDescuento == SiNoConstante.S ? DigitoConstante.UNO : DigitoConstante.CERO,
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
      [HttpPost("ImportarEditPopup")]
      public IActionResult ImportarEditPopup([FromBody] ListaPrecioViewModel model)
      {
         List<DataImportItemDTO> dataImportItemDTOList = dataImportItemProxy.Search(new DataImportItemDTO()
         {
            OrderBy = "orden",
            DataImportDTO = new DataImportDTO() { Codigo = DataImportConstant.LISTA_PRECIO_GENERAL }
         });

         if (model == null)
            model = new ListaPrecioViewModel();

         model.DataImportItemDTOList = dataImportItemDTOList;

         return PartialView(model);
      }
      [HttpPost("SubirListaPrecioGeneral")]
      public ActionResult SubirListaPrecioGeneral(int? chunks, int? chunk, string name, string listaPrecioId)
      {
         var fileUpload = Request.Form.Files[0];
         chunk ??= 0;
         chunks ??= 0;

         List<DataImportItemDTO> dataImportItemDTOList = dataImportItemProxy.Search(new DataImportItemDTO()
         {
            OrderBy = "orden",
            DataImportDTO = new DataImportDTO() { 
               Codigo = DataImportConstant.LISTA_PRECIO_GENERAL 
            }
         });

         ExcelPackage package = new(fileUpload.OpenReadStream());
         ExcelWorksheet workSheet = package.Workbook.Worksheets.First();

         List<ListaPrecioProductoDTO> listaPrecioProductoDTOList = new();
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
               if(dataImportItemDTO.Columna != excelRange.Text)
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
            ListaPrecioDTO listaPrecioDTO = listaPrecioProxy.GetById(new ListaPrecioDTO() { ListaPrecioId = Convert.ToInt32(listaPrecioId) });
            List<ListaPrecioProductoDTO> listaPrecioProductoDTOListTemp = listaPrecioProductoProxy.
               Search(new ListaPrecioProductoDTO() { ListaPrecioId = Convert.ToInt32(listaPrecioId) });

            List<string> productoCodigoList = new();

            for (int row = 2; row <= workSheet.Dimension.End.Row; row++)
            {

               ListaPrecioProductoDTO listaPrecioProductoDTO = new() { DTOState = DTOState.Unchanged };

               if (workSheet.Dimension.End.Column - 1 > dataImportItemDTOList.Count)
               {
                  sb.AppendLine("Fila " + row.ToString() + " : Las columnas no esta de acuerdo a la estructura requerida");
                  break;
               }

               ExcelRange excelRange = workSheet.Cells[row, 1];
               object cellValue = excelRange.Text;
               if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
               {
                  int value = 0;
                  int.TryParse(cellValue.ToString(), out value);
                  listaPrecioProductoDTO.ListaPrecioProductoId = value;
                  if(listaPrecioProductoDTO.ListaPrecioProductoId != 0)
                  {
                     ListaPrecioProductoDTO listaPrecioProductoDTOTemp = listaPrecioProductoDTOListTemp.Where(x => x.ListaPrecioProductoId == value).FirstOrDefault();
                     listaPrecioProductoDTO.ProductoId = listaPrecioProductoDTOTemp.ProductoId;
                     listaPrecioProductoDTO.ProductoDTO = listaPrecioProductoDTOTemp.ProductoDTO;
                     listaPrecioProductoDTO.DTOState = DTOState.Modified;
                  }
               }

               if(listaPrecioProductoDTO.ListaPrecioProductoId == 0)
               {
                  excelRange = workSheet.Cells[row, 2];
                  cellValue = excelRange.Text;
                  if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
                  {
                     int value = 0;
                     int.TryParse(cellValue.ToString(), out value);
                     listaPrecioProductoDTO.ProductoId = value;
                     if (listaPrecioProductoDTO.ProductoId != 0)
                     {
                        ListaPrecioProductoDTO listaPrecioProductoDTOTemp = listaPrecioProductoDTOListTemp.Where(x => x.ProductoId == value).FirstOrDefault();
                        if(listaPrecioProductoDTOTemp != null)
                        {
                           listaPrecioProductoDTO.ListaPrecioProductoId = listaPrecioProductoDTOTemp.ListaPrecioProductoId;
                           listaPrecioProductoDTO.ProductoId = listaPrecioProductoDTOTemp.ProductoId;
                           listaPrecioProductoDTO.ProductoDTO = listaPrecioProductoDTOTemp.ProductoDTO;
                           listaPrecioProductoDTO.DTOState = DTOState.Modified;
                        }
                     }
                  }
               }

               if (listaPrecioProductoDTO.ProductoId == 0)
               {
                  excelRange = workSheet.Cells[row, 3];
                  cellValue = excelRange.Text;
                  if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
                  {
                     ListaPrecioProductoDTO listaPrecioProductoDTOTemp = listaPrecioProductoDTOListTemp.Where(x => x.ProductoDTO.Codigo == cellValue.ToString()).FirstOrDefault();
                     if (listaPrecioProductoDTOTemp != null)
                     {
                        listaPrecioProductoDTO.ListaPrecioProductoId = listaPrecioProductoDTOTemp.ListaPrecioProductoId;
                        listaPrecioProductoDTO.ProductoId = listaPrecioProductoDTOTemp.ProductoId;
                        listaPrecioProductoDTO.ProductoDTO = listaPrecioProductoDTOTemp.ProductoDTO;
                        listaPrecioProductoDTO.DTOState = DTOState.Modified;
                     }
                     else
                     {
                        productoCodigoList.Add(cellValue.ToString());
                        listaPrecioProductoDTO.ProductoDTO = new ProductoDTO() { Codigo = cellValue.ToString() };
                     }

                  }
               }

               excelRange = workSheet.Cells[row, 5];
               cellValue = excelRange.Text;
               if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
               {
                  decimal value = 0;
                  decimal.TryParse(cellValue.ToString(), out value);
                  listaPrecioProductoDTO.ValorUnitario = value;
               }

               excelRange = workSheet.Cells[row, 6];
               cellValue = excelRange.Text;
               if (cellValue != null || !string.IsNullOrEmpty(cellValue.ToString()))
               {
                  decimal value = 0;
                  decimal.TryParse(cellValue.ToString(), out value);
                  listaPrecioProductoDTO.PrecioUnitario = value;
               }

               AuditDTO(listaPrecioProductoDTO);

               listaPrecioProductoDTOList.Add(listaPrecioProductoDTO);
               //}
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
                           TipoProductoDTO = new TipoProductoDTO() { EmpresaId = listaPrecioDTO.EmpresaSedeDTO.EmpresaId}
                        }
                     }
                  },
                  ProductoList = productoCodigoList
               });

               foreach(ListaPrecioProductoDTO listaPrecioProductoDTO in listaPrecioProductoDTOList)
               {
                  if (listaPrecioProductoDTO.DTOState != DTOState.Unchanged)
                     continue;

                  ProductoDTO productoDTO = productoDTOList.Where(x => x.Codigo == listaPrecioProductoDTO.ProductoDTO.Codigo).FirstOrDefault();
                  if(productoDTO != null)
                  {
                     listaPrecioProductoDTO.ProductoId = productoDTO.ProductoId;
                     listaPrecioProductoDTO.ProductoDTO = productoDTO;
                     listaPrecioProductoDTO.DTOState = DTOState.Modified;
                  }

               }
            }

            foreach (ListaPrecioProductoDTO listaPrecioProductoDTO in listaPrecioProductoDTOList)
            {
               if (listaPrecioProductoDTO.DTOState != DTOState.Modified)
                  continue;

               listaPrecioProductoDTO.ListaPrecioId = listaPrecioDTO.Id;

               CalculadorVentaItemDTO calculadorVentaItemDTO = null;
               if (listaPrecioProductoDTO.PrecioUnitario > decimal.Zero)
                  calculadorVentaItemDTO = CalculadorVenta.CalcularPrecios(new CalculadorVentaItemDTO() { 
                     TipoIGV = listaPrecioProductoDTO.ProductoDTO.TipoIgvDTO.Codigo,
                     ImpuestoTasa = listaPrecioDTO.ImpuestoTasaDTO.Tasa,
                     PrecioUnitario = listaPrecioProductoDTO.PrecioUnitario
                  });
               else
                  calculadorVentaItemDTO = CalculadorVenta.CalcularPrecios(new CalculadorVentaItemDTO()
                  {
                     TipoIGV = listaPrecioProductoDTO.ProductoDTO.TipoIgvDTO.Codigo,
                     ImpuestoTasa = listaPrecioDTO.ImpuestoTasaDTO.Tasa,
                     ValorUnitario = listaPrecioProductoDTO.ValorUnitario
                  });

               listaPrecioProductoDTO.ValorUnitario = calculadorVentaItemDTO.ValorUnitario;
               listaPrecioProductoDTO.PrecioUnitario = calculadorVentaItemDTO.PrecioUnitario;
               if (string.IsNullOrEmpty(listaPrecioProductoDTO.Estado))
                  listaPrecioProductoDTO.Estado = EstadoConstante.ACTIVO;

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
               listaPrecioProductoProxy.ActualizarLote(listaPrecioProductoDTOList);

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
      [HttpGet("ReportToExcel")]
      public FileContentResult ReportToExcel(string cid, string listaPrecioId)
      {
         Dictionary<string, string> parameters = new()
         {
            { "listaPrecioId", listaPrecioId }
         };

         return appHelper.ReportToExcelData("sp_vtrpt_listaPrecio", parameters, "ListaPreciosGeneral");
      }
      [HttpPost("GrabarListaPrecioProducto")]
      public ActionResult GrabarListaPrecioProducto([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ListaPrecioProductoDTO listaPrecioProductoDTO = cargarListaPrecioProductoDTO(model);

            int newId = model.ListaPrecioProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (listaPrecioProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = listaPrecioProductoProxy.Insert(listaPrecioProductoDTO);
                  messageResourceKey = "MG_producto_presentacion_agregada";
                  break;

               case DTOState.Modified:
                  listaPrecioProductoProxy.Update(listaPrecioProductoDTO);
                  messageResourceKey = "MG_producto_presentacion_actualizada";
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
      private ListaPrecioProductoDTO cargarListaPrecioProductoDTO(ListaPrecioViewModel model)
      {

         ListaPrecioProductoDTO listaPrecioProductoDTO;
         try
         {

            listaPrecioProductoDTO = model.ListaPrecioProductoDTO;

            AuditDTO(listaPrecioProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return listaPrecioProductoDTO;
      }
      [HttpPost("EliminarListaPrecioProducto")]
      public ActionResult EliminarListaPrecioProducto([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ListaPrecioProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ListaPrecioProductoDTO);

            listaPrecioProductoProxy.Delete(model.ListaPrecioProductoDTO);

            resultResponse.Message = SR.GetString("MG_producto_presentacion_eliminada");
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
      [HttpPost("CalcularPrecio")]
      public ActionResult CalcularPrecio([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularPrecios(model.CalculadorVentaItemDTO);

            resultResponse.Data = new
            {
               calculadorVentaItemDTO.PrecioUnitario,
               calculadorVentaItemDTO.ValorUnitario
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
      [HttpPost("CalcularPrecioCombo")]
      public ActionResult CalcularPrecioCombo([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            List<ComboProductoDTO> comboProductoDTOList = comboProductoProxy.Search(new ComboProductoDTO()
            {
               ProductoId = model.ListaPrecioProductoDTO.ProductoId,
               Estado = EstadoConstante.ACTIVO
            });

            decimal precioUnitarioFinal = decimal.Zero;

            foreach (ComboProductoDTO comboProductoDTO in comboProductoDTOList)
            {

               ListaPrecioProductoDTO listaPrecioProductoDTO = listaPrecioProductoProxy.SingleSearch(new ListaPrecioProductoDTO()
               {
                  ListaPrecioId = model.ListaPrecioProductoDTO.ListaPrecioId,
                  ProductoId = comboProductoDTO.ProductoIncluidoId,
                  Estado = EstadoConstante.ACTIVO
               });

               decimal precioUnitario = decimal.Round((listaPrecioProductoDTO.PrecioUnitario * comboProductoDTO.PorcentajePrecio) / 100,
                  comboProductoDTO.NumeroDecimalesRedondeo);

               precioUnitarioFinal += comboProductoDTO.Cantidad * precioUnitario;
            }

            model.CalculadorVentaItemDTO.PrecioUnitario = precioUnitarioFinal;

            CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularPrecios(model.CalculadorVentaItemDTO);

            resultResponse.Data = new
            {
               calculadorVentaItemDTO.PrecioUnitario,
               calculadorVentaItemDTO.ValorUnitario
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
      [Route("PresentacionEditPopup")]
      public IActionResult PresentacionEditPopup([FromBody] ListaPrecioViewModel model)
      {

         ListaPrecioProductoDTO listaPrecioProductoDTO = listaPrecioProductoProxy.GetById(model.ListaPrecioProductoDTO);

         model.ListaPrecioProductoDTO = listaPrecioProductoDTO;

         List<ProductoPresentacionVentaDTO> productoPresentacionVentaDTOList = productoPresentacionVentaProxy.Search(new ProductoPresentacionVentaDTO()
         {
            ProductoId = listaPrecioProductoDTO.ProductoId,
            Estado = EstadoConstante.ACTIVO,
         });

         model.UnidadMedidaString = string.Join(SeparadorConstante.PUNTO_COMA, productoPresentacionVentaDTOList.
            Select(x => x.Id.ToString() + SeparadorConstante.DOS_PUNTOS + x.UnidadMedidaDTO.Nombre));

         return PartialView(model);
      }
      [HttpPost("ListarListaPrecioProductoPresentacion")]
      public ActionResult ListarListaPrecioProductoPresentacion(string jsonModel)
      {

         Logger.InfoStart();

         ListaPrecioViewModel model = JsonSerializer.Deserialize<ListaPrecioViewModel>(jsonModel);

         model.ListaPrecioProductoPresentacionDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ListaPrecioProductoPresentacionDTO.OrderBy = "um.unidad_medida";
         model.ListaPrecioProductoPresentacionDTO.Estado = EstadoConstante.ACTIVO;

         try
         {
            List<ListaPrecioProductoPresentacionDTO> listaPrecioProductoPresentacionDTOList = listaPrecioProductoPresentacionProxy.Search(model.ListaPrecioProductoPresentacionDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = listaPrecioProductoPresentacionDTOList.Count,
               rows = from f in listaPrecioProductoPresentacionDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ListaPrecioProductoPresentacionId,
                            f.ListaPrecioProductoId,
                            f.ProductoPresentacionVentaId,
                            f.ProductoPresentacionVentaDTO.UnidadMedidaId,
                            UnidadMedidaNombre = f.ProductoPresentacionVentaDTO.UnidadMedidaDTO.Nombre,
                            f.ProductoPresentacionVentaDTO.CantidadEquivalente,
                            PorcentajeDescuento = f.PorcentajePrecioBase,
                            f.NumeroDecimalesRedondeo,
                            ValorUnitario = obtenerValorUnitario(f),
                            PrecioUnitario = decimal.Round((f.ListaPrecioProductoDTO.PrecioUnitario * f.PorcentajePrecioBase) / 100, f.NumeroDecimalesRedondeo),
                            MontoTotal = f.ProductoPresentacionVentaDTO.CantidadEquivalente * 
                              decimal.Round((f.ListaPrecioProductoDTO.PrecioUnitario * f.PorcentajePrecioBase) / 100, f.NumeroDecimalesRedondeo),
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
      private decimal obtenerValorUnitario(ListaPrecioProductoPresentacionDTO listaPrecioProductoPresentacionDTO)
      {
         decimal precioUnitario = decimal.Round(
            (listaPrecioProductoPresentacionDTO.ListaPrecioProductoDTO.PrecioUnitario *
            listaPrecioProductoPresentacionDTO.PorcentajePrecioBase) / 100, listaPrecioProductoPresentacionDTO.NumeroDecimalesRedondeo
         );


         CalculadorVentaItemDTO calculadorVentaItemDTO = CalculadorVenta.CalcularPrecios(new CalculadorVentaItemDTO()
         {
            TipoIGV = listaPrecioProductoPresentacionDTO.ProductoPresentacionVentaDTO.ProductoDTO.TipoIgvDTO.Codigo,
            ImpuestoTasa = listaPrecioProductoPresentacionDTO.ListaPrecioProductoDTO.ListaPrecioDTO.ImpuestoTasaDTO.Tasa,
            PrecioUnitario = precioUnitario
         });

         return calculadorVentaItemDTO.ValorUnitario;
      }
      [HttpPost("GrabarListaPrecioProductoPresentacion")]
      public ActionResult GrabarListaPrecioProductoPresentacion([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ListaPrecioProductoPresentacionDTO listaPrecioProductoPresentacionDTO = cargarListaPrecioProductoPresentacionDTO(model);

            int newId = model.ListaPrecioProductoPresentacionDTO.Id;
            string messageResourceKey = string.Empty;
            switch (listaPrecioProductoPresentacionDTO.DTOState)
            {
               case DTOState.Added:
                  newId = listaPrecioProductoPresentacionProxy.Insert(listaPrecioProductoPresentacionDTO);
                  messageResourceKey = "MG_producto_presentacion_agregada";
                  break;

               case DTOState.Modified:
                  listaPrecioProductoPresentacionProxy.Update(listaPrecioProductoPresentacionDTO);
                  messageResourceKey = "MG_producto_presentacion_actualizada";
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
      private ListaPrecioProductoPresentacionDTO cargarListaPrecioProductoPresentacionDTO(ListaPrecioViewModel model)
      {

         ListaPrecioProductoPresentacionDTO listaPrecioProductoDTO;
         try
         {

            listaPrecioProductoDTO = model.ListaPrecioProductoPresentacionDTO;

            AuditDTO(listaPrecioProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return listaPrecioProductoDTO;
      }
      [HttpPost("EliminarListaPrecioProductoPresentacion")]
      public ActionResult EliminarListaPrecioProductoPresentacion([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ListaPrecioProductoPresentacionDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ListaPrecioProductoPresentacionDTO);

            listaPrecioProductoPresentacionProxy.Delete(model.ListaPrecioProductoPresentacionDTO);

            resultResponse.Message = SR.GetString("MG_producto_presentacion_eliminada");
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
      [HttpPost("ListarListaPrecioProveedor")]
      public ActionResult ListarListaPrecioProveedor(string jsonModel)
      {

         Logger.InfoStart();

         ListaPrecioViewModel model = JsonSerializer.Deserialize<ListaPrecioViewModel>(jsonModel);

         model.ListaPrecioProveedorDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ListaPrecioProveedorDTO.OrderBy = "nombre";

         try
         {
            List<ListaPrecioProveedorDTO> listaPrecioProveedorDTOList = listaPrecioProveedorProxy.Search(model.ListaPrecioProveedorDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = listaPrecioProveedorDTOList.Count,
               rows = from f in listaPrecioProveedorDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ListaPrecioProveedorId,
                            f.ListaPrecioId,
                            f.GrupoProveedorId,
                            f.GrupoProveedorDTO.Nombre,
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
      [HttpPost("GrabarListaPrecioProveedor")]
      public ActionResult GrabarListaPrecioProveedor([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ListaPrecioProveedorDTO listaPrecioProveedorDTO = cargarListaPrecioProveedorDTO(model);

            int newId = model.ListaPrecioProveedorDTO.Id;
            string messageResourceKey = "VT_lista_precio_proveedor_agregado";

            newId = listaPrecioProveedorProxy.Insert(listaPrecioProveedorDTO);

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
      private ListaPrecioProveedorDTO cargarListaPrecioProveedorDTO(ListaPrecioViewModel model)
      {

         ListaPrecioProveedorDTO listaPrecioProveedorDTO;
         try
         {

            listaPrecioProveedorDTO = model.ListaPrecioProveedorDTO;
            listaPrecioProveedorDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(listaPrecioProveedorDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return listaPrecioProveedorDTO;
      }
      [HttpPost("EliminarListaPrecioProveedor")]
      public ActionResult EliminarListaPrecioProveedor([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ListaPrecioProveedorDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ListaPrecioProveedorDTO);

            listaPrecioProveedorProxy.Delete(model.ListaPrecioProveedorDTO);

            resultResponse.Message = SR.GetString("VT_lista_precio_proveedor_eliminado");
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
      [HttpPost("ListarListaPrecioProveedorProducto")]
      public ActionResult ListarListaPrecioProveedorProducto(string jsonModel)
      {

         Logger.InfoStart();

         ListaPrecioViewModel model = JsonSerializer.Deserialize<ListaPrecioViewModel>(jsonModel);

         model.ListaPrecioProveedorProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ListaPrecioProveedorProductoDTO.OrderBy = "p.descripcion";

         try
         {
            List<ListaPrecioProveedorProductoDTO> listaPrecioProveedorProductoDTOList = listaPrecioProveedorProductoProxy
               .Search(model.ListaPrecioProveedorProductoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = listaPrecioProveedorProductoDTOList.Count,
               rows = from f in listaPrecioProveedorProductoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ListaPrecioProveedorProductoId,
                            f.ListaPrecioProveedorId,
                            f.ListaPrecioProductoId,
                            f.ListaPrecioProductoDTO.ProductoDTO.Codigo,
                            f.ListaPrecioProductoDTO.ProductoDTO.Descripcion,
                            f.ListaPrecioProductoDTO.PrecioUnitario,
                            f.PorcentajePrecioBase,
                            PrecioUnitarioFinal = decimal.Round((f.ListaPrecioProductoDTO.PrecioUnitario * f.PorcentajePrecioBase)/100,1),
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
      [HttpPost("GrabarListaPrecioProveedorProducto")]
      public ActionResult GrabarListaPrecioProveedorProducto([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            ListaPrecioProveedorProductoDTO listaPrecioProveedorProductoDTO = cargarListaPrecioProveedorProductoDTO(model);

            int newId = model.ListaPrecioProveedorProductoDTO.Id;
            string messageResourceKey = string.Empty;
            switch (listaPrecioProveedorProductoDTO.DTOState)
            {
               case DTOState.Added:
                  newId = listaPrecioProveedorProductoProxy.Insert(listaPrecioProveedorProductoDTO);
                  messageResourceKey = "MG_combo_producto_agregado";
                  break;

               case DTOState.Modified:
                  listaPrecioProveedorProductoProxy.Update(listaPrecioProveedorProductoDTO);
                  messageResourceKey = "MG_combo_producto_actualizado";
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
      private ListaPrecioProveedorProductoDTO cargarListaPrecioProveedorProductoDTO(ListaPrecioViewModel model)
      {

         ListaPrecioProveedorProductoDTO listaPrecioProveedorProductoDTO;
         try
         {

            listaPrecioProveedorProductoDTO = model.ListaPrecioProveedorProductoDTO;
            listaPrecioProveedorProductoDTO.Estado = EstadoConstante.ACTIVO;

            AuditDTO(listaPrecioProveedorProductoDTO);

         }
         catch (Exception)
         {
            throw;
         }

         return listaPrecioProveedorProductoDTO;
      }
      [HttpPost("EliminarListaPrecioProveedorProducto")]
      public ActionResult EliminarListaPrecioProveedorProducto([FromBody] ListaPrecioViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            model.ListaPrecioProveedorProductoDTO.DTOState = DTOState.Modified;
            AuditDTO(model.ListaPrecioProveedorProductoDTO);

            listaPrecioProveedorProductoProxy.Delete(model.ListaPrecioProveedorProductoDTO);

            resultResponse.Message = SR.GetString("MG_combo_producto_eliminado");
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
      [Route("ListPopup")]
      public IActionResult ListPopup([FromBody] ListaPrecioViewModel model)
      {
         return PartialView(model);
      }
      [Route("SaldoListPopup")]
      public IActionResult SaldoListPopup([FromBody] ListaPrecioViewModel model)
      {

         if(model.ListaPrecioProductoDTO == null)
         {
            model.ListaPrecioProductoDTO = new ListaPrecioProductoDTO();

            string mes = string.Format(StringFormatConstant.DATE_MONTH, DateTime.Today);
            MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO() { Codigo = mes });

            model.ListaPrecioProductoDTO.MesContableId = mesContableDTO.Id;
         }

         cargarListaMoneda(model);
         cargarListaPrecio(model);
         cargarGridModel(model);

         return PartialView(model);
      }
      private void cargarListaPrecio(ListaPrecioViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         int monedaId = 0;
         if (model.ListaPrecioProductoDTO != null)
            monedaId = model.ListaPrecioProductoDTO.ListaPrecioDTO.MonedaId;
         else
            monedaId = model.ListaPrecioProveedorProductoDTO.ListaPrecioProductoDTO.ListaPrecioDTO.MonedaId;

        

         VendedorDTO vendedorDTO = vendedorProxy.SingleSearch(new VendedorDTO()
         {
            ProveedorDTO = new ProveedorDTO()
            {
                EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
                UsuarioId = ObtenerUsuarioId()
            }
         });

         List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, monedaId, vendedorDTO.VendedorId);
         model.ListaPrecioList = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         if (model.SelectedItemList == null)
            return;

         SelectedItem selectedItem = model.SelectedItemList.Where(x => x.Name == "ListaPrecioId").FirstOrDefault();
         if (selectedItem != null)
         {
            if (selectedItem.SelectedItemType == SelectedItemType.Value)
            {
               if (listaPrecioDTOList.Count > 0)
                  WebHelper.SelectedListItemSetValue(model.ListaPrecioList, Convert.ToInt32(selectedItem.Value));
            }
            if (selectedItem.SelectedItemType == SelectedItemType.FirstElement)
            {
               if (listaPrecioDTOList.Count > 0)
                  WebHelper.SelectedListItemSetValue(model.ListaPrecioList, listaPrecioDTOList[0].Id);
            }
         }
      }
        private List<ListaPrecioDTO> obtenerListaPrecio(PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO, int monedaId, int vendedorId)
        {

            List<ListaPrecioVendedorDTO> listaPrecioVendedorDTOList = listaPrecioVendedorProxy.Search(new ListaPrecioVendedorDTO()
            {
                VendedorId = vendedorId,
                ListaPrecioDTO = new ListaPrecioDTO()
                {
                    EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
                    EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                    MonedaId = monedaId,
                    Estado = EstadoConstante.ACTIVO,
                },
                Estado = EstadoConstante.ACTIVO
            });

            List<ListaPrecioDTO> listaPrecioDTOList = new List<ListaPrecioDTO>();

            if (EsAdministrador())
            {
                listaPrecioDTOList = listaPrecioProxy.Search(new ListaPrecioDTO()
                {
                    EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
                    EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
                    MonedaId = monedaId,
                    Estado = EstadoConstante.ACTIVO,
                });
            }
            else
            {
                listaPrecioDTOList = listaPrecioVendedorDTOList.Select(x => x.ListaPrecioDTO).ToList();
            }
            return listaPrecioDTOList;
        }
      private void cargarGridModel(ListaPrecioViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO()
            {
               EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            },
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            Estado = EstadoConstante.ACTIVO
         });

         string colName = ",,Código,Descripción,,,,,,,,,U.M.,,,,P.U.,,,Saldo,Reserva";

         foreach (AlmacenDTO almacenDTO in almacenDTOList)
            colName += string.Concat(SeparadorConstante.COMA, almacenDTO.Abreviatura);

         colName += ",";

         string colModel = "["
            + "{\"name\":\"ListaPrecioProductoId\",\"hidden\":true},"
            + "{\"name\":\"ProductoId\",\"hidden\":true},"
            + "{\"name\":\"Codigo\",\"width\":\"80\",\"align\":\"left\"},"
            + "{\"name\":\"Descripcion\",\"width\":\"280\",\"align\":\"left\"},"
            + "{\"name\":\"PesoNeto\",\"hidden\":true},"
            + "{\"name\":\"DetalleRequerido\",\"hidden\":true},"
            + "{\"name\":\"TipoProducto\",\"hidden\":true},"
            + "{\"name\":\"UsarIncrementoFraccion\",\"hidden\":true},"
            + "{\"name\":\"EsCombo\",\"hidden\":true},"
            + "{\"name\":\"TipoDetraccionId\",\"hidden\":true},"
            + "{\"name\":\"TasaDetraccion\",\"hidden\":true},"
            + "{\"name\":\"UnidadMedidaId\",\"hidden\":true},"
            + "{\"name\":\"UnidadMedidaNombre\",\"width\":\"45\",\"align\":\"left\"},"
            + "{\"name\":\"TipoIgvId\",\"hidden\":true},"
            + "{\"name\":\"TipoIgv\",\"hidden\":true},"
            + "{\"name\":\"ValorUnitario\",\"hidden\":true},"
            + "{\"name\":\"PrecioUnitario\",\"width\":\"65\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"},"
            + "{\"name\":\"IncluyeDescuentoListaPrecio\",\"hidden\":true},"
            + "{\"name\":\"PermiteDescuento\",\"hidden\":true},"
            + "{\"name\":\"CantidadSaldo\",\"width\":\"65\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"},"
            + "{\"name\":\"CantidadReserva\",\"width\":\"65\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"}"
            ;

         foreach (AlmacenDTO almacenDTO in almacenDTOList)
         {
            colModel += ",{\"name\":\"" + almacenDTO.Id.ToString()
               + "\",\"width\":\"70\",\"align\":\"right\",\"editable\":false,\"formatter\":\"number\",\"sorttype\":\"number\"}";
         }

         colModel += ",{\"name\":\"Estado\",\"hidden\":true}";
         colModel += "]";

         model.GridColNames = colName;
         model.GridColModel = colModel;
      }
      [HttpPost("ListarListaPrecioProductoPopup")]
      public ActionResult ListarListaPrecioProductoPopup(string jsonModel)
      {

         Logger.InfoStart();

         ListaPrecioViewModel model = JsonSerializer.Deserialize<ListaPrecioViewModel>(jsonModel);

         model.ListaPrecioProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ListaPrecioProductoDTO.PeriodoId = ObtenerPeriodoId(model.ViewModelState.CID);

         try
         {
            string result = listaPrecioProductoProxy.ListarSaldoAlmacenGrid(model.ListaPrecioProductoDTO);

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
      [HttpPost("ListarListaPrecio")]
      public JsonResult ListarListaPrecio([FromBody] ListaPrecioViewModel model)
      {

         Logger.InfoStart();
         ResultResponse resultResponse = new();

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);
            
            VendedorDTO vendedorDTO = vendedorProxy.SingleSearch(new VendedorDTO() { 
               ProveedorDTO = new ProveedorDTO()
               {
                  EmpresaId = ObtenerEmpresaId(model.ViewModelState.CID),
                  UsuarioId = ObtenerUsuarioId()
               }
            });

            List<ListaPrecioDTO> listaPrecioDTOList = obtenerListaPrecio(periodoEmpresaSedeDTO, model.ListaPrecioDTO.MonedaId, vendedorDTO.VendedorId);

            resultResponse.Data = new
            {
               List = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select)
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
      [Route("SaldoProveedorListPopup")]
      public IActionResult SaldoProveedorListPopup([FromBody] ListaPrecioViewModel model)
      {

         if (model.ListaPrecioProveedorProductoDTO.ListaPrecioProductoDTO == null)
         {
            model.ListaPrecioProveedorProductoDTO.ListaPrecioProductoDTO = new ListaPrecioProductoDTO();

            string mes = string.Format(StringFormatConstant.DATE_MONTH, DateTime.Today);
            MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO() { Codigo = mes });

            model.ListaPrecioProveedorProductoDTO.ListaPrecioProductoDTO.MesContableId = mesContableDTO.Id;
         }

         cargarListaMoneda(model);
         cargarListaPrecio(model);
         cargarGridModel(model);

         return PartialView(model);
      }
      [HttpPost("ListarListaPrecioProveedorProductoPopup")]
      public ActionResult ListarListaPrecioProveedorProductoPopup(string jsonModel)
      {

         Logger.InfoStart();

         ListaPrecioViewModel model = JsonSerializer.Deserialize<ListaPrecioViewModel>(jsonModel);

         model.ListaPrecioProveedorProductoDTO.ListaPrecioProductoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ListaPrecioProveedorProductoDTO.ListaPrecioProductoDTO.PeriodoId = ObtenerPeriodoId(model.ViewModelState.CID);

         try
         {
            string result = listaPrecioProveedorProductoProxy.ListarSaldoAlmacenGrid(model.ListaPrecioProveedorProductoDTO);

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