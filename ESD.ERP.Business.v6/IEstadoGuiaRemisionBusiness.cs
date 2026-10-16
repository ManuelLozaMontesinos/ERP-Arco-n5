using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoGuiaRemisionBusiness
	{
		EstadoGuiaRemisionDTO GetById(int id);
		List<EstadoGuiaRemisionDTO> Search(EstadoGuiaRemisionDTO dto);
		EstadoGuiaRemisionDTO SingleSearch(EstadoGuiaRemisionDTO dto);
		int Insert(EstadoGuiaRemisionDTO dto);
		int Update(EstadoGuiaRemisionDTO dto);
		void Delete(EstadoGuiaRemisionDTO dto);
		EstadoGuiaRemisionDTO ObtenerPorCodigo(string codigo);
	}
}
