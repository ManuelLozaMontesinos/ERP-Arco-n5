using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.AS.Models
{
   public class DocuComSerieViewModel : ViewModel
   {
      public DocuComSerieViewModel()
      {
         Title = "Documentos Serie";
      }

      public DocumentoComercialSerieDTO DocumentoComercialSerieDTO { get; set; }

      public List<SelectListItem> EmpresaSedeList { get; set; }
      public List<SelectListItem> TipoDocumentoComercialList { get; set; }
      public List<SelectListItem> TipoDocumentoComercialRelacionadoList { get; set; }
      public List<SelectListItem> AlmacenList { get; set; }
   }
}