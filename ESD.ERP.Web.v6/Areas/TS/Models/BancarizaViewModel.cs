using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.TS.Models
{
   public class BancarizaViewModel : ViewModel
   {
      public BancarizaViewModel() {
         Title = "Bancarización";
      }

      public ComprobanteDTO ComprobanteDTO { get; set; }
      public ComprobanteCobranzaDTO ComprobanteCobranzaDTO { get; set; }

      public List<SelectListItem> PeriodoEmpresaSedeList { get; set; }
      public List<SelectListItem> ListaPrecioList { get; set; }
      public List<SelectListItem> TipoCondicionPagoList { get; set; }
      public List<SelectListItem> MedioPagoList { get; set; }
      public string EmpresaCuentaBancariaString { get; set; }
   }
}
