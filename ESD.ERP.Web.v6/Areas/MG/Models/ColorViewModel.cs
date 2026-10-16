using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.MG.Models
{
   public class ColorViewModel : ViewModel
   {
      public ColorViewModel()
      {
         Title = "Colores";
      }

      public ColorDTO ColorDTO { get; set; }
   }
}