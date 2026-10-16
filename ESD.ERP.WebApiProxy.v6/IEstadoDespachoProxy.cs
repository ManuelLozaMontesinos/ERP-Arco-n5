using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoDespachoProxy
	{
		EstadoDespachoDTO GetById(EstadoDespachoDTO dto);
		List<EstadoDespachoDTO> Search(EstadoDespachoDTO dto);
		EstadoDespachoDTO SingleSearch(EstadoDespachoDTO dto);
		int Insert(EstadoDespachoDTO dto);
		int Update(EstadoDespachoDTO dto);
		void Delete(EstadoDespachoDTO dto);
		EstadoDespachoDTO ObtenerPorCodigo(EstadoDespachoDTO dto);
	}
}
