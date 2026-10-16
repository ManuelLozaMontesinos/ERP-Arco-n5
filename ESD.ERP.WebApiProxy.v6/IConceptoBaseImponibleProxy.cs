using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IConceptoBaseImponibleProxy
	{
		ConceptoBaseImponibleDTO GetById(ConceptoBaseImponibleDTO dto);
		List<ConceptoBaseImponibleDTO> Search(ConceptoBaseImponibleDTO dto);
		ConceptoBaseImponibleDTO SingleSearch(ConceptoBaseImponibleDTO dto);
		int Insert(ConceptoBaseImponibleDTO dto);
		int Update(ConceptoBaseImponibleDTO dto);
		void Delete(ConceptoBaseImponibleDTO dto);
		List<ConceptoBaseImponibleDTO> ListarPorConceptoRemunerativoId(ConceptoBaseImponibleDTO dto);
	}
}
