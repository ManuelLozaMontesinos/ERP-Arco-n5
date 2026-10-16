using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class ConsumoIntViewModel : ViewModel
   {
      public ConsumoIntViewModel() {
         Title = "Consumo Interno";
      }
      public MovimientoAlmacenDTO MovimientoAlmacenDTO { get; set; }
      public MovimientoAlmacenProductoDTO MovimientoAlmacenProductoDTO { get; set; }

      public List<SelectListItem> AlmacenList { get; set; }
      public List<SelectListItem> CentroCostoList { get; set; }

      public string FileId { get; set; }
   }
}
