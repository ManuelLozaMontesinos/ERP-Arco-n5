using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class PlanillaViewModel : ViewModel
   {
      public PlanillaViewModel() {
         Title = "Planilla";
      }

      public PlanillaDTO PlanillaDTO { get; set; }
      public PlanillaConceptoRemunerativoDTO PlanillaConceptoRemunerativoDTO { get; set; }
      public List<SelectListItem> TipoTrabajadorList { get; set; }
   }
}
