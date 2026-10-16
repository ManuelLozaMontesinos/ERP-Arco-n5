using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IOrdenProduccionProductoEntregaProxy
	{
		OrdenProduccionProductoEntregaDTO GetById(OrdenProduccionProductoEntregaDTO dto);
		List<OrdenProduccionProductoEntregaDTO> Search(OrdenProduccionProductoEntregaDTO dto);
		OrdenProduccionProductoEntregaDTO SingleSearch(OrdenProduccionProductoEntregaDTO dto);
		int Insert(OrdenProduccionProductoEntregaDTO dto);
		int Update(OrdenProduccionProductoEntregaDTO dto);
		void Delete(OrdenProduccionProductoEntregaDTO dto);
	}
}
