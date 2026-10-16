using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Model
{
   public class PerfilAccesoViewModel
   {
      public int PeriodoId { get; set; }
      public int EmpresaId { get; set; }
      public int PeriodoEmpresaSedeId { get; set; }
      public List<SelectListItem> PeriodoList { get; set; }
      public List<SelectListItem> EmpresaList { get; set; }
      public List<SelectListItem> PeriodoEmpresaSedeList { get; set; }
   }
}
