using ESD.Core;
using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class CuadroCostoController : AuthorizedController
   {
      private readonly IListaPrecioProxy listaPrecioProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly ICuadroCostoProxy cuadroCostoProxy;
      private readonly AppHelper appHelper;
      public CuadroCostoController(
         IListaPrecioProxy listaPrecioProxy,
         IMesContableProxy mesContableProxy,
         ICuadroCostoProxy cuadroCostoProxy,
         AppHelper appHelper
         )
      {
         this.listaPrecioProxy = listaPrecioProxy;
         this.mesContableProxy = mesContableProxy;
         this.cuadroCostoProxy = cuadroCostoProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         CuadroCostoViewModel cuadroCostoViewModel = new();

         cargarListas(cuadroCostoViewModel);

         cuadroCostoViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.ExportDataXLS}
         };

         return PartialView(cuadroCostoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         CuadroCostoViewModel model = JsonSerializer.Deserialize<CuadroCostoViewModel>(jsonModel);

         model.CuadroCostoDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.CuadroCostoDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);

         string mesContable = string.Format(StringFormatConstant.DATE_MONTH, DateTime.Today);

         MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO() { Codigo = mesContable });

         model.CuadroCostoDTO.MesContableId = mesContableDTO.Id;

         try
         {
            List<CuadroCostoDTO> cuadroCostoDTOList = cuadroCostoProxy.Search(model.CuadroCostoDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = cuadroCostoDTOList.Count,
               rows = from f in cuadroCostoDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.CuadroCostoId,
                            Producto = f.ProductoDTO.Codigo,
                            f.ProductoDTO.Descripcion,
                            f.ValorPromedio,
                            FechaUltimaCompra = f.FechaUltimaCompra == null ? string.Empty : f.FechaUltimaCompra.Value.ToShortDateString(),
                            f.ValorUltimaCompra,
                            f.PrecioUnitario,
                            f.MargenA,
                            f.MargenB
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
      private void cargarListas(CuadroCostoViewModel model)
      {

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();
         List<ListaPrecioDTO> listaPrecioDTOList = listaPrecioProxy.Search(new ListaPrecioDTO()
         {
            EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            Estado = EstadoConstante.ACTIVO
         });

         model.ListaPrecioList = WebHelper.ToSelectListItem(listaPrecioDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string listaPrecioId)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, listaPrecioId);

         string reportName = "VT_CuadroCosto";
         string dataName = "sp_rptvt_cuadroCosto";
         string downloadFileName = reportName;

         FileContentResult fileContentResult = appHelper.ReportToExcelData(dataName, parameters, downloadFileName); ;

         return fileContentResult;
      }
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string listaPrecioId)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         string mesContable = string.Format(StringFormatConstant.DATE_MONTH, DateTime.Today);

         MesContableDTO mesContableDTO = mesContableProxy.ObtenerPorCodigo(new MesContableDTO() { Codigo = mesContable });

         Dictionary<string, string> parameters = new()
         {
            { "periodoEmpresaSedeId", periodoEmpresaSedeDTO.PeriodoEmpresaSedeId.ToString() },
            { "listaPrecioId", listaPrecioId },
            { "mesContableId", mesContableDTO.Id.ToString() }
         };

         return parameters;
      }

   }
}
