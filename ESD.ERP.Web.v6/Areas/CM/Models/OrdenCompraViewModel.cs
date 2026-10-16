using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.CM.Models
{
   public class OrdenCompraViewModel : ViewModel
   {
      public OrdenCompraViewModel() {
         Title = "Orden de Compra";
      }

      public OrdenCompraDTO OrdenCompraDTO { get; set; }
      public OrdenCompraProductoDTO OrdenCompraProductoDTO { get; set; }
      public OrdenCompraEntregaDTO OrdenCompraEntregaDTO { get; set; }
      public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public List<SelectListItem> CondicionPagoList { get; set; }
      public List<SelectListItem> TipoProductoList { get; set; }
      public List<SelectListItem> ImpuestoTasaList { get; set; }
      public List<SelectListItem> OrdenCompraProductoList { get; set; }
      public List<SelectListItem> EstadoOrdenCompraList { get; set; }
      public bool EsFechaEntrega { get; set; }

   }
}