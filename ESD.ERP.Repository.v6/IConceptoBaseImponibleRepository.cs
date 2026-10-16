using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IConceptoBaseImponibleRepository : IGenericRepository<ConceptoBaseImponible>
	{
		List<ConceptoBaseImponible> ListarPorConceptoRemunerativoId(ConceptoBaseImponible entity);
	}
}
