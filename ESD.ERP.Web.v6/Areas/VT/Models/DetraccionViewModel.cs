using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class DetraccionViewModel : ViewModel
   {
      public DetraccionViewModel() {
         Title = "Detracción";
      }

      public ComprobanteDTO ComprobanteDTO { get; set; }

      public List<SelectListItem> PeriodoEmpresaSedeList { get; set; }

   }
}
