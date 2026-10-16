using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.MG.Models
{
   public class UITViewModel : ViewModel
   {
      public UITViewModel()
      {
         Title = "UIT";
      }

      public UITDTO UITDTO { get; set; }
   }
}