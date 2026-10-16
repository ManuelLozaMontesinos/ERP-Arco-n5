using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class PlanillaHoraViewModel : ViewModel
   {
      public PlanillaHoraViewModel() {
         Title = "Horas Laboradas";
      }

      public PlanillaProcesoDTO PlanillaProcesoDTO { get; set; }
      public PlanillaProcesoHoraDTO PlanillaProcesoHoraDTO { get; set; }
      public List<PlanillaProcesoHoraDTO> PlanillaProcesoHoraDTOList { get; set; }
      public PersonalMarcacionDTO PersonalMarcacionDTO { get; set; }
      public List<SelectListItem> PlanillaList { get; set; }
      public List<SelectListItem> MesList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public string GridColNames { get; set; }
      public string GridColModel { get; set; }
      public string HoraList { get; set; }
   }
}
