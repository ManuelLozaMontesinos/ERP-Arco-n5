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
   public class CambioBoletaController : AuthorizedController
   {
      private readonly ITipoComprobanteProxy tipoComprobanteProxy;
      private readonly IComprobanteProxy comprobanteProxy;

      public CambioBoletaController(
         ITipoComprobanteProxy tipoComprobanteProxy,
         IComprobanteProxy comprobanteProxy
         )
      {
         this.tipoComprobanteProxy = tipoComprobanteProxy;
         this.comprobanteProxy = comprobanteProxy;
      }
      [Route("[action]")]
      public IActionResult Index()
      {
         CambioBoletaViewModel cambioBoletaViewModel = new();

         cargarListaTipoComprobante(cambioBoletaViewModel);

         cambioBoletaViewModel.MenuItemList = new List<MenuItem>
         {
            new MenuItem() { Key = "cambiar_tipo", Content = "Cambiar Tipo", Disabled = true}
         };

         return PartialView(cambioBoletaViewModel);
      }
      [HttpPost("ListarBandeja")]
      public ActionResult ListarBandeja(string jsonModel)
      {

         Logger.InfoStart();

         CambioBoletaViewModel model = JsonSerializer.Deserialize<CambioBoletaViewModel>(jsonModel);

         model.ComprobanteDTO.PageSize = ObtenerMaximoRegistrosBusqueda();
         model.ComprobanteDTO.OrderBy = "c.fecha desc,c.numero_opera desc";

         model.ComprobanteDTO.PeriodoEmpresaSedeId = ObtenerPeriodoEmpresaSedeId(model.ViewModelState.CID);
         model.ComprobanteDTO.TipoDocumentoComercialList = new List<string>() { TipoDocumentoComercialConstante.BOLETA_VENTA };

         if(model.ComprobanteDTO.TipoComprobanteId == 0)
            model.ComprobanteDTO.TipoComprobanteList = new List<string>() { TipoComprobanteConstante.NORMAL, TipoComprobanteConstante.TEMPORAL };

         model.ComprobanteDTO.EstadoSunatDTO = new EstadoSunatDTO() { Codigo = EstadoSunatConstante.PENDIENTE_ENVIO };
         model.ComprobanteDTO.EstadoComprobanteCodigoList = new List<string>() { EstadoComprobanteConstante.EMITIDO };

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
                            VendedorNombre = f.VendedorDTO.ProveedorDTO.RazonSocial,
                            Moneda = f.MonedaDTO.Simbolo,
                            f.MontoTotal,
                            TipoComprobante = f.TipoComprobanteDTO.Codigo,
                            TipoComprobanteNombre = f.TipoComprobanteDTO.Nombre,
                            EstadoSunat = f.EstadoSunatDTO != null ? f.EstadoSunatDTO.Codigo : string.Empty,
                            EstadoSunatNombre = f.EstadoSunatDTO != null ? f.EstadoSunatDTO.Nombre : string.Empty,
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
      private void cargarListaTipoComprobante(CambioBoletaViewModel model)
      {
         List<TipoComprobanteDTO> tipoComprobanteDTOList = tipoComprobanteProxy.Search(new TipoComprobanteDTO() { 
            CodigoList = new List<string>() { TipoComprobanteConstante.NORMAL,TipoComprobanteConstante.TEMPORAL},
            Estado = EstadoConstante.ACTIVO 
         });
         model.TipoComprobanteList = WebHelper.ToSelectListItem(tipoComprobanteDTOList, x => x.Id.ToString(), x => x.Nombre,
            SelectListFirstOptionType.Select);
      }
      [HttpPost("CambiarTipoEditPopup")]
      public ActionResult CambiarTipoEditPopup([FromBody] CambioBoletaViewModel model)
      {

         try
         {
            if(model == null)
               model = new CambioBoletaViewModel();

            cargarListaTipoComprobante(model);
         }
         catch (Exception e)
         {
            Logger.Error(e);
            throw;
         }

         return PartialView(model);
      }
      [HttpPost("CambiarTipo")]
      public ActionResult CambiarTipo([FromBody] CambioBoletaViewModel model)
      {
         Logger.InfoStart();

         ResultResponse resultResponse = new();

         try
         {
            AuditDTO(model.ComprobanteDTO);

            comprobanteProxy.ActualizarTipoComprobante(model.ComprobanteDTO);

            ComprobanteDTO comprobanteDTO = comprobanteProxy.GetById(new ComprobanteDTO() { ComprobanteId = model.ComprobanteDTO.Id });

            resultResponse.Message = SR.GetString("VT_comprobante_actualizado", comprobanteDTO.SerieNumero);
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
