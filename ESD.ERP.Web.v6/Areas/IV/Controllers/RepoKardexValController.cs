using ESD.Core;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("IV")]
   [Route("[controller]")]
   public class RepoKardexValController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly AppHelper appHelper;

      public RepoKardexValController(
         IAlmacenProxy almacenProxy,
         IMesContableProxy mesContableProxy,
         AppHelper appHelper
         )
      {
         this.almacenProxy = almacenProxy;
         this.mesContableProxy = mesContableProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReporteGenViewModel reporteGenViewModel = new()
         {
            Title = "Kardex Valorizado",
            Fecha = DateTime.Today
         };

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

         AlmacenDTO almacenDTO = new()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
            Estado = EstadoConstante.ACTIVO
         };

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(almacenDTO);
         reporteGenViewModel.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MesContableDTO> mesContableDTOList = mesContableProxy.Search(new MesContableDTO()
         {
            OrderBy = "mc.mes_contable",
            Estado = EstadoConstante.ACTIVO,
         });

         reporteGenViewModel.MesList = WebHelper.ToSelectListItem(mesContableDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         return PartialView(reporteGenViewModel);
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string almacenId, string mesId, string fecha, string productoId)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, almacenId, mesId, fecha, productoId);

         string reportName = "LG_KardexConsolidado";
         string dataName = "sp_rptiv_kardexValorizado";
         string downloadFileName = reportName;

         FileContentResult fileContentResult = null;
         switch (format)
         {
            case ReportFormatConstant.PDF:
               fileContentResult = appHelper.ReportToPdf(reportName, parameters, downloadFileName);
               break;

            case ReportFormatConstant.EXCEL:
               fileContentResult = appHelper.ReportToExcel(reportName, parameters, downloadFileName);
               break;

            case ReportFormatConstant.EXCELDATA:
               fileContentResult = appHelper.ReportToExcelData(dataName, parameters, downloadFileName);
               break;
         }

         return fileContentResult;
      }
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string almacenId, string mesId, string fecha, string productoId)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);
         MesContableDTO mesContableDTO = mesContableProxy.GetById(new MesContableDTO() { MesContableId = Convert.ToInt32(mesId) });

         Dictionary<string, string> parameters = new()
         {
            { "periodoId", periodoEmpresaSedeDTO.PeriodoId.ToString() },
            { "almacenId", almacenId },
            { "mes", mesContableDTO.Codigo }
         };

         return parameters;
      }
   }
}
