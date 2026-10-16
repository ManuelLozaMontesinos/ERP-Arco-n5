using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoPlanillaProcesoProxy
	{
		EstadoPlanillaProcesoDTO GetById(EstadoPlanillaProcesoDTO dto);
		List<EstadoPlanillaProcesoDTO> Search(EstadoPlanillaProcesoDTO dto);
		EstadoPlanillaProcesoDTO SingleSearch(EstadoPlanillaProcesoDTO dto);
		int Insert(EstadoPlanillaProcesoDTO dto);
		int Update(EstadoPlanillaProcesoDTO dto);
		void Delete(EstadoPlanillaProcesoDTO dto);
		EstadoPlanillaProcesoDTO ObtenerPorCodigo(EstadoPlanillaProcesoDTO dto);
	}
}
