using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class ConceptoRemuViewModel : ViewModel
   {
      public ConceptoRemuViewModel() {
         Title = "Concepto Remunerativo";
      }

      public ConceptoRemunerativoDTO ConceptoRemunerativoDTO { get; set; }
      public ConceptoBaseImponibleDTO ConceptoBaseImponibleDTO { get; set; }
      public List<ConceptoBaseImponibleDTO> ConceptoBaseImponibleDTOList { get; set; }
      public List<SelectListItem> TipoConceptoRemunerativoList { get; set; }
   }
}
