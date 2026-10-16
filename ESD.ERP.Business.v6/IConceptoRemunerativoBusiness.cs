using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IConceptoRemunerativoBusiness
	{
		ConceptoRemunerativoDTO GetById(int id);
		List<ConceptoRemunerativoDTO> Search(ConceptoRemunerativoDTO dto);
		ConceptoRemunerativoDTO SingleSearch(ConceptoRemunerativoDTO dto);
		int Insert(ConceptoRemunerativoDTO dto);
		int Update(ConceptoRemunerativoDTO dto);
		void Delete(ConceptoRemunerativoDTO dto);
	}
}
