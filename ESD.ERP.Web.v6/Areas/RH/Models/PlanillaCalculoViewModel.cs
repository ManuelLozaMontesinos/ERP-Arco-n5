using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class PlanillaCalculoViewModel : ViewModel
   {
      public PlanillaCalculoViewModel() {
         Title = "Calculo";
      }

      public PlanillaProcesoDTO PlanillaProcesoDTO { get; set; }
      public List<SelectListItem> PlanillaList { get; set; }
      public List<SelectListItem> MesList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
   }
}
