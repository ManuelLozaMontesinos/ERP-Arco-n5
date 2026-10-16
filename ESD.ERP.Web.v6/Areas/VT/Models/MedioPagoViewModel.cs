using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.VT.Models
{
   public class MedioPagoViewModel : ViewModel
   {
      public MedioPagoViewModel() {
         Title = "Medio de Pago";
      }

      public MedioPagoDTO MedioPagoDTO { get; set; }
   }
}
