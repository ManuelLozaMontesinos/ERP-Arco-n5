using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class ComproAnticipoViewModel : ViewModel
   {
      public ComproAnticipoViewModel() {
         Title = "Facturacion de Anticipos";
      }

      public ComprobanteDTO ComprobanteDTO { get; set; }
      public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }

      public List<SelectListItem> TipoDocumentoComercialList { get; set; }
      public List<SelectListItem> SerieList { get; set; }
      public List<SelectListItem> EstadoComprobanteList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public List<SelectListItem> MedioPagoList { get; set; }

      public string Vendedor { get; set; }
      public string Descripcion { get; set; }
   }
}
