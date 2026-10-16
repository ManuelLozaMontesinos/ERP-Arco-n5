using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.MK.Models
{
   public class ClienteViewModel : ViewModel
   {
      public ClienteViewModel()
      {
         Title = "Cliente";
      }

      public ProveedorDTO ProveedorDTO { get; set; }
      public string DepartamentoId { get; set; }
      public string ProvinciaId { get; set; }
      public string TipoNaturalezaJuridica { get; set; }
      public List<SelectListItem> TipoNaturalezaJuridicaList { get; set; }
      public List<SelectListItem> TipoDocumentoIdentidadList { get; set; }
      public List<SelectListItem> TipoDocumentoIdentidad2List { get; set; }
      public List<SelectListItem> TipoProfesionOficioList { get; set; }
      public List<SelectListItem> TipoVentaClienteList { get; set; }
      public List<SelectListItem> DepartamentoList { get; set; }
      public List<SelectListItem> ProvinciaList { get; set; }
      public List<SelectListItem> DistritoList { get; set; }
      public List<SelectListItem> EmpresaSedeList { get; set; }
      public ProveedorRedSocialDTO ProveedorRedSocialDTO { get; set; }
      public TipoDocumentoIdentidadDTO TipoDocumentoIdentidadDTO { get; set; }
      public ContactosDTO ContactosDTO { get; set; }
      public PuestoDTO PuestoDTO { get; set; }
      public string RedSocialString { get; set; }
      public string TipoDocumentoString { get; set; }
      public string PuestoString {  get; set; }

   }
}
