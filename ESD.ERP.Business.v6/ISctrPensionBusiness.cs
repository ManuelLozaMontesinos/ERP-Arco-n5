using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ISctrPensionBusiness
	{
		SctrPensionDTO GetById(int id);
		List<SctrPensionDTO> Search(SctrPensionDTO dto);
		SctrPensionDTO SingleSearch(SctrPensionDTO dto);
		int Insert(SctrPensionDTO dto);
		int Update(SctrPensionDTO dto);
		void Delete(SctrPensionDTO dto);
	}
}
