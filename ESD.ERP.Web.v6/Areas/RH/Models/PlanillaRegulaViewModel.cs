using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class PlanillaRegulaViewModel : ViewModel
   {
      public PlanillaRegulaViewModel() {
         Title = "Regularizaciones";
      }

      public PlanillaProcesoDTO PlanillaProcesoDTO { get; set; }
      public PlanillaProcesoRegularizacionDTO PlanillaProcesoRegularizacionDTO { get; set; }
      public List<PlanillaProcesoRegularizacionDTO> PlanillaProcesoRegularizacionDTOList { get; set; }
      public List<SelectListItem> PlanillaList { get; set; }
      public List<SelectListItem> MesList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
   }
}
