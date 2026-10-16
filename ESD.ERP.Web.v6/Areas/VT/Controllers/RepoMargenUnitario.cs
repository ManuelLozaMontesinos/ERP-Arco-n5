using ESD.Core;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class RepoMargenUnitarioController : AuthorizedController
   {
      private readonly IMesContableProxy mesContableProxy;
      private readonly IGrupoProductoProxy grupoProductoProxy;
      private readonly AppHelper appHelper;

      public RepoMargenUnitarioController(
         IMesContableProxy mesContableProxy,
         IGrupoProductoProxy grupoProductoProxy,
         AppHelper appHelper
         )
      {
         this.mesContableProxy = mesContableProxy;
         this.grupoProductoProxy = grupoProductoProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReporteGenViewModel reporteGenViewModel = new()
         {
            Title = "Margenes Unitarios",
            Fecha = DateTime.Today
         };

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

         List<MesContableDTO> mesContableDTOList = mesContableProxy.Search(new MesContableDTO()
         {
            OrderBy = "mc.mes_contable",
            NotMesContableList = new List<string>() { MesContableConstante.APERTURA, MesContableConstante.CIERRE },
            Estado = EstadoConstante.ACTIVO,
         });
         reporteGenViewModel.SeleccionList = WebHelper.ToSelectListItem(mesContableDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         List<GrupoProductoDTO> grupoProductoDTOList = grupoProductoProxy.Search(new GrupoProductoDTO()
         {
            OrderBy = "gp.grupo_producto",
            TipoProductoDTO = new TipoProductoDTO { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId},
            Estado = EstadoConstante.ACTIVO,
         });
         reporteGenViewModel.Seleccion2List = WebHelper.ToSelectListItem(grupoProductoDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         return PartialView(reporteGenViewModel);
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string mesId, string grupoProductoId)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, mesId, grupoProductoId);

         string reportName = "VT_MargenUnitario";
         string dataName = "sp_rptvt_margenUnitario";
         string downloadFileName = reportName;

         FileContentResult fileContentResult = null;
         switch (format)
         {
            case ReportFormatConstant.PDF:
               fileContentResult = appHelper.ReportToPdf(reportName, parameters, downloadFileName);
               break;

            case ReportFormatConstant.EXCEL:
            case ReportFormatConstant.EXCELDATA:
               fileContentResult = appHelper.ReportToExcelData(dataName, parameters, downloadFileName);
               break;
         }

         return fileContentResult;
      }
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string mesId, string grupoProductoId)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "periodoId", periodoEmpresaSedeDTO.PeriodoId.ToString() },
            { "mesContableId", mesId }
         };

         if(!string.IsNullOrEmpty(grupoProductoId))
            parameters.Add("grupoProductoId", grupoProductoId);

         return parameters;
      }
   }
}
