using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class ValorizaViewModel : ViewModel
   {
      public ValorizaViewModel() {
         Title = "Valorizacion";
      }

      public List<SelectListItem> AlmacenList { get; set; }
      public MovimientoAlmacenCierreDTO MovimientoAlmacenCierreDTO { get; set; }
      public MovimientoAlmacenSaldoConsolidadoDTO MovimientoAlmacenSaldoConsolidadoDTO { get; set; }
   }
}
