using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.CM.Models
{
   public class OrdenComAutoViewModel : ViewModel
   {
      public OrdenComAutoViewModel() {
         Title = "Orden de Compra - Autorizacion";
      }

      public OrdenCompraDTO OrdenCompraDTO { get; set; }
      public OrdenCompraProductoDTO OrdenCompraProductoDTO { get; set; }
      public OrdenCompraEntregaDTO OrdenCompraEntregaDTO { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public List<SelectListItem> CondicionPagoList { get; set; }
      public List<SelectListItem> TipoProductoList { get; set; }
      public List<SelectListItem> ImpuestoTasaList { get; set; }
      public List<SelectListItem> OrdenCompraProductoList { get; set; }

   }
}
