using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPlanillaProcesoRegularizacionProxy
	{
		PlanillaProcesoRegularizacionDTO GetById(PlanillaProcesoRegularizacionDTO dto);
		List<PlanillaProcesoRegularizacionDTO> Search(PlanillaProcesoRegularizacionDTO dto);
		PlanillaProcesoRegularizacionDTO SingleSearch(PlanillaProcesoRegularizacionDTO dto);
		int Insert(PlanillaProcesoRegularizacionDTO dto);
		int Update(PlanillaProcesoRegularizacionDTO dto);
		void Delete(PlanillaProcesoRegularizacionDTO dto);
	}
}
