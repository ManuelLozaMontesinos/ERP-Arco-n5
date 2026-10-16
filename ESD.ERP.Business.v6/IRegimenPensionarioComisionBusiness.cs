using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRegimenPensionarioComisionBusiness
	{
		RegimenPensionarioComisionDTO GetById(int id);
		List<RegimenPensionarioComisionDTO> Search(RegimenPensionarioComisionDTO dto);
		RegimenPensionarioComisionDTO SingleSearch(RegimenPensionarioComisionDTO dto);
		int Insert(RegimenPensionarioComisionDTO dto);
		int Update(RegimenPensionarioComisionDTO dto);
		void Delete(RegimenPensionarioComisionDTO dto);
	}
}
