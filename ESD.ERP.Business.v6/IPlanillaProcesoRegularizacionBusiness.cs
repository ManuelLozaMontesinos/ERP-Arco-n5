using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPlanillaProcesoRegularizacionBusiness
	{
		PlanillaProcesoRegularizacionDTO GetById(int id);
		List<PlanillaProcesoRegularizacionDTO> Search(PlanillaProcesoRegularizacionDTO dto);
		PlanillaProcesoRegularizacionDTO SingleSearch(PlanillaProcesoRegularizacionDTO dto);
		int Insert(PlanillaProcesoRegularizacionDTO dto);
		int Update(PlanillaProcesoRegularizacionDTO dto);
		void Delete(PlanillaProcesoRegularizacionDTO dto);
	}
}
