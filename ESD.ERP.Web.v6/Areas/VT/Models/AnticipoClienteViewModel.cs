using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class AnticipoClienteViewModel : ViewModel
   {
      public AnticipoClienteViewModel() {
         Title = "Anticipo de Clientes";
      }

      public ProveedorDTO ProveedorDTO { get; set; }
      public AnticipoClienteDTO AnticipoClienteDTO { get; set; }
      public List<SelectListItem> TipoDocumentoIdentidadList { get; set; }
      public int VendedorId { get; set; }

      public List<SelectListItem> MonedaList { get; set; }
      public List<SelectListItem> MedioPagoList { get; set; }
      public List<SelectListItem> BancoList { get; set; }
      public List<SelectListItem> TipoDocumentoComercialList { get; set; }
      public List<SelectListItem> SerieList { get; set; }
      public string FileId { get; set; }
      public string ServidorImpresion { get; set; }
      public string DataUrl { get; set; }
   }
}
