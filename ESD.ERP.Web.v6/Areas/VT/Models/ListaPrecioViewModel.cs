using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class ListaPrecioViewModel : ViewModel
   {
      public ListaPrecioViewModel()
      {
         Title = "Lista de Precios";
      }

      public ListaPrecioDTO ListaPrecioDTO { get; set; }
      public ListaPrecioProductoDTO ListaPrecioProductoDTO { get; set; }
      public ListaPrecioProductoPresentacionDTO ListaPrecioProductoPresentacionDTO { get; set; }
      public ListaPrecioProveedorDTO ListaPrecioProveedorDTO { get; set; }
      public ListaPrecioProveedorProductoDTO ListaPrecioProveedorProductoDTO { get; set; }
      public List<SelectListItem> EmpresaSedeList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public List<SelectListItem> ListaPrecioList { get; set; }
      public List<SelectListItem> ImpuestoTasaList { get; set; }
      public List<SelectListItem> ProveedorList { get; set; }
      public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }
      public List<DataImportItemDTO> DataImportItemDTOList { get; set; }
      public string UnidadMedidaString { get; set; }
      public string GridColNames { get; set; }
      public string GridColModel { get; set; }
   }
}