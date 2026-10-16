using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IOrdenCompraProductoProxy
	{
		OrdenCompraProductoDTO GetById(OrdenCompraProductoDTO dto);
		List<OrdenCompraProductoDTO> Search(OrdenCompraProductoDTO dto);
		OrdenCompraProductoDTO SingleSearch(OrdenCompraProductoDTO dto);
		int Insert(OrdenCompraProductoDTO dto);
		int Update(OrdenCompraProductoDTO dto);
		void Delete(OrdenCompraProductoDTO dto);
	}
}
