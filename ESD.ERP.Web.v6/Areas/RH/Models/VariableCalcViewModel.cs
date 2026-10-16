using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class VariableCalcViewModel : ViewModel
   {
      public VariableCalcViewModel() {
         Title = "Variables de Calculo";
      }

      public VariableCalculoDTO VariableCalculoDTO { get; set; }
      public List<SelectListItem> TipoVariableCalculoList { get; set; }
      public List<SelectListItem> TipoDatoList { get; set; }
      public string TipoDato { get; set; }
      public string TieneCondicion { get; set; }
   }
}
