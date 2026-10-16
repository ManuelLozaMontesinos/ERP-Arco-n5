using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ICortesGratisBusiness
	{
		CortesGratisDTO GetById(int id);
		List<CortesGratisDTO> Search(CortesGratisDTO dto);
        CortesGratisDTO SingleSearch(CortesGratisDTO dto);
		int Insert(CortesGratisDTO dto);
		int Update(CortesGratisDTO dto);
	}
}
