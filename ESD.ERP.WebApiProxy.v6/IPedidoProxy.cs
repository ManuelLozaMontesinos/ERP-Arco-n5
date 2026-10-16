using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPedidoProxy
	{
		PedidoDTO GetById(PedidoDTO dto);
		List<PedidoDTO> Search(PedidoDTO dto);
		PedidoDTO SingleSearch(PedidoDTO dto);
		int Insert(PedidoDTO dto);
		int Update(PedidoDTO dto);
		void Delete(PedidoDTO dto);
	}
}
