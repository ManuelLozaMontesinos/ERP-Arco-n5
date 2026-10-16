using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.CM.Models
{
   public class ValorizaCompraViewModel : ViewModel
   {
      public ValorizaCompraViewModel() {
         Title = "Valorización";
      }

      public OrdenCompraComprobanteDTO OrdenCompraComprobanteDTO { get; set; }
      public string CondicionPagoString { get; set; }
      public string TipoDocumentoComercialString { get; set; }
      public List<SelectListItem> CondicionPagoList { get; set; }
    }
}   