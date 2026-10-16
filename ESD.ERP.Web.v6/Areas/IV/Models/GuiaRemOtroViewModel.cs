using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class GuiaRemOtroViewModel : ViewModel
   {
      public GuiaRemOtroViewModel() {
         Title = "Guia de Remisión - Otro";
      }
      public GuiaRemisionDTO GuiaRemisionDTO { get; set; }
      public GuiaRemisionProductoDTO GuiaRemisionProductoDTO { get; set; }
      public List<SelectListItem> SerieList { get; set; }
      public List<SelectListItem> TransportistaList { get; set; }
      public List<SelectListItem> TransportistaConductorList { get; set; }
      public List<SelectListItem> TransportistaUnidadList { get; set; }
      public List<SelectListItem> MotivoTrasladoList { get; set; }
      public List<SelectListItem> EstadoGuiaRemisionList { get; set; }
      public List<SelectListItem> DepartamentoList { get; set; }
      public List<SelectListItem> ProvinciaList { get; set; }
      public List<SelectListItem> DistritoList { get; set; }
      public List<SelectListItem> DepartamentoList2 { get; set; }
      public List<SelectListItem> ProvinciaList2 { get; set; }
      public List<SelectListItem> DistritoList2 { get; set; }
      public List<SelectListItem> TipoDocumentoComercialList { get; set; }
      public string DepartamentoId { get; set; }
      public string ProvinciaId { get; set; }
      public string Departamento2Id { get; set; }
      public string Provincia2Id { get; set; }

      public int AlmacenId { get; set; }
      public string FileId { get; set; }
   }
}
