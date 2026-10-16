using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoPlanillaProcesoBusiness
	{
		EstadoPlanillaProcesoDTO GetById(int id);
		List<EstadoPlanillaProcesoDTO> Search(EstadoPlanillaProcesoDTO dto);
		EstadoPlanillaProcesoDTO SingleSearch(EstadoPlanillaProcesoDTO dto);
		int Insert(EstadoPlanillaProcesoDTO dto);
		int Update(EstadoPlanillaProcesoDTO dto);
		void Delete(EstadoPlanillaProcesoDTO dto);
		EstadoPlanillaProcesoDTO ObtenerPorCodigo(string codigo);
	}
}
