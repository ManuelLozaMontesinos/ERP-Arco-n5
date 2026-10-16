using System.Collections.Generic;
using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class RegPensionarioViewModel : ViewModel
   {
      public RegPensionarioViewModel()
      {
         Title = "Regimen Pensionario";
      }

      public RegimenPensionarioDTO RegimenPensionarioDTO { get; set; }
      public RegimenPensionarioEsquemaDTO RegimenPensionarioEsquemaDTO { get; set; }
      public RegimenPensionarioComisionDTO RegimenPensionarioComisionDTO { get; set; }
      public string TipoEsquemaPensionString { get; set; }
      public string TipoAporteComisionString { get; set; }
      public List<SelectListItem> RegimenPensionarioEsquemaList { get; set; }
   }
}