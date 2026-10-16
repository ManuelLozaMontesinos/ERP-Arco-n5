using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IOrdenCompraComprobanteProxy
	{
		OrdenCompraComprobanteDTO GetById(OrdenCompraComprobanteDTO dto);
		List<OrdenCompraComprobanteDTO> Search(OrdenCompraComprobanteDTO dto);
		OrdenCompraComprobanteDTO SingleSearch(OrdenCompraComprobanteDTO dto);
		int Insert(OrdenCompraComprobanteDTO dto);
		int Update(OrdenCompraComprobanteDTO dto);
		void Delete(OrdenCompraComprobanteDTO dto);
	}
}
