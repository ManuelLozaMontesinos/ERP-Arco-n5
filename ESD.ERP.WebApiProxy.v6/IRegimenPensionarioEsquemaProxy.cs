using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRegimenPensionarioEsquemaProxy
	{
		RegimenPensionarioEsquemaDTO GetById(RegimenPensionarioEsquemaDTO dto);
		List<RegimenPensionarioEsquemaDTO> Search(RegimenPensionarioEsquemaDTO dto);
		RegimenPensionarioEsquemaDTO SingleSearch(RegimenPensionarioEsquemaDTO dto);
		int Insert(RegimenPensionarioEsquemaDTO dto);
		int Update(RegimenPensionarioEsquemaDTO dto);
		void Delete(RegimenPensionarioEsquemaDTO dto);
	}
}
