using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.MK.Models
{
   public class LeadSourceViewModel : ViewModel
   {
      public LeadSourceViewModel()
      {
         Title = "Lead Source";
      }

      public LeadSourceDTO LeadSourceDTO { get; set; }
   }
}