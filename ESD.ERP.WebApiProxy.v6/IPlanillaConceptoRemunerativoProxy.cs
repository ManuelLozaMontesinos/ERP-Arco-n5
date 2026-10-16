using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPlanillaConceptoRemunerativoProxy
	{
		PlanillaConceptoRemunerativoDTO GetById(PlanillaConceptoRemunerativoDTO dto);
		List<PlanillaConceptoRemunerativoDTO> Search(PlanillaConceptoRemunerativoDTO dto);
		PlanillaConceptoRemunerativoDTO SingleSearch(PlanillaConceptoRemunerativoDTO dto);
		int Insert(PlanillaConceptoRemunerativoDTO dto);
		int Update(PlanillaConceptoRemunerativoDTO dto);
		void Delete(PlanillaConceptoRemunerativoDTO dto);
	}
}
