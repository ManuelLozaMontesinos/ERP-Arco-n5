using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoOrdenProduccionBusiness
	{
		EstadoOrdenProduccionDTO GetById(int id);
		List<EstadoOrdenProduccionDTO> Search(EstadoOrdenProduccionDTO dto);
		EstadoOrdenProduccionDTO SingleSearch(EstadoOrdenProduccionDTO dto);
		int Insert(EstadoOrdenProduccionDTO dto);
		int Update(EstadoOrdenProduccionDTO dto);
		void Delete(EstadoOrdenProduccionDTO dto);
		EstadoOrdenProduccionDTO ObtenerPorCodigo(string codigo);
	}
}
