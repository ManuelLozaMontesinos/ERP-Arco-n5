using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ICajaPersonalProxy
	{
		CajaPersonalDTO GetById(CajaPersonalDTO dto);
		List<CajaPersonalDTO> Search(CajaPersonalDTO dto);
		CajaPersonalDTO SingleSearch(CajaPersonalDTO dto);
		int Insert(CajaPersonalDTO dto);
		int Update(CajaPersonalDTO dto);
		void Delete(CajaPersonalDTO dto);
	}
}
