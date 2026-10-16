using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class AlmacenViewModel : ViewModel
   {
      public AlmacenViewModel() {
         Title = "Almacenes";
      }

      public AlmacenDTO AlmacenDTO { get; set; }
      public AlmacenTransaccionDTO AlmacenTransaccionDTO { get; set; }
      public AlmacenTransaccionConfiguracionDTO AlmacenTransaccionConfiguracionDTO { get; set; }
      public AlmacenSerieDTO AlmacenSerieDTO { get; set; }

      public List<SelectListItem> EmpresaSedeList { get; set; }
      public List<SelectListItem> TipoViaList { get; set; }
      public List<SelectListItem> TipoZonaList { get; set; }
      public List<SelectListItem> DepartamentoList { get; set; }
      public List<SelectListItem> ProvinciaList { get; set; }
      public List<SelectListItem> DistritoList { get; set; }
      public List<SelectListItem> TransaccionAlmacenList { get; set; }
      public string DepartamentoId { get; set; }
      public string ProvinciaId { get; set; }
      public string TransaccionAlmacenString { get; set; }
   }
}
