using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoAprobacionProxy
	{
		EstadoAprobacionDTO GetById(EstadoAprobacionDTO dto);
		List<EstadoAprobacionDTO> Search(EstadoAprobacionDTO dto);
		EstadoAprobacionDTO SingleSearch(EstadoAprobacionDTO dto);
		int Insert(EstadoAprobacionDTO dto);
		int Update(EstadoAprobacionDTO dto);
		void Delete(EstadoAprobacionDTO dto);
		EstadoAprobacionDTO ObtenerPorCodigo(EstadoAprobacionDTO dto);
	}
}
