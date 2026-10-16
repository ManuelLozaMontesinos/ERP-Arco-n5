using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class VolumenExistenViewModel : ViewModel
   {
      public VolumenExistenViewModel()
      {
         Title = "Volumen de Existencias";
      }

      public AlmacenVolumenProductoDTO AlmacenVolumenProductoDTO { get; set; }

      public List<SelectListItem> EmpresaSedeList { get; set; }
      public List<SelectListItem> PeriodoEmpresaSedeList { get; set; }
      public List<DataImportItemDTO> DataImportItemDTOList { get; set; }
      public string GridColNames { get; set; }
      public string GridColModel { get; set; }

   }
}