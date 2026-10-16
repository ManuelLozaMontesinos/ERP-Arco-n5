using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoPedidoCompraBusiness
	{
		EstadoPedidoCompraDTO GetById(int id);
		List<EstadoPedidoCompraDTO> Search(EstadoPedidoCompraDTO dto);
		EstadoPedidoCompraDTO SingleSearch(EstadoPedidoCompraDTO dto);
		int Insert(EstadoPedidoCompraDTO dto);
		int Update(EstadoPedidoCompraDTO dto);
		void Delete(EstadoPedidoCompraDTO dto);
		EstadoPedidoCompraDTO ObtenerPorCodigo(string codigo);
	}
}
