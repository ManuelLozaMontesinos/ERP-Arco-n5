using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPedidoCompraProductoProxy
	{
		PedidoCompraProductoDTO GetById(PedidoCompraProductoDTO dto);
		List<PedidoCompraProductoDTO> Search(PedidoCompraProductoDTO dto);
		PedidoCompraProductoDTO SingleSearch(PedidoCompraProductoDTO dto);
		int Insert(PedidoCompraProductoDTO dto);
		int Update(PedidoCompraProductoDTO dto);
		void Delete(PedidoCompraProductoDTO dto);
		void ActualizarProducto(PedidoCompraProductoDTO dto);
	}
}
