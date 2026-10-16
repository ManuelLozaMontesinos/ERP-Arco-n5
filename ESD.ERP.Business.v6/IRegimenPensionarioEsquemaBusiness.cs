using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRegimenPensionarioEsquemaBusiness
	{
		RegimenPensionarioEsquemaDTO GetById(int id);
		List<RegimenPensionarioEsquemaDTO> Search(RegimenPensionarioEsquemaDTO dto);
		RegimenPensionarioEsquemaDTO SingleSearch(RegimenPensionarioEsquemaDTO dto);
		int Insert(RegimenPensionarioEsquemaDTO dto);
		int Update(RegimenPensionarioEsquemaDTO dto);
		void Delete(RegimenPensionarioEsquemaDTO dto);
	}
}
