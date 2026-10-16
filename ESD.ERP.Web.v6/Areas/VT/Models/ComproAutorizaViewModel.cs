using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class ComproAutorizaViewModel : ViewModel
   {
      public ComproAutorizaViewModel() {
         Title = "Comprobante - Autorizaciones";
      }
      public ComprobanteAutorizacionDTO ComprobanteAutorizacionDTO { get; set; }
      public List<SelectListItem> EstadoAprobacionList { get; set; }
      public ComprobanteDTO ComprobanteDTO { get; set; }
      public ComprobanteProductoDTO ComprobanteProductoDTO { get; set; }
      public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }
      public decimal PorcentajeFraccionPU { get; set; }
      public List<ComprobanteProductoDTO> ComprobanteProductoDTOList { get; set; }

      public List<SelectListItem> TipoDocumentoComercialList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public List<SelectListItem> ListaPrecioList { get; set; }
      public List<SelectListItem> TipoCondicionPagoList { get; set; }
      public List<SelectListItem> MedioPagoList { get; set; }
   }
}
