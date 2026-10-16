using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class ControlMermaViewModel : ViewModel
   {
      public ControlMermaViewModel() {
         Title = "Control de Mermas";
      }
      public MovimientoAlmacenDTO MovimientoAlmacenDTO { get; set; }
      public MovimientoAlmacenProductoDTO MovimientoAlmacenProductoDTO { get; set; }

      public List<SelectListItem> AlmacenList { get; set; }
      public List<SelectListItem> TipoMovimientoAlmacenList { get; set; }

      public string FileId { get; set; }
   }
}
