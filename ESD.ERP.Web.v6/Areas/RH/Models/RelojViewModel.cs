using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class RelojViewModel : ViewModel
   {
      public RelojViewModel() {
         Title = "Reloj";
      }

      public RelojDTO RelojDTO { get; set; }
   }
}
