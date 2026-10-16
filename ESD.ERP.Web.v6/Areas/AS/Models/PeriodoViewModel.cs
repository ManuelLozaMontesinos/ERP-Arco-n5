using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.AS.Models
{
   public class PeriodoViewModel : ViewModel
   {
      public PeriodoViewModel() {
         Title = "Periodo";
      }

      public PeriodoDTO PeriodoDTO { get; set; }
   }
}
