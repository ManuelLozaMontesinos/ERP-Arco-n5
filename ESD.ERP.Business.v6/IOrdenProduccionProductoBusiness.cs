using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOrdenProduccionProductoBusiness
	{
		OrdenProduccionProductoDTO GetById(int id);
		List<OrdenProduccionProductoDTO> Search(OrdenProduccionProductoDTO dto);
		OrdenProduccionProductoDTO SingleSearch(OrdenProduccionProductoDTO dto);
		int Insert(OrdenProduccionProductoDTO dto);
		int Update(OrdenProduccionProductoDTO dto);
		void UpdateOrden(OrdenProduccionProductoDTO dto);
		void actualizarEnvioProd(OrdenProduccionProductoDTO dto);
        void Delete(OrdenProduccionProductoDTO dto);
		void GenerarSubProcesos(OrdenProduccionProductoDTO dto);
		void actualizarInsumos(OrdenProduccionProductoDTO dto);
    }
}