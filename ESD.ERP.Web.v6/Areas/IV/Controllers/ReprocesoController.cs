using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.IV.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.IV.Controllers
{
   [Area("IV")]
   [Route("[controller]")]
   public class ReprocesoController : AuthorizedController
   {

      private readonly IMovimientoAlmacenCierreProxy movimientoAlmacenCierreProxy;
      private readonly IAlmacenProxy almacenProxy;

      public ReprocesoController(
         IMovimientoAlmacenCierreProxy movimientoAlmacenCierreProxy,
         IAlmacenProxy almacenProxy
         )
      {
         this.movimientoAlmacenCierreProxy = movimientoAlmacenCierreProxy;
         this.almacenProxy = almacenProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ReprocesoViewModel reprocesoViewModel = new();

         cargarListas(reprocesoViewModel);

         reprocesoViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key = "reproceso", Content = "Reprocesar" },
            new MenuItem() { Key = "reproceso_anual", Content = "Reproceso Anual"}
         };

         return PartialView(reprocesoViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ReprocesoViewModel model = JsonSerializer.Deserialize<ReprocesoViewModel>(jsonModel);

         model.MovimientoAlmacenCierreDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.MovimientoAlmacenCierreDTO.OrderBy = "mc.mes_contable";

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         model.MovimientoAlmacenCierreDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.Id;

         try
         {
            List<MovimientoAlmacenCierreDTO> movimientoAlmacenCierreDTOList = movimientoAlmacenCierreProxy.Search(model.MovimientoAlmacenCierreDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = movimientoAlmacenCierreDTOList.Count,
               rows = from f in movimientoAlmacenCierreDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.MovimientoAlmacenCierreId,
                            f.MesContableId,
                            MesContable = f.MesContableDTO.Codigo,
                            MesContableNombre = f.MesContableDTO.Nombre,
                            FechaCierre = f.FechaCierre == null ? string.Empty : f.FechaCierre.Value.ToShortDateString(),
                            EstadoAperturaCierre = f.EstadoAperturaCierreDTO.Codigo,
                            EstadoAperturaCierreNombre = f.EstadoAperturaCierreDTO.Nombre
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
      private void cargarListas(ReprocesoViewModel model)
      {
         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<AlmacenDTO> almacenDTOList = almacenProxy.Search(new AlmacenDTO() { 
            EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
            EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
            Estado = EstadoConstante.ACTIVO 
         });
         model.AlmacenList = WebHelper.ToSelectListItem(almacenDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("Reprocesar")]
      public ActionResult Reprocesar([FromBody] ReprocesoViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            model.MovimientoAlmacenCierreDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.Id;

            AuditDTO(model.MovimientoAlmacenCierreDTO);

            movimientoAlmacenCierreProxy.Reprocesar(model.MovimientoAlmacenCierreDTO);

            resultResponse.Message = SR.GetString("IV_mes_reprocesado", model.MovimientoAlmacenCierreDTO.MesContableDTO.Nombre);
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
        [HttpPost("ReprocesoAnual")]
        public ActionResult ReprocesoAnual([FromBody] ReprocesoViewModel model)
        {
            Logger.InfoStart();

            ResultResponse resultResponse = new();

            try
            {
                PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

                model.MovimientoAlmacenCierreDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.Id;

                AuditDTO(model.MovimientoAlmacenCierreDTO);

                movimientoAlmacenCierreProxy.ReprocesoAnual(model.MovimientoAlmacenCierreDTO);

                resultResponse.Message = SR.GetString("IV_mes_reprocesado_anual");
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
    }
}