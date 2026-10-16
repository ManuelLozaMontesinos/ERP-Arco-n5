using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.AS.Models
{
   public class ParametroSistemaViewModel : ViewModel
   {
      public ParametroSistemaViewModel() {
         Title = "Parametros del Sistema";
      }

      public ParametroSistemaDTO ParametroSistemaDTO { get; set; }
      public string ValorCadena { get; set; }
      public string ValorCadenaOculta { get; set; }
      public int ValorCadenaEntero { get; set; }
      public decimal ValorCadenaDecimal { get; set; }
      public string ValorHoraDesde { get; set; }
      public string ValorHoraHasta { get; set; }
   }
}
