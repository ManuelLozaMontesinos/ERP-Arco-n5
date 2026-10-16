using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class CuadroCostoViewModel : ViewModel
   {
      public CuadroCostoViewModel()
      {
         Title = "Cuadro de Costos";
      }

      public CuadroCostoDTO CuadroCostoDTO { get; set; }

      public List<SelectListItem> ListaPrecioList { get; set; }
   }
}