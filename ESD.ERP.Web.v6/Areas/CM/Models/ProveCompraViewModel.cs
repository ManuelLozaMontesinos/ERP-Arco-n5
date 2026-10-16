using ESD.Core.Web;
using ESD.ERP.Common;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.CM.Models
{
   public class ProveCompraViewModel : ViewModel
   {
      public ProveCompraViewModel() {
         Title = "Proveedores";
      }

      public ProveedorDTO ProveedorDTO { get; set; }
      public ProveedorProductoDTO ProveedorProductoDTO { get; set; }
      public ProveedorListaPrecioDTO ProveedorListaPrecioDTO { get; set; }
      public ProveedorListaPrecioProductoDTO ProveedorListaPrecioProductoDTO { get; set; }
      public string DepartamentoId { get; set; }
      public string ProvinciaId { get; set; }
      public string TipoNaturalezaJuridica { get; set; }
      public List<SelectListItem> TipoNaturalezaJuridicaList { get; set; }
      public List<SelectListItem> TipoDocumentoIdentidadList { get; set; }
      public List<SelectListItem> TipoViaList { get; set; }
      public List<SelectListItem> TipoZonaList { get; set; }
      public List<SelectListItem> DepartamentoList { get; set; }
      public List<SelectListItem> ProvinciaList { get; set; }
      public List<SelectListItem> DistritoList { get; set; }
      public List<SelectListItem> CondicionPagoList { get; set; }
      public string ActualizarMaestro { get; set; }
      public string MonedaString { get; set; }
      public string ImpuestoTasaString { get; set; }
      public List<SelectListItem> ProveedorListaPrecioList { get; set; }
      public CalculadorVentaItemDTO CalculadorVentaItemDTO { get; set; }
      public List<DataImportItemDTO> DataImportItemDTOList { get; set; }

   }
}
