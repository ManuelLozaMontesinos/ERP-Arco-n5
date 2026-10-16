using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoOrdenProduccionProxy
	{
		EstadoOrdenProduccionDTO GetById(EstadoOrdenProduccionDTO dto);
		List<EstadoOrdenProduccionDTO> Search(EstadoOrdenProduccionDTO dto);
		EstadoOrdenProduccionDTO SingleSearch(EstadoOrdenProduccionDTO dto);
		int Insert(EstadoOrdenProduccionDTO dto);
		int Update(EstadoOrdenProduccionDTO dto);
		void Delete(EstadoOrdenProduccionDTO dto);
		EstadoOrdenProduccionDTO ObtenerPorCodigo(EstadoOrdenProduccionDTO dto);
	}
}