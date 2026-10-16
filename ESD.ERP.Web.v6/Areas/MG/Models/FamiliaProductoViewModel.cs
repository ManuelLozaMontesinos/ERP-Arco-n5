using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.MG.Models
{
   public class FamiliaProductoViewModel : ViewModel
   {
      public FamiliaProductoViewModel()
      {
         Title = "Grupo/Clase/Familia";
      }

      public GrupoProductoDTO GrupoProductoDTO { get; set; }
      public ClaseProductoDTO ClaseProductoDTO { get; set; }
      public FamiliaProductoDTO FamiliaProductoDTO { get; set; }
      public List<SelectListItem> TipoProductoList { get; set; }
      public string TipoDetraccionString { get; set; }
      public List<SelectListItem>ClaseProductoList { get; set; }

   }
}
