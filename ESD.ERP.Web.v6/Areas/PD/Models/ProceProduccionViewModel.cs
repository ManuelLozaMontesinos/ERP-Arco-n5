using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.PD.Models
{
   public class ProceProduccionViewModel : ViewModel
   {
      public ProceProduccionViewModel()
      {
         Title = "Procesos";
      }

      public ProcesoProduccionDTO ProcesoProduccionDTO { get; set; }
      public SubProcesoProduccionDTO SubProcesoProduccionDTO { get; set; }
      public SubProcesoProduccionPersonalDTO SubProcesoProduccionPersonalDTO { get; set; }
      public List<SelectListItem> SubProcesoProduccionList { get; set; }
   }
}