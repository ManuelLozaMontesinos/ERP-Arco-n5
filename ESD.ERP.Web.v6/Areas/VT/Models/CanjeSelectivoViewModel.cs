using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class CanjeSelectivoViewModel : ViewModel
   {
      public CanjeSelectivoViewModel() {
         Title = "Canje Selectivo";
      }

      public ComprobanteDTO ComprobanteDTO { get; set; }
      public ComprobanteProductoDTO ComprobanteProductoDTO { get; set; }
      public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }
      public ComprobanteAutorizacionDTO ComprobanteAutorizacionDTO { get; set; }
      public List<ComprobanteAutorizacionDTO> ComprobanteAutorizacionDTOList { get; set; }
      public List<ComprobanteProductoDTO> ComprobanteProductoDTOList { get; set; }

      public List<SelectListItem> TipoDocumentoComercialList { get; set; }
      public List<SelectListItem> SerieList { get; set; }
      public List<SelectListItem> EstadoComprobanteList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public List<SelectListItem> ListaPrecioList { get; set; }
      public List<SelectListItem> TipoCondicionPagoList { get; set; }
      public List<SelectListItem> MedioPagoList { get; set; }

      public int ListaPrecioProveedorId { get; set; }
   }
}
