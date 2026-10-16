using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class ConsultaLlegadaViewModel : ViewModel
   {
      public ConsultaLlegadaViewModel() {
         Title = "Consulta de Llegadas";
      }
      public OrdenCompraEntregaDTO OrdenCompraEntregaDTO { get; set; }
      public string SinProveedor { get; set; }
   }
}
