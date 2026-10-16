using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPedidoBusiness
	{
		PedidoDTO GetById(int id);
		List<PedidoDTO> Search(PedidoDTO dto);
		PedidoDTO SingleSearch(PedidoDTO dto);
		int Insert(PedidoDTO dto);
		int Update(PedidoDTO dto);
		void Delete(PedidoDTO dto);
	}
}
