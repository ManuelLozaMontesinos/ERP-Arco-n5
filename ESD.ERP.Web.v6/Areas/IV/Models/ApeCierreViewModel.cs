using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class ApeCierreViewModel : ViewModel
   {
      public ApeCierreViewModel() {
         Title = "Apertura/Cierre";
      }

      public List<SelectListItem> AlmacenList { get; set; }
      public MovimientoAlmacenCierreDTO MovimientoAlmacenCierreDTO { get; set; }
   }
}
