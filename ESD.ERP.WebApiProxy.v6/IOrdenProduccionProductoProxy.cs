using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IOrdenProduccionProductoProxy
	{
		OrdenProduccionProductoDTO GetById(OrdenProduccionProductoDTO dto);
		List<OrdenProduccionProductoDTO> Search(OrdenProduccionProductoDTO dto);
		OrdenProduccionProductoDTO SingleSearch(OrdenProduccionProductoDTO dto);
		int Insert(OrdenProduccionProductoDTO dto);
		int Update(OrdenProduccionProductoDTO dto);
		void Delete(OrdenProduccionProductoDTO dto);
		void actualizarInsumos(OrdenProduccionProductoDTO dto);

    }
}
