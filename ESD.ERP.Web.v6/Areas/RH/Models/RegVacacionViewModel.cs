using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class RegVacacionViewModel : ViewModel
   {
      public RegVacacionViewModel() {
         Title = "Vacaciones";
      }

      public PersonalVacacionDTO PersonalVacacionDTO { get; set; }


      public PersonalDTO PersonalDTO { get; set; }
      public PersonalRemuneracionDTO PersonalRemuneracionDTO { get; set; }
      public PersonalPeriodoLaboralDTO PersonalPeriodoLaboralDTO { get; set; }
      public PersonalDependienteDTO PersonalDependienteDTO { get; set; }
      public List<SelectListItem> TipoDocumentoIdentidadList { get; set; }
      public string TipoDocumentoIdentidad { get; set; }
      public string FormatoValidacion { get; set; }
      public List<SelectListItem> SexoList { get; set; }
      public string DepartamentoId { get; set; }
      public string ProvinciaId { get; set; }
      public List<SelectListItem> TipoViaList { get; set; }
      public List<SelectListItem> TipoZonaList { get; set; }
      public List<SelectListItem> DepartamentoList { get; set; }
      public List<SelectListItem> ProvinciaList { get; set; }
      public List<SelectListItem> DistritoList { get; set; }
      public List<SelectListItem> TipoTrabajadorList { get; set; }
      public List<SelectListItem> CategoriaOcupacionalList { get; set; }
      public List<SelectListItem> RegimenLaboralList { get; set; }
      public List<SelectListItem> SituacionEducativaList { get; set; }
      public List<SelectListItem> RegimenPensionarioList { get; set; }
      public List<SelectListItem> RegimenPensionarioEsquemaList { get; set; }
      public List<SelectListItem> SctrSaludList { get; set; }
      public List<SelectListItem> TipoModalidadFormativaList { get; set; }
      public List<SelectListItem> TipoContratoList { get; set; }
      public List<SelectListItem> SituacionEspecialTrabajadorList { get; set; }
      public List<SelectListItem> PeriodicidadRemunerativaList { get; set; }
      public List<SelectListItem> EstadoCivilList { get; set; }
      public List<SelectListItem> TipoPagoRemuneracionList { get; set; }
      public List<SelectListItem> BancoList { get; set; }
      public List<SelectListItem> BancoCTSList { get; set; }
      public List<SelectListItem> CentroCostoList { get; set; }
      public List<SelectListItem> ConvenioDobleTributacionList { get; set; }
      public List<SelectListItem> VinculoFamiliarList { get; set; }
      public List<SelectListItem> TipoDocumentoVinculoFamiliarList { get; set; }
      public List<SelectListItem> MotivoBajaDependienteList { get; set; }

      public string RestriccionPaisEmisorXTipoDocumento { get; set; }
      public string RestriccionSituacionEducativaXTipoTrabajador { get; set; }
      public string RestriccionCategoriaOcupacionalXTipoTrabajador { get; set; }
      public string MonedaString { get; set; }
      public string TipoRemuneracionString { get; set; }
   }
}
