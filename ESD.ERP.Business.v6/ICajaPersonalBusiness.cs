using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ICajaPersonalBusiness
	{
		CajaPersonalDTO GetById(int id);
		List<CajaPersonalDTO> Search(CajaPersonalDTO dto);
		CajaPersonalDTO SingleSearch(CajaPersonalDTO dto);
		int Insert(CajaPersonalDTO dto);
		int Update(CajaPersonalDTO dto);
		void Delete(CajaPersonalDTO dto);
	}
}
