using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.CM.Models
{
   public class PedidoComAutoViewModel : ViewModel
   {
      public PedidoComAutoViewModel() {
         Title = "Pedido de Compra - Autorizacion";
      }

      public PedidoCompraDTO PedidoCompraDTO { get; set; }
      public List<SelectListItem> TipoProductoList { get; set; }
   }
}
