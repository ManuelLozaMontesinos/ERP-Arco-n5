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
   public class RepoSaldoController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly IMesContableProxy mesContableProxy;
      private readonly AppHelper appHelper;

      public RepoSaldoController(
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
            Title = "Saldo de Unidades",
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

         List<MesContableDTO> mesContableDTOList = mesContableProxy.Search(new MesContableDTO()
         {
            OrderBy = "mc.mes_contable",
            NotMesContableList = new List<string>() { MesContableConstante.APERTURA,MesContableConstante.CIERRE},
            Estado = EstadoConstante.ACTIVO,
         });

         reporteGenViewModel.MesList = WebHelper.ToSelectListItem(mesContableDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         return PartialView(reporteGenViewModel);
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string tipoReporte, string almacenId, string mesId, string fecha, 
         string productoId, string incluirReserva)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, tipoReporte,almacenId, mesId, fecha, productoId, incluirReserva);

         string reportName = string.Empty;
         string dataName = string.Empty;

         switch (tipoReporte)
         {
            case DigitoConstante.UNO:
               reportName = "LG_SaldoUnidadConsolidado";
               dataName = "sp_rptiv_saldoUnidadConso";
               break;
            case DigitoConstante.DOS:
               reportName = "LG_SaldoConsoAlmacen";
               dataName = "sp_rptiv_saldoConsoAlmacen";
               break;
            case DigitoConstante.TRES:
               reportName = "LG_SaldoUnidad";
               dataName = "sp_rptiv_saldoUnidad";
               break;
         }

         string downloadFileName = reportName;

         FileContentResult fileContentResult = null;
         switch (format)
         {
            case ReportFormatConstant.PDF:
               fileContentResult = appHelper.ReportToPdf(reportName, parameters, downloadFileName);
               break;

            case ReportFormatConstant.EXCEL:
            case ReportFormatConstant.EXCELDATA:
               if(tipoReporte == DigitoConstante.DOS)
                  fileContentResult = appHelper.ReportToExcel(reportName, parameters, downloadFileName);
               else
                  fileContentResult = appHelper.ReportToExcelData(dataName, parameters, downloadFileName);
               break;
         }

         return fileContentResult;
      }
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string tipoReporte, string almacenId, string mesId, string fecha, 
         string productoId, string incluirReserva)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "periodoId", periodoEmpresaSedeDTO.PeriodoId.ToString() }
         };

         switch (tipoReporte)
         {
            case DigitoConstante.UNO:
               parameters.Add("mesContableId", mesId);
               if (!string.IsNullOrEmpty(productoId))
                  parameters.Add("productoId", productoId);
               break;

            case DigitoConstante.DOS:
               parameters.Add("mesContableId", mesId);
               if (!string.IsNullOrEmpty(productoId))
                  parameters.Add("productoId", productoId);
               parameters.Add("incluirReserva", incluirReserva);
               break;

            case DigitoConstante.TRES:
               parameters.Add("almacenId", almacenId);
               if (string.IsNullOrEmpty(mesId))
                  mesId = DigitoConstante.CERO;

               parameters.Add("mesContableId", mesId);

               DateTime date;
               if (mesId != DigitoConstante.CERO)
               {
                  MesContableDTO mesContableDTO = mesContableProxy.GetById(new MesContableDTO() { MesContableId = Convert.ToInt32(mesId) });
                  DateTime newDate = new(periodoEmpresaSedeDTO.PeriodoDTO.Codigo, Convert.ToInt32(mesContableDTO.Codigo), 1);
                  date = newDate.AddMonths(1).AddDays(-1);
               }
               else
                  date = Convert.ToDateTime(fecha);

               fecha = string.Format(StringFormatConstant.DATE_ISO, date);
               parameters.Add("fecha", fecha);

               if (!string.IsNullOrEmpty(productoId))
                  parameters.Add("productoId", productoId);
               break;
         }

         return parameters;
      }
   }
}
