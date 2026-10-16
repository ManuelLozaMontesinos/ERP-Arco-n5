using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPedidoProductoProxy
	{
		PedidoProductoDTO GetById(PedidoProductoDTO dto);
		List<PedidoProductoDTO> Search(PedidoProductoDTO dto);
		PedidoProductoDTO SingleSearch(PedidoProductoDTO dto);
		int Insert(PedidoProductoDTO dto);
		int Update(PedidoProductoDTO dto);
		void Delete(PedidoProductoDTO dto);
	}
}
