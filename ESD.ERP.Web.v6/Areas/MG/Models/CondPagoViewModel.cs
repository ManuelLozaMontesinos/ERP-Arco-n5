using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.MG.Models
{
   public class CondPagoViewModel : ViewModel
   {
      public CondPagoViewModel() {
         Title = "Condición de Pago";
      }

      public CondicionPagoDTO CondicionPagoDTO { get; set; }
      public List<SelectListItem> TipoCondicionPagoList { get; set; }
   }
}
