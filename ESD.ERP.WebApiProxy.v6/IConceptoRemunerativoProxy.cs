using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IConceptoRemunerativoProxy
	{
		ConceptoRemunerativoDTO GetById(ConceptoRemunerativoDTO dto);
		List<ConceptoRemunerativoDTO> Search(ConceptoRemunerativoDTO dto);
		ConceptoRemunerativoDTO SingleSearch(ConceptoRemunerativoDTO dto);
		int Insert(ConceptoRemunerativoDTO dto);
		int Update(ConceptoRemunerativoDTO dto);
		void Delete(ConceptoRemunerativoDTO dto);
	}
}
