using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class ReporteGenViewModel : ViewModel
   {

      public CajaDTO CajaDTO { get; set; }

      public List<SelectListItem> SeleccionList { get; set; }
      public List<SelectListItem> Seleccion2List { get; set; }
      public DateTime Fecha { get; set; }
   }
}
