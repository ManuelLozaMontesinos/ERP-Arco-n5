using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class FeriadoViewModel : ViewModel
   {
      public FeriadoViewModel()
      {
         Title = "Feriados";
      }

      public FeriadoDTO FeriadoDTO { get; set; }
   }
}