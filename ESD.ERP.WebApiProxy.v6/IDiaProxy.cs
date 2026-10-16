using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IDiaProxy
	{
		DiaDTO GetById(DiaDTO dto);
		List<DiaDTO> Search(DiaDTO dto);
		DiaDTO SingleSearch(DiaDTO dto);
		int Insert(DiaDTO dto);
		int Update(DiaDTO dto);
		void Delete(DiaDTO dto);
	}
}
