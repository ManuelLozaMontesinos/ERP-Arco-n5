using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class CajaViewModel : ViewModel
   {
      public CajaViewModel() {
         Title = "Caja";
      }

      public CajaDTO CajaDTO { get; set; }
      public CajaPersonalDTO CajaPersonalDTO { get; set; }
   }
}