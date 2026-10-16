using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPlanillaProxy
	{
		PlanillaDTO GetById(PlanillaDTO dto);
		List<PlanillaDTO> Search(PlanillaDTO dto);
		PlanillaDTO SingleSearch(PlanillaDTO dto);
		int Insert(PlanillaDTO dto);
		int Update(PlanillaDTO dto);
		void Delete(PlanillaDTO dto);
	}
}
