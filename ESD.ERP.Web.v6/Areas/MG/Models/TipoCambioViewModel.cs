using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.MG.Models
{
   public class TipoCambioViewModel : ViewModel
   {
      public TipoCambioViewModel()
      {
         Title = "Tipo de Cambio";
      }

      public TipoCambioDTO TipoCambioDTO { get; set; }
      public TipoCambioDiaDTO TipoCambioDiaDTO { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public string MonedaString { get; set; }
   }
}