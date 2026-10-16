using ESD.Core;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.PD.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using ESD.ERP.WebApiProxy.Impl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.PD.Controllers
{
    [Area("PD")]
    [Route("[controller]")]
    public class RepoProceProdController : AuthorizedController
    {
        private readonly IMesProxy mesProxy;
        private readonly IProcesoProduccionProxy procesoProduccionProxy;
        private readonly ISubProcesoProduccionProxy subProcesoProduccionProxy;
        private readonly AppHelper appHelper;

        public RepoProceProdController(
           IMesProxy mesProxy,
           IProcesoProduccionProxy procesoProduccionProxy,
           ISubProcesoProduccionProxy subProcesoProduccionProxy,
           AppHelper appHelper
        )

        {
            this.procesoProduccionProxy = procesoProduccionProxy;
            this.mesProxy = mesProxy;
            this.appHelper = appHelper;
        }
        [Route("[action]")]
        public IActionResult Index()
        {
            ReporteGenViewModel reporteGenViewModel = new()
            {
                Title = "Procesos de Produccion",
                Fecha = DateTime.Today
            };

            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO();

            List<MesDTO> mesDTOList = mesProxy.Search(new MesDTO()
            {
                OrderBy = "m.mes",
                Estado = EstadoConstante.ACTIVO,
            });

            reporteGenViewModel.MesList = WebHelper.ToSelectListItem(mesDTOList, x => x.Id.ToString(), x => x.Nombre,
               SelectListFirstOptionType.Select);

            return PartialView(reporteGenViewModel);
        }
        [HttpGet("ReportTo")]
        public FileContentResult ReportTo(string format, string cid, string mesId, string fechaDesde, string fechaHasta, string resumen, string detallado)
        {
            Dictionary<string, string> parameters = obtenerParametrosReporte(cid, mesId, fechaDesde, fechaHasta);

            string reportName = string.Empty;
            string dataName = string.Empty;

            if (resumen == SiNoConstante.S)
                dataName = "sp_rptpd_procesosProduccion";
                reportName = "PD_ProcesosProduccionResumen";

            if (detallado == SiNoConstante.S)
                dataName = "sp_rptpd_procesosProduccionDetallado";
                reportName = "PD_ProcesosProduccionDetallado";

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
        private Dictionary<string, string> obtenerParametrosReporte(string cid, string mesId, string fechaDesde, string fechaHasta)
        {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

            Dictionary<string, string> parameters = new()
            {
            { "periodoEmpresaSedeId", periodoEmpresaSedeDTO.PeriodoEmpresaSedeId.ToString() }
            };

            if (!string.IsNullOrEmpty(mesId))
            {
                MesDTO mesDTO = mesProxy.GetById(new MesDTO() { MesId = Convert.ToInt32(mesId) });
                mesId = mesDTO.Codigo;
            }
            parameters.Add("mes", mesId);

            if (!string.IsNullOrEmpty(fechaDesde))
            {
                DateTime date = Convert.ToDateTime(fechaDesde);
                fechaDesde = string.Format(StringFormatConstant.DATE_ISO, date);
            }
            else
                fechaDesde = string.Empty;
            parameters.Add("fechaDesde", fechaDesde);

            if (!string.IsNullOrEmpty(fechaHasta))
            {
                DateTime date = Convert.ToDateTime(fechaHasta);
                fechaHasta = string.Format(StringFormatConstant.DATE_ISO, date);
            }
            else
                fechaHasta = string.Empty;
            parameters.Add("fechaHasta", fechaHasta);

            return parameters;
        }
    }
}
