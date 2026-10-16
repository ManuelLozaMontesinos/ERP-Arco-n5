using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoConceptoRemunerativoProxy
	{
		TipoConceptoRemunerativoDTO GetById(TipoConceptoRemunerativoDTO dto);
		List<TipoConceptoRemunerativoDTO> Search(TipoConceptoRemunerativoDTO dto);
		TipoConceptoRemunerativoDTO SingleSearch(TipoConceptoRemunerativoDTO dto);
		int Insert(TipoConceptoRemunerativoDTO dto);
		int Update(TipoConceptoRemunerativoDTO dto);
		void Delete(TipoConceptoRemunerativoDTO dto);
	}
}
