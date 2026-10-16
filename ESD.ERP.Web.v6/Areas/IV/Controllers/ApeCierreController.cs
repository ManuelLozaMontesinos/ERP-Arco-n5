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
   public class ApeCierreController : AuthorizedController
   {

      private readonly IMovimientoAlmacenCierreProxy movimientoAlmacenCierreProxy;
      private readonly IAlmacenProxy almacenProxy;

      public ApeCierreController(
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
         ApeCierreViewModel apeCierreViewModel = new ApeCierreViewModel();

         cargarListas(apeCierreViewModel);

         apeCierreViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key = "apertura", Content = "Apertura" },
            new MenuItem() { Key = "cierre", Content = "Cierre" }
         };

         return PartialView(apeCierreViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         ApeCierreViewModel model = JsonSerializer.Deserialize<ApeCierreViewModel>(jsonModel);

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
      private void cargarListas(ApeCierreViewModel model)
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
      [HttpPost("Aperturar")]
      public ActionResult Aperturar([FromBody] ApeCierreViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            model.MovimientoAlmacenCierreDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.Id;

            AuditDTO(model.MovimientoAlmacenCierreDTO);

            movimientoAlmacenCierreProxy.Aperturar(model.MovimientoAlmacenCierreDTO);

            resultResponse.Message = SR.GetString("IV_mes_aperturado", model.MovimientoAlmacenCierreDTO.MesContableDTO.Nombre);
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
      [HttpPost("Cerrar")]
      public ActionResult Cerrar([FromBody] ApeCierreViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

            model.MovimientoAlmacenCierreDTO.PeriodoEmpresaSedeId = periodoEmpresaSedeDTO.Id;

            AuditDTO(model.MovimientoAlmacenCierreDTO);

            movimientoAlmacenCierreProxy.Cerrar(model.MovimientoAlmacenCierreDTO);

            resultResponse.Message = SR.GetString("IV_mes_cerrado", model.MovimientoAlmacenCierreDTO.MesContableDTO.Nombre);
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