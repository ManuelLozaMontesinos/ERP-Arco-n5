using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.MG.Models
{
   public class RemMinVitalViewModel : ViewModel
   {
      public RemMinVitalViewModel()
      {
         Title = "Remuneracion Minima Vital";
      }

      public RemuneracionMinimaVitalDTO RemuneracionMinimaVitalDTO { get; set; }
   }
}