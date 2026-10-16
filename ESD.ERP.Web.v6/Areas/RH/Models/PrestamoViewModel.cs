using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class PrestamoViewModel : ViewModel
   {
      public PrestamoViewModel() {
         Title = "Prestamos/Adelantos";
      }

      public PrestamoPersonalDTO PrestamoPersonalDTO { get; set; }
      public PrestamoCuotaDTO PrestamoCuotaDTO { get; set; }
      public List<PrestamoCuotaDTO> PrestamoCuotaDTOList { get; set; }
      public List<SelectListItem> TipoPrestamoList { get; set; }
      public List<SelectListItem> MonedaList { get; set; }
      public int TipoCancelacionCuotaId { get; set; }
      public int EstadoCuotaId { get; set; }
      public string TipoCancelacionCuotaNombre { get; set; }
      public string EstadoCuotaNombre { get; set; }
   }
}
