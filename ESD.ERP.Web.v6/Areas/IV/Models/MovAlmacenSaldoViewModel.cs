using ESD.Core.Web;
using ESD.ERP.DTO;

namespace ESD.ERP.Web.Areas.IV.Models
{
   public class MovAlmacenViewSaldoModel : ViewModel
   {
      public MovAlmacenViewSaldoModel() {
         Title = "Saldo de Almacen";
      }
      public MovimientoAlmacenSaldoDTO MovimientoAlmacenSaldoDTO { get; set; }
   }
}
