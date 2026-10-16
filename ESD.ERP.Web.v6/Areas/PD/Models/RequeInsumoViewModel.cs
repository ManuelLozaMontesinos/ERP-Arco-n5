using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.PD.Models
{
   public class RequeInsumoViewModel : ViewModel
   {
      public RequeInsumoViewModel() {
         Title = "Requerimiento de Insumos";
      }

      public OrdenProduccionDTO OrdenProduccionDTO { get; set; }
      public OrdenProduccionProductoDTO OrdenProduccionProductoDTO { get; set; }
      public List<SelectListItem> AlmacenList { get; set; }
      public List<SelectListItem> EstadoDespachoList { get; set; }
      public List<MenuItem> InsumoMenuBar { get; set; }

   }
}