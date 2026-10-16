using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class ConsultaSaldoViewModel : ViewModel
   {
      public ConsultaSaldoViewModel() {
         Title = "Consulta de Saldos";
      }
      public MovimientoAlmacenSaldoDTO MovimientoAlmacenSaldoDTO { get; set; }

      public List<SelectListItem> MesContableList { get; set; }
      public string MesContableId { get; set; }
      public string GridColNames { get; set; }
      public string GridColModel { get; set; }
   }
}
