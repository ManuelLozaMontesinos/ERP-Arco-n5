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
   public class ValorizaController : AuthorizedController
   {

      private readonly IMovimientoAlmacenCierreProxy movimientoAlmacenCierreProxy;
      private readonly IAlmacenProxy almacenProxy;
      private readonly IMovimientoAlmacenSaldoConsolidadoProxy movimientoAlmacenSaldoConsolidadoProxy;
      private readonly IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy;

      public ValorizaController(
         IMovimientoAlmacenCierreProxy movimientoAlmacenCierreProxy,
         IAlmacenProxy almacenProxy,
         IMovimientoAlmacenSaldoConsolidadoProxy movimientoAlmacenSaldoConsolidadoProxy,
         IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy
         )
      {
         this.movimientoAlmacenCierreProxy = movimientoAlmacenCierreProxy;
         this.almacenProxy = almacenProxy;
         this.movimientoAlmacenSaldoConsolidadoProxy = movimientoAlmacenSaldoConsolidadoProxy;
         this.periodoEmpresaSedeProxy = periodoEmpresaSedeProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         ValorizaViewModel valorizaViewModel = new();

         cargarListas(valorizaViewModel);

         valorizaViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key = "reproceso", Content = "Valorizar" }
         };

         return PartialView(valorizaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ValorizaViewModel model = JsonSerializer.Deserialize<ValorizaViewModel>(jsonModel);

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
      private void cargarListas(ValorizaViewModel model)
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
      [HttpPost("Valorizar")]
      public ActionResult Valorizar([FromBody] ValorizaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            int periodoId = periodoEmpresaSedeDTO.PeriodoId;
            int empresaId = periodoEmpresaSedeDTO.EmpresaSedeId;

            periodoEmpresaSedeDTO = periodoEmpresaSedeProxy.SingleSearch(new PeriodoEmpresaSedeDTO()
            {
               PeriodoId = periodoId,
               EmpresaSedeDTO = new EmpresaSedeDTO() { 
                  EsCentral = SiNoConstante.S,
                  EmpresaId = empresaId
               }
            });

            AlmacenDTO almacenDTO = almacenProxy.SingleSearch(new AlmacenDTO() { 
               EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
               EmpresaSedeId = periodoEmpresaSedeDTO.EmpresaSedeId,
               EsVenta = SiNoConstante.S
            });

            model.MovimientoAlmacenSaldoConsolidadoDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.Id;
            model.MovimientoAlmacenSaldoConsolidadoDTO.AlmacenId = almacenDTO.Id;

            AuditDTO(model.MovimientoAlmacenCierreDTO);

            movimientoAlmacenSaldoConsolidadoProxy.Valorizar(model.MovimientoAlmacenSaldoConsolidadoDTO);

            resultResponse.Message = SR.GetString("IV_mes_valorizado", model.MovimientoAlmacenCierreDTO.MesContableDTO.Nombre);
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