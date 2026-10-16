using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPlanillaProcesoBusiness
	{
		PlanillaProcesoDTO GetById(int id);
		List<PlanillaProcesoDTO> Search(PlanillaProcesoDTO dto);
		PlanillaProcesoDTO SingleSearch(PlanillaProcesoDTO dto);
		int Insert(PlanillaProcesoDTO dto);
		int Update(PlanillaProcesoDTO dto);
		void Delete(PlanillaProcesoDTO dto);
		void Calcular(PlanillaProcesoDTO dto);
	}
}
