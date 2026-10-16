using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoGuiaRemisionProxy
	{
		EstadoGuiaRemisionDTO GetById(EstadoGuiaRemisionDTO dto);
		List<EstadoGuiaRemisionDTO> Search(EstadoGuiaRemisionDTO dto);
		EstadoGuiaRemisionDTO SingleSearch(EstadoGuiaRemisionDTO dto);
		int Insert(EstadoGuiaRemisionDTO dto);
		int Update(EstadoGuiaRemisionDTO dto);
		void Delete(EstadoGuiaRemisionDTO dto);
		EstadoGuiaRemisionDTO ObtenerPorCodigo(EstadoGuiaRemisionDTO dto);
	}
}
