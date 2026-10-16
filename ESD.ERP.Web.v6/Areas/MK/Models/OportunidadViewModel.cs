using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.MK.Models
{
   public class OportunidadViewModel : ViewModel
   {
      public OportunidadViewModel()
      {
         Title = "Oportunidades";
      }

      public OportunidadVentaDTO OportunidadVentaDTO { get; set; }
   }
}