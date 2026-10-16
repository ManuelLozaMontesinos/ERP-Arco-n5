using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPedidoCompraBusiness
	{
		PedidoCompraDTO GetById(int id);
		List<PedidoCompraDTO> Search(PedidoCompraDTO dto);
		PedidoCompraDTO SingleSearch(PedidoCompraDTO dto);
		int Insert(PedidoCompraDTO dto);
		int Update(PedidoCompraDTO dto);
		void Delete(PedidoCompraDTO dto);
		int AgregarDesdeStockMinimo(PedidoCompraDTO dto);
		void SolicitarAutorizacion(PedidoCompraDTO dto);
		void Aprobar(PedidoCompraDTO dto);
		void Desaprobar(PedidoCompraDTO dto);
		void SolicitarModificacion(PedidoCompraDTO dto);
	}
}
