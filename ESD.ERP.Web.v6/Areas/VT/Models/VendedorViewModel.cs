using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace ESD.ERP.Web.Areas.VT.Models
{
   public class VendedorViewModel : ViewModel
   {
      public VendedorViewModel() {
         Title = "Vendedores";
      }

      public VendedorDTO VendedorDTO { get; set; }
      public VendedorSerieDTO VendedorSerieDTO { get; set; }
      public ListaPrecioVendedorDTO ListaPrecioVendedorDTO { get; set; }
      public ListaPrecioDTO ListaPrecioDTO { get; set; }
      public List<SelectListItem> MonedaList { get; set; }

      public List<MenuItem> MenuListaPrecioVendedorList { get; set; }
      public List<MenuItem> MenuVendedorSerieList { get; set; }
    }
}
