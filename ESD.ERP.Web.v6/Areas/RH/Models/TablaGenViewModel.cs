using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class TablaGenViewModel : ViewModel
   {
      public TablaGenViewModel() {
         Title = "Pais";
      }

      public PaisDTO PaisDTO { get; set; }
      public PaisEmisorDTO PaisEmisorDTO { get; set; }
      public OcupacionDTO OcupacionDTO { get; set; }
   }
}
