using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPedidoProductoBusiness
	{
		PedidoProductoDTO GetById(int id);
		List<PedidoProductoDTO> Search(PedidoProductoDTO dto);
		PedidoProductoDTO SingleSearch(PedidoProductoDTO dto);
		int Insert(PedidoProductoDTO dto);
		int Update(PedidoProductoDTO dto);
		void Delete(PedidoProductoDTO dto);
	}
}
