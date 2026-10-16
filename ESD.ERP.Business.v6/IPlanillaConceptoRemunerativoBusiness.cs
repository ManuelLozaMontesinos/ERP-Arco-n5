using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPlanillaConceptoRemunerativoBusiness
	{
		PlanillaConceptoRemunerativoDTO GetById(int id);
		List<PlanillaConceptoRemunerativoDTO> Search(PlanillaConceptoRemunerativoDTO dto);
		PlanillaConceptoRemunerativoDTO SingleSearch(PlanillaConceptoRemunerativoDTO dto);
		int Insert(PlanillaConceptoRemunerativoDTO dto);
		int Update(PlanillaConceptoRemunerativoDTO dto);
		void Delete(PlanillaConceptoRemunerativoDTO dto);
	}
}
