using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.PD.Models
{
   public class OrdenProduViewModel : ViewModel
   {
      public OrdenProduViewModel() {
         Title = "Orden de Producción";
      }

      public OrdenProduccionDTO OrdenProduccionDTO { get; set; }
      public OrdenProduccionProductoDTO OrdenProduccionProductoDTO { get; set; }
      public OrdenProduccionSubProcesoDTO OrdenProduccionSubProcesoDTO { get; set; }
      public CotizacionDTO CotizacionDTO { get; set; }
      public List<SelectListItem> AlmacenList { get; set; }
      public List<SelectListItem> EstadoOrdenProduccionMelList { get; set; }
      public List<SelectListItem> EstadoOrdenProduccionMdfList { get; set; }
      public string Vendedor { get; set; }
      public string Maquina { get; set; }
    }
}