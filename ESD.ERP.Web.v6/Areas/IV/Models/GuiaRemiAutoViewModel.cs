using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class GuiaRemiAutoViewModel : ViewModel
   {
      public GuiaRemiAutoViewModel() {
         Title = "Guia Remision - Autorizacion";
      }
      public GuiaRemisionDTO GuiaRemisionDTO { get; set; }
      public GuiaRemisionProductoDTO GuiaRemisionProductoDTO { get; set; }
      public List<SelectListItem> AlmacenList { get; set; }
      public List<SelectListItem> SerieList { get; set; }
      public List<SelectListItem> TransportistaList { get; set; }
      public List<SelectListItem> TransportistaConductorList { get; set; }
      public List<SelectListItem> TransportistaUnidadList { get; set; }
      public List<SelectListItem> MotivoTrasladoList { get; set; }

   }
}
