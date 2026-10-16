using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoAprobacionBusiness
	{
		EstadoAprobacionDTO GetById(int id);
		List<EstadoAprobacionDTO> Search(EstadoAprobacionDTO dto);
		EstadoAprobacionDTO SingleSearch(EstadoAprobacionDTO dto);
		int Insert(EstadoAprobacionDTO dto);
		int Update(EstadoAprobacionDTO dto);
		void Delete(EstadoAprobacionDTO dto);
		EstadoAprobacionDTO ObtenerPorCodigo(string codigo);
	}
}
