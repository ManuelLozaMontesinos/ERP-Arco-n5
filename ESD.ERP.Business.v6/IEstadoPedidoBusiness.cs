using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoPedidoBusiness
	{
		EstadoPedidoDTO GetById(int id);
		List<EstadoPedidoDTO> Search(EstadoPedidoDTO dto);
		EstadoPedidoDTO SingleSearch(EstadoPedidoDTO dto);
		int Insert(EstadoPedidoDTO dto);
		int Update(EstadoPedidoDTO dto);
		void Delete(EstadoPedidoDTO dto);
	}
}
