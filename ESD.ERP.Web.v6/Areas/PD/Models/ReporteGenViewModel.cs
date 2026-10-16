using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.PD.Models
{
   public class ReporteGenViewModel : ViewModel
   {

      public AlmacenDTO AlmacenDTO { get; set; }

      public List<SelectListItem> AlmacenList { get; set; }
      public List<SelectListItem> TransaccionAlmacenList { get; set; }
      public List<SelectListItem> MesList { get; set; }
      public List<SelectListItem> SeleccionList { get; set; }
      public List<SelectListItem> ProcesoList { get; set; }
      public List<SelectListItem> SubProcesoList { get; set; }
      public DateTime Fecha { get; set; }
   }
}
