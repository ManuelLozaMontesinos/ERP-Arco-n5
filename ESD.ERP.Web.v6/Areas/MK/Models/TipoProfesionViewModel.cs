using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.MK.Models
{
   public class TipoProfesionViewModel : ViewModel
   {
      public TipoProfesionViewModel()
      {
         Title = "Tipificacion";
      }

      public TipoProfesionOficioDTO TipoProfesionOficioDTO { get; set; }
   }
}