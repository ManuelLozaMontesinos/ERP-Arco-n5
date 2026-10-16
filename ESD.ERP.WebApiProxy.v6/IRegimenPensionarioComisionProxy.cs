using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRegimenPensionarioComisionProxy
	{
		RegimenPensionarioComisionDTO GetById(RegimenPensionarioComisionDTO dto);
		List<RegimenPensionarioComisionDTO> Search(RegimenPensionarioComisionDTO dto);
		RegimenPensionarioComisionDTO SingleSearch(RegimenPensionarioComisionDTO dto);
		int Insert(RegimenPensionarioComisionDTO dto);
		int Update(RegimenPensionarioComisionDTO dto);
		void Delete(RegimenPensionarioComisionDTO dto);
	}
}
