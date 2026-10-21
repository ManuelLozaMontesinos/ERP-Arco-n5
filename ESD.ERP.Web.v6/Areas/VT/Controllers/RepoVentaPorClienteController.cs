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
   public class RepoVentaPorClienteController : AuthorizedController
   {
      private readonly IMesContableProxy mesContableProxy;
      private readonly AppHelper appHelper;

      public RepoVentaPorClienteController(
         IMesContableProxy mesContableProxy,
         AppHelper appHelper
         )
      {
         this.mesContableProxy = mesContableProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReporteGenViewModel reporteGenViewModel = new()
         {
            Title = "Ventas por Cliente",
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

         return PartialView(reporteGenViewModel);
      }
      [HttpGet("ReportTo")]
      public FileContentResult ReportTo(string format, string cid, string tipoReporte, string mesContableId, string fechaDesde, string fechaHasta, string excluirObsequio)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, tipoReporte, mesContableId, fechaDesde, fechaHasta, excluirObsequio);

         string reportName = string.Empty;
         string dataName = string.Empty;

            switch (tipoReporte)
            {
                case DigitoConstante.UNO:

                    switch (excluirObsequio)
                    {
                        case SiNoConstante.S:
                            reportName = "VT_VentaClienteRegistro";
                            dataName = "sp_rptvt_ventaPorClienteSinObsequios";
                            break;

                        case SiNoConstante.N:
                            reportName = "VT_VentaClienteRegistro";
                            dataName = "sp_rptvt_ventaPorCliente";
                            break;
                    }
                break;

                case DigitoConstante.DOS:

                    switch (excluirObsequio)
                    {
                        case SiNoConstante.S:
                            reportName = "VT_VentaClienteConsolidado";
                            dataName = "sp_rptvt_ventaPorClienteConsolidadoSinObsequios";
                            break;

                        case SiNoConstante.N:
                            reportName = "VT_VentaClienteConsolidado";
                            dataName = "sp_rptvt_ventaPorClienteConsolidado";
                            break;
                    }
                break;

                case DigitoConstante.TRES:

                    switch (excluirObsequio)
                    {
                        case SiNoConstante.S:
                            reportName = "VT_VentaClientePorProducto";
                            dataName = "sp_rptvt_ventaPorClienteProductoSinObsequios";
                            break;

                        case SiNoConstante.N:
                            reportName = "VT_VentaClientePorProducto";
                            dataName = "sp_rptvt_ventaPorClienteProducto";
                            break;
                    }
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
               fileContentResult = appHelper.ReportToExcelData(dataName, parameters, downloadFileName);
               break;
         }

         return fileContentResult;
      }
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string tipoReporte, string mesContableId, string fechaDesde, string fechaHasta, string excluirObsequio)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "periodoEmpresaSedeId", periodoEmpresaSedeDTO.Id.ToString() }
         };

         if (!string.IsNullOrEmpty(mesContableId))
         {
            MesContableDTO mesContableDTO = mesContableProxy.GetById(new MesContableDTO() { MesContableId = Convert.ToInt32(mesContableId) });
            parameters.Add("mes", mesContableDTO.Codigo);
         }
         else
            parameters.Add("mes", null);

         string fechaDesdeIso = string.Empty;
         if (!string.IsNullOrEmpty(fechaDesde))
         {
            DateTime date = Convert.ToDateTime(fechaDesde);
            fechaDesdeIso = string.Format(StringFormatConstant.DATE_ISO, date);
         }
         parameters.Add("fechaDesde", fechaDesdeIso);

         string fechaHastaIso = string.Empty;
         if (!string.IsNullOrEmpty(fechaHasta))
         {
            DateTime date = Convert.ToDateTime(fechaHasta);
            fechaHastaIso = string.Format(StringFormatConstant.DATE_ISO, date);
         }
         parameters.Add("fechaHasta", fechaHastaIso);

         parameters.Add("incluirInterno", SiNoConstante.S);

         return parameters;
      }
   }
}
