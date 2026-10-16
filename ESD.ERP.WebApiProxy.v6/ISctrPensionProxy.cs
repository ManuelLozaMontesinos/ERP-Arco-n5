using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ISctrPensionProxy
	{
		SctrPensionDTO GetById(SctrPensionDTO dto);
		List<SctrPensionDTO> Search(SctrPensionDTO dto);
		SctrPensionDTO SingleSearch(SctrPensionDTO dto);
		int Insert(SctrPensionDTO dto);
		int Update(SctrPensionDTO dto);
		void Delete(SctrPensionDTO dto);
	}
}
