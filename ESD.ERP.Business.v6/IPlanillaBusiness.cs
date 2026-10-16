using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPlanillaBusiness
	{
		PlanillaDTO GetById(int id);
		List<PlanillaDTO> Search(PlanillaDTO dto);
		PlanillaDTO SingleSearch(PlanillaDTO dto);
		int Insert(PlanillaDTO dto);
		int Update(PlanillaDTO dto);
		void Delete(PlanillaDTO dto);
	}
}
