using ESD.Core.Web;
using ESD.ERP.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ESD.ERP.Web.Areas.CM.Models
{
   public class PedidoCompraViewModel : ViewModel
   {
      public PedidoCompraViewModel() {
         Title = "Pedido de Compra";
      }

      public PedidoCompraDTO PedidoCompraDTO { get; set; }
      public PedidoCompraProductoDTO PedidoCompraProductoDTO { get; set; }
      public List<PedidoCompraProductoDTO> PedidoCompraProductoDTOList { get; set; }
      public List<SelectListItem> TipoProductoList { get; set; }

      public List<MenuItem> MenuItemList2 { get; set; }
   }
}