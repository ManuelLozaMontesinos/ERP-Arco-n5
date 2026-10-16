using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.MG.Models
{
   public class ProveedorViewModel : ViewModel
   {
      public ProveedorViewModel() {
         Title = "Cliente/Proveedor";
      }

      public ProveedorDTO ProveedorDTO { get; set; }
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

   }
}
