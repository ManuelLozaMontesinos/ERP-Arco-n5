using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOrdenCompraComprobanteBusiness
	{
		OrdenCompraComprobanteDTO GetById(int id);
		List<OrdenCompraComprobanteDTO> Search(OrdenCompraComprobanteDTO dto);
		OrdenCompraComprobanteDTO SingleSearch(OrdenCompraComprobanteDTO dto);
		int Insert(OrdenCompraComprobanteDTO dto);
		int Update(OrdenCompraComprobanteDTO dto);
		void Delete(OrdenCompraComprobanteDTO dto);
	}
}
