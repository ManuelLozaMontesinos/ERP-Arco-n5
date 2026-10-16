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
   public class RepoKardexController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly IMesProxy mesProxy;
      private readonly AppHelper appHelper;

      public RepoKardexController(
         IAlmacenProxy almacenProxy,
         IMesProxy mesProxy,
         AppHelper appHelper
         )
      {
         this.almacenProxy = almacenProxy;
         this.mesProxy = mesProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReporteGenViewModel reporteGenViewModel = new()
         {
            Title = "Kardex de Unidades",
            Fecha = DateTime.Today
         };

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

         AlmacenDTO almacenDTO = new()
         {
            EmpresaSedeDTO = new EmpresaSedeDTO() { EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId },
            Estado = EstadoConstante.ACTIVO
         };

         if (!EsAdministrador())
            almacenDTO.EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId;

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(almacenDTO);
         reporteGenViewModel.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         List<MesDTO> mesDTOList = mesProxy.Search(new MesDTO()
         {
            OrderBy = "m.mes",
            Estado = EstadoConstante.ACTIVO,
         });

         reporteGenViewModel.MesList = WebHelper.ToSelectListItem(mesDTOList, x => x.Id.ToString(), x => x.Nombre, SelectListFirstOptionType.Select);

         return PartialView(reporteGenViewModel);
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string almacenId, string anual, string mesId, string fecha, string productoId)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, almacenId, anual, mesId, fecha, productoId);

         string reportName = string.Empty;
         string dataName = string.Empty;
         
         if(anual == SiNoConstante.N){

            reportName = string.IsNullOrEmpty(mesId) ? "LG_KardexUnidadFecha" : "LG_KardexUnidadMensual";
            dataName = string.IsNullOrEmpty(mesId) ? "sp_rptiv_kardexUnidadFecha" : "sp_rptiv_kardexUnidadMensual";
         }else{
            
            reportName = "LG_KardexUnidadAnual";
            dataName = "sp_rptiv_kardexUnidadAnual";
         }
         
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
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string almacenId, string anual, string mesId, string fecha, string productoId)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "periodoId", periodoEmpresaSedeDTO.PeriodoId.ToString() },
            { "almacenId", almacenId }
         };

         if (!string.IsNullOrEmpty(mesId))
         {
            MesDTO mesDTO = mesProxy.GetById(new MesDTO() { MesId = Convert.ToInt32(mesId) });
            parameters.Add("mes", mesDTO.Codigo);
         }

         if (!string.IsNullOrEmpty(fecha))
         {
            DateTime date = Convert.ToDateTime(fecha);
            fecha = string.Format(StringFormatConstant.DATE_ISO, date);
            parameters.Add("fecha", fecha);
         }

         if (!string.IsNullOrEmpty(productoId))
            parameters.Add("productoId", productoId);

         return parameters;
      }
   }
}
