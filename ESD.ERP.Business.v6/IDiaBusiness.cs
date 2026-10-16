using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IDiaBusiness
	{
		DiaDTO GetById(int id);
		List<DiaDTO> Search(DiaDTO dto);
		DiaDTO SingleSearch(DiaDTO dto);
		int Insert(DiaDTO dto);
		int Update(DiaDTO dto);
		void Delete(DiaDTO dto);
	}
}
