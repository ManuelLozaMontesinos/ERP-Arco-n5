using ESD.Core.Logging;
using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using ESD.ERP.Web.Areas.VT.Models;
using ESD.ERP.Web.Infrastructure;
using ESD.ERP.WebApiProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ESD.ERP.Web.Areas.VT.Controllers
{
   [Area("VT")]
   [Route("[controller]")]
   public class DetraccionController : AuthorizedController
   {
      private readonly IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy;
      private readonly IComprobanteProxy comprobanteProxy;

      public DetraccionController(
         IPeriodoEmpresaSedeProxy periodoEmpresaSedeProxy,
         IComprobanteProxy comprobanteProxy
         )
      {
         this.periodoEmpresaSedeProxy = periodoEmpresaSedeProxy;
         this.comprobanteProxy = comprobanteProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         DetraccionViewModel detraccionViewModel = new DetraccionViewModel();

         cargarListaPeriodoEmpresaSede(detraccionViewModel);

         detraccionViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { MenuItemType = MenuItemType.Edit }
         };

         return PartialView(detraccionViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         DetraccionViewModel model = JsonSerializer.Deserialize<DetraccionViewModel>(jsonModel);

         model.ComprobanteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteDTO.OrderBy = "c.fecha desc,c.numero_opera desc";

         model.ComprobanteDTO.TipoDocumentoComercialDTO = new TipoDocumentoComercialDTO() { Codigo = TipoDocumentoComercialConstante.FACTURA };
         model.ComprobanteDTO.EstadoComprobanteCodigoList = new List<string>() { EstadoComprobanteConstante.EMITIDO };
         model.ComprobanteDTO.NotTipoDetraccion = TipoDetraccionConstante.SIN_DETRACCION;
         model.ComprobanteDTO.Estado = EstadoConstante.ACTIVO;

         try
         {
            List<ComprobanteDTO> comprobanteDTOList = comprobanteProxy.Search(model.ComprobanteDTO);

            int index = 1;
            var jsonData = new
            {
               total = 1,
               page = 1,
               records = comprobanteDTOList.Count,
               rows = from f in comprobanteDTOList.AsEnumerable()
                      select new
                      {
                         id = index++,
                         cell = new
                         {
                            f.ComprobanteId,
                            TipoDocumentoComercialNombre = f.TipoDocumentoComercialDTO.Abreviatura,
                            f.SerieNumero,
                            Fecha = f.Fecha.ToShortDateString(),
                            f.ProveedorDTO.RazonSocial,
                            f.ProveedorDTO.NumeroDocumentoIdentidad,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.MontoTotal,
                            TipoDetraccionNombre = f.TipoDetraccionDTO.Nombre,
                            TipoDetraccionTasa = f.TipoDetraccionDTO.Tasa,
                            f.MontoDetraccion,
                            f.NumeroOperacionPago,
                            FechaPago = f.FechaPago.HasValue ? f.FechaPago.Value.ToShortDateString() : string.Empty,
                            f.MontoPago,
                            f.MontoCargo,
                            f.Estado
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
      private void cargarListaPeriodoEmpresaSede(DetraccionViewModel model)
      {

         PeriodoEmpresaSedeDTO periodoEmpresaSedeDTO = ObtenerPeriodoEmpresaSedeDTO(model.ViewModelState.CID);

         List<PeriodoEmpresaSedeDTO> periodoEmpresaSedeDTOList = periodoEmpresaSedeProxy.Search(new PeriodoEmpresaSedeDTO() { 
            PeriodoId = periodoEmpresaSedeDTO.PeriodoId,
            EmpresaSedeDTO = periodoEmpresaSedeDTO.EmpresaSedeDTO,
            Estado = EstadoConstante.ACTIVO 
         });
         model.PeriodoEmpresaSedeList = WebHelper.ToSelectListItem(periodoEmpresaSedeDTOList, x => x.Id.ToString(), x => x.EmpresaSedeDTO.Nombre,
            SelectListFirstOptionType.Select);

         WebHelper.SelectedListItemSetValue(model.PeriodoEmpresaSedeList, periodoEmpresaSedeDTO.PeriodoEmpresaSedeId);
      }
      [HttpPost("EditPopup")]
      public ActionResult EditPopup([FromBody] DetraccionViewModel model)
      {

         try
         {
            model.ComprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = model.ComprobanteDTO.Id });
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(model);
      }
      [HttpPost("Grabar")]
      public ActionResult Grabar([FromBody] DetraccionViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {

            comprobanteProxy.ActualizarDetraccion(model.ComprobanteDTO);

            resultResponse.Message = SR.GetString("VT_detraccion_actualizada");

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
