using ESD.Core;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("IV")]
   [Route("[controller]")]
   public class RepoMoviAlmaController : AuthorizedController
   {
      private readonly IAlmacenProxy almacenProxy;
      private readonly ITransaccionAlmacenProxy transaccionAlmacenProxy;
      private readonly IMesProxy mesProxy;
      private readonly AppHelper appHelper;

      public RepoMoviAlmaController(
         IAlmacenProxy almacenProxy,
         ITransaccionAlmacenProxy transaccionAlmacenProxy,
         IMesProxy mesProxy,
         AppHelper appHelper
         )
      {
         this.almacenProxy = almacenProxy;
         this.transaccionAlmacenProxy = transaccionAlmacenProxy;
         this.mesProxy = mesProxy;
         this.appHelper = appHelper;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReporteGenViewModel reporteGenViewModel = new()
         {
            Title = "Movimientos de Almacen",
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

         List<TransaccionAlmacenDTO> transaccionAlmacenDTOList = transaccionAlmacenProxy.Search(new TransaccionAlmacenDTO()
         {
            EmpresaId = periodoEmpresaSedeDTO.EmpresaSedeDTO.EmpresaId,
            NotTransaccionAlmacenList = new List<string>() { TransaccionAlmacenConstante.TRANSFERENCIA_ENTRE_ALMACENES },
            Estado = EstadoConstante.ACTIVO
         });
         reporteGenViewModel.TransaccionAlmacenList = WebHelper.ToSelectListItem(transaccionAlmacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

         reporteGenViewModel.GrupoTransaccionAlmacenList = WebHelper.ToSelectListItem(transaccionAlmacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);

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
      public FileContentResult ReportTo(string format, string cid, string almacenId, string transaccionAlmacenId, string mesId, string fecha, 
         string proveedorId)
      {
         Dictionary<string, string> parameters = obtenerParametrosReporte(cid, almacenId, transaccionAlmacenId, mesId, fecha, proveedorId);

         string reportName = "IV_MovimientoAlmacen";
         string dataName = "sp_rptiv_movimientoAlmacen"; 
         string TA = transaccionAlmacenId;
         string downloadFileName = reportName;

         if (TA == "7" || TA == "8" || TA == "9")
         {
            dataName = "sp_rptiv_movimientoAlmacen2";
         }

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
      private Dictionary<string, string> obtenerParametrosReporte(string cid, string almacenId, string transaccionAlmacenId, string mesId, string fecha, 
         string proveedorId)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(cid);

         Dictionary<string, string> parameters = new()
         {
            { "periodoEmpresaSedeId", periodoEmpresaSedeDTO.Id.ToString() },
            { "almacenId", almacenId },
            { "transaccionAlmacenId", transaccionAlmacenId },
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

         if (!string.IsNullOrEmpty(proveedorId))
            parameters.Add("proveedorId", proveedorId);

         return parameters;
      }
   }
}
