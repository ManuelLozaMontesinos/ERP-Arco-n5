using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPlanillaProcesoProxy
	{
		PlanillaProcesoDTO GetById(PlanillaProcesoDTO dto);
		List<PlanillaProcesoDTO> Search(PlanillaProcesoDTO dto);
		PlanillaProcesoDTO SingleSearch(PlanillaProcesoDTO dto);
		int Insert(PlanillaProcesoDTO dto);
		int Update(PlanillaProcesoDTO dto);
		void Delete(PlanillaProcesoDTO dto);
		void Calcular(PlanillaProcesoDTO dto);
	}
}
