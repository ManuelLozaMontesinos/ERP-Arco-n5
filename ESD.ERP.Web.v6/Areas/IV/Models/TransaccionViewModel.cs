using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class TransaccionViewModel : ViewModel
   {
      public TransaccionViewModel() {
         Title = "Transacciones";
      }

      public TransaccionAlmacenDTO TransaccionAlmacenDTO { get; set; }

      public List<SelectListItem> TipoMovimientoAlmacenList { get; set; }
      public List<SelectListItem> TipoOperacionList { get; set; }
   }
}
