using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.PD.Models
{
   public class RutaProduccionViewModel : ViewModel
   {
      public RutaProduccionViewModel()
      {
         Title = "Rutas de Producción";
      }

      public RutaProduccionDTO RutaProduccionDTO { get; set; }
      public SubProcesoProduccionDTO SubProcesoProduccionDTO { get; set; }
      public SubProcesoProduccionPersonalDTO SubProcesoProduccionPersonalDTO { get; set; }
      public List<SelectListItem> ProcesoProduccionList { get; set; }
   }
}