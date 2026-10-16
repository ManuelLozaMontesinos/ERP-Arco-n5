using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ICajaProxy
	{
		CajaDTO GetById(CajaDTO dto);
		List<CajaDTO> Search(CajaDTO dto);
		CajaDTO SingleSearch(CajaDTO dto);
		int Insert(CajaDTO dto);
		int Update(CajaDTO dto);
		void Delete(CajaDTO dto);
	}
}
