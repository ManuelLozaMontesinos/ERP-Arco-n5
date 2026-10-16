using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class CargoPersonalViewModel : ViewModel
   {
      public CargoPersonalViewModel() {
         Title = "Cargo Personal";
      }

      public CargoPersonalDTO CargoPersonalDTO { get; set; }
   }
}
