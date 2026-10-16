using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoOrdenProduccionSubProcesoBusiness
	{
		EstadoOrdenProduccionSubProcesoDTO GetById(int id);
		List<EstadoOrdenProduccionSubProcesoDTO> Search(EstadoOrdenProduccionSubProcesoDTO dto);
		EstadoOrdenProduccionSubProcesoDTO SingleSearch(EstadoOrdenProduccionSubProcesoDTO dto);
		int Insert(EstadoOrdenProduccionSubProcesoDTO dto);
		int Update(EstadoOrdenProduccionSubProcesoDTO dto);
		void Delete(EstadoOrdenProduccionSubProcesoDTO dto);
		EstadoOrdenProduccionSubProcesoDTO ObtenerPorCodigo(string codigo);
	}
}
