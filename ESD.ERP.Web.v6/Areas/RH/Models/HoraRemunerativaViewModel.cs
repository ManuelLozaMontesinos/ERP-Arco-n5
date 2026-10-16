using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class HoraRemunerativaViewModel : ViewModel
   {
      public HoraRemunerativaViewModel() {
         Title = "Hora Remunerativa";
      }

      public HoraRemunerativaDTO HoraRemunerativaDTO { get; set; }
   }
}
