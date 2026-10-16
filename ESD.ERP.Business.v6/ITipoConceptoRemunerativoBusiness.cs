using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoConceptoRemunerativoBusiness
	{
		TipoConceptoRemunerativoDTO GetById(int id);
		List<TipoConceptoRemunerativoDTO> Search(TipoConceptoRemunerativoDTO dto);
		TipoConceptoRemunerativoDTO SingleSearch(TipoConceptoRemunerativoDTO dto);
		int Insert(TipoConceptoRemunerativoDTO dto);
		int Update(TipoConceptoRemunerativoDTO dto);
		void Delete(TipoConceptoRemunerativoDTO dto);
	}
}
