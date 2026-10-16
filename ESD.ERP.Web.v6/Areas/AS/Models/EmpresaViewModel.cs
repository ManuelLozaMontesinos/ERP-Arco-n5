using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.AS.Models
{
   public class EmpresaViewModel : ViewModel
   {
      public EmpresaViewModel() {
         Title = "Empresa";
      }

      public EmpresaDTO EmpresaDTO { get; set; }
      public EmpresaSedeDTO EmpresaSedeDTO { get; set; }
      public PeriodoEmpresaSedeDTO PeriodoEmpresaSedeDTO { get; set; }
      public string DepartamentoId { get; set; }
      public string ProvinciaId { get; set; }
      public List<SelectListItem> SectorList { get; set; }
      public List<SelectListItem> TipoViaList { get; set; }
      public List<SelectListItem> TipoZonaList { get; set; }
      public List<SelectListItem> DepartamentoList { get; set; }
      public List<SelectListItem> ProvinciaList { get; set; }
      public List<SelectListItem> DistritoList { get; set; }
      public List<SelectListItem> BancoList { get; set; }
      public List<SelectListItem> PeriodoList { get; set; }
      public List<SelectListItem> EmpresaList { get; set; }
      public string EmpresaSedeString { get; set; }
   }
}
