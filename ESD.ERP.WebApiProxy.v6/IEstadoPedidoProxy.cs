using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoPedidoProxy
	{
		EstadoPedidoDTO GetById(EstadoPedidoDTO dto);
		List<EstadoPedidoDTO> Search(EstadoPedidoDTO dto);
		EstadoPedidoDTO SingleSearch(EstadoPedidoDTO dto);
		int Insert(EstadoPedidoDTO dto);
		int Update(EstadoPedidoDTO dto);
		void Delete(EstadoPedidoDTO dto);
	}
}
