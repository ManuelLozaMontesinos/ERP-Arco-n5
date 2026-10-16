using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ICajaBusiness
	{
		CajaDTO GetById(int id);
		List<CajaDTO> Search(CajaDTO dto);
		CajaDTO SingleSearch(CajaDTO dto);
		int Insert(CajaDTO dto);
		int Update(CajaDTO dto);
		void Delete(CajaDTO dto);
	}
}
