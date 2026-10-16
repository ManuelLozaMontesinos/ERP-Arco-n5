using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class ReporteBoletaViewModel : ViewModel
   {
      public ReporteBoletaViewModel() {
         Title = "Boleta";
      }

      public List<SelectListItem> PlanillaProcesoList { get; set; }
      public PeriodoDTO PeriodoDTO { get; set; }
      public ModuloDTO ModuloDTO { get; set; }
      public ProcesoIntegracionDTO ProcesoIntegracionDTO { get; set; }
      public List<ProcesoIntegracionDTO> ProcesoIntegracionDTOList { get; set; }
   }
}
