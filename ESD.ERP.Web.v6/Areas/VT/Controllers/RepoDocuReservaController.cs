using ESD.Core;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
    [Area("VT")]
    [Route("[controller]")]
    public class RepoDocuReservaController : AuthorizedController
    {
        private readonly IMesProxy mesProxy;
        private readonly AppHelper appHelper;

        public RepoDocuReservaController(
            IMesProxy mesProxy,
            AppHelper appHelper
           )
        {
            this.mesProxy = mesProxy;
            this.appHelper = appHelper;
        }
        [Route("[action]")]
        public IActionResult Index()
        {
            ReporteGenViewModel reporteGenViewModel = new()
            {
                Title = "Documentos con Reserva"
            };

            return PartialView(reporteGenViewModel);
        }
        [HttpGet("ReportTo")]
        public FileContentResult ReportTo(string format, string cid, string tipoGrupo, string vendedorId)
        {
            Dictionary<string, string> parameters = obtenerParametrosReporte(cid, tipoGrupo, vendedorId);

            string reportName = (tipoGrupo == DigitoConstante.CERO ? "VT_DocSaldoReservado" : "VT_DocSaldoResPorTipo");
            string dataName = "sp_rptvt_documentoSaldoReservado";
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
        private Dictionary<string, string> obtenerParametrosReporte(string cid, string tipoGrupo, string vendedorId)
        {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

            Dictionary<string, string> parameters = new()
         {
            { "periodoEmpresaSedeId", periodoEmpresaSedeDTO.PeriodoEmpresaSedeId.ToString() }
         };

            if (!string.IsNullOrEmpty(vendedorId))
                parameters.Add("vendedorId", vendedorId);
            else
                parameters.Add("vendedorId", null);

            return parameters;
        }
    }
}
