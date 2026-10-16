using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class CambioBoletaViewModel : ViewModel
   {
      public CambioBoletaViewModel() {
         Title = "Cambio de Boleta";
      }

      public ComprobanteDTO ComprobanteDTO { get; set; }
      public List<SelectListItem> TipoComprobanteList { get; set; }
   }
}
