using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.PD.Models
{
   public class DespachoViewModel : ViewModel
   {
      public DespachoViewModel() {
         Title = "Despacho";
      }

      public ComprobanteDTO ComprobanteDTO { get; set; }
      public ComprobanteProductoDTO ComprobanteProductoDTO { get; set; }
      public ComprobanteProductoEntregaDTO ComprobanteProductoEntregaDTO { get; set; }
      public List<SelectListItem> EstadoDespachoList { get; set; }

      public List<MenuItem> MenuItemList2 { get; set; }
   }
}
