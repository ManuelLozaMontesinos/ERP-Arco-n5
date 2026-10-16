using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoOrdenProduccionSubProcesoProxy
	{
		EstadoOrdenProduccionSubProcesoDTO GetById(EstadoOrdenProduccionSubProcesoDTO dto);
		List<EstadoOrdenProduccionSubProcesoDTO> Search(EstadoOrdenProduccionSubProcesoDTO dto);
		EstadoOrdenProduccionSubProcesoDTO SingleSearch(EstadoOrdenProduccionSubProcesoDTO dto);
		int Insert(EstadoOrdenProduccionSubProcesoDTO dto);
		int Update(EstadoOrdenProduccionSubProcesoDTO dto);
		void Delete(EstadoOrdenProduccionSubProcesoDTO dto);
		EstadoOrdenProduccionSubProcesoDTO ObtenerPorCodigo(EstadoOrdenProduccionSubProcesoDTO dto);
	}
}