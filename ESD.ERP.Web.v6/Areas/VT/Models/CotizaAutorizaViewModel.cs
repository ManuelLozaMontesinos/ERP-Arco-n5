using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class CotizaAutorizaViewModel : ViewModel
   {
      public CotizaAutorizaViewModel() {
         Title = "Cotizacion - Autorizaciones";
      }
      public CotizacionAutorizacionDTO CotizacionAutorizacionDTO { get; set; }
      public List<SelectListItem> EstadoAprobacionList { get; set; }
      public CotizacionDTO CotizacionDTO { get; set; }
      public CotizacionProductoDTO CotizacionProductoDTO { get; set; }
      public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }
      public decimal PorcentajeFraccionPU { get; set; }
      public List<CotizacionProductoDTO> CotizacionProductoDTOList { get; set; }

      public List<SelectListItem> MonedaList { get; set; }
      public List<SelectListItem> ListaPrecioList { get; set; }
      public List<SelectListItem> TipoCondicionPagoList { get; set; }
   }
}