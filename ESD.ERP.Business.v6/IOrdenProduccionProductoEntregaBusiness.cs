using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOrdenProduccionProductoEntregaBusiness
	{
		OrdenProduccionProductoEntregaDTO GetById(int id);
		List<OrdenProduccionProductoEntregaDTO> Search(OrdenProduccionProductoEntregaDTO dto);
		OrdenProduccionProductoEntregaDTO SingleSearch(OrdenProduccionProductoEntregaDTO dto);
		int Insert(OrdenProduccionProductoEntregaDTO dto);
		int Update(OrdenProduccionProductoEntregaDTO dto);
		void Delete(OrdenProduccionProductoEntregaDTO dto);
	}
}
