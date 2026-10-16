using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPlanillaCalculoBusiness
	{
		PlanillaCalculoDTO GetById(int id);
		List<PlanillaCalculoDTO> Search(PlanillaCalculoDTO dto);
		PlanillaCalculoDTO SingleSearch(PlanillaCalculoDTO dto);
		int Insert(PlanillaCalculoDTO dto);
		int Update(PlanillaCalculoDTO dto);
		void Delete(PlanillaCalculoDTO dto);
	}
}
