using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.MK.Models
{
   public class ProspectoViewModel : ViewModel
   {
      public ProspectoViewModel()
      {
         Title = "Prospecto";
      }

      public ProveedorDTO ProveedorDTO { get; set; }
      public string DepartamentoId { get; set; }
      public string ProvinciaId { get; set; }
      public string TipoNaturalezaJuridica { get; set; }
      public List<SelectListItem> TipoNaturalezaJuridicaList { get; set; }
      public List<SelectListItem> TipoDocumentoIdentidadList { get; set; }
      public List<SelectListItem> TipoProfesionOficioList { get; set; }
      public List<SelectListItem> TipoVentaClienteList { get; set; }
      public List<SelectListItem> DepartamentoList { get; set; }
      public List<SelectListItem> ProvinciaList { get; set; }
      public List<SelectListItem> DistritoList { get; set; }
      public List<SelectListItem> LeadSourceList { get; set; }
      public ProveedorOportunidadDTO ProveedorOportunidadDTO { get; set; }
      public string OportunidadVentaString { get; set; }

   }

}
