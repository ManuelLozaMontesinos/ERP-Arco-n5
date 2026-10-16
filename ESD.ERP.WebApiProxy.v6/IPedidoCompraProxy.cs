using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPedidoCompraProxy
	{
		PedidoCompraDTO GetById(PedidoCompraDTO dto);
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
