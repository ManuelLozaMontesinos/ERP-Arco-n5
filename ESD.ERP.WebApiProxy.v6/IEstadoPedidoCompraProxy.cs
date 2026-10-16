using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoPedidoCompraProxy
	{
		EstadoPedidoCompraDTO GetById(EstadoPedidoCompraDTO dto);
		List<EstadoPedidoCompraDTO> Search(EstadoPedidoCompraDTO dto);
		EstadoPedidoCompraDTO SingleSearch(EstadoPedidoCompraDTO dto);
		int Insert(EstadoPedidoCompraDTO dto);
		int Update(EstadoPedidoCompraDTO dto);
		void Delete(EstadoPedidoCompraDTO dto);
		EstadoPedidoCompraDTO ObtenerPorCodigo(EstadoPedidoCompraDTO dto);
	}
}
