using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.RH.Models
{
   public class TipoRemuneraViewModel : ViewModel
   {
      public TipoRemuneraViewModel() {
         Title = "Tipo de Remuneracion";
      }

      public TipoRemuneracionDTO TipoRemuneracionDTO { get; set; }
   }
}
