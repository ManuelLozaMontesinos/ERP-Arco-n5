using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPedidoCompraProductoBusiness
	{
		PedidoCompraProductoDTO GetById(int id);
		List<PedidoCompraProductoDTO> Search(PedidoCompraProductoDTO dto);
		PedidoCompraProductoDTO SingleSearch(PedidoCompraProductoDTO dto);
		int Insert(PedidoCompraProductoDTO dto);
		int Update(PedidoCompraProductoDTO dto);
		void Delete(PedidoCompraProductoDTO dto);
		void ActualizarProducto(PedidoCompraProductoDTO dto);
	}
}
