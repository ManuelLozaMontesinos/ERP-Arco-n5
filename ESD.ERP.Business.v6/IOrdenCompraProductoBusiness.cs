using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOrdenCompraProductoBusiness
	{
		OrdenCompraProductoDTO GetById(int id);
		List<OrdenCompraProductoDTO> Search(OrdenCompraProductoDTO dto);
		OrdenCompraProductoDTO SingleSearch(OrdenCompraProductoDTO dto);
		int Insert(OrdenCompraProductoDTO dto);
		int Update(OrdenCompraProductoDTO dto);
		void Delete(OrdenCompraProductoDTO dto);
	}
}
