using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.MG.Models
{
   public class TransportistaViewModel : ViewModel
   {
      public TransportistaViewModel()
      {
         Title = "Transportista";
      }

      public TransportistaDTO TransportistaDTO { get; set; }
      public TransportistaUnidadDTO TransportistaUnidadDTO { get; set; }
      public TransportistaConductorDTO TransportistaConductorDTO { get; set; }
      public string TipoDocumentoIdentidadString { get; set; }
   }
}