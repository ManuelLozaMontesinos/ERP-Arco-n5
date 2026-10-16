using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRelojMarcacionBusiness
	{
		RelojMarcacionDTO GetById(int id);
		List<RelojMarcacionDTO> Search(RelojMarcacionDTO dto);
		RelojMarcacionDTO SingleSearch(RelojMarcacionDTO dto);
		int Insert(RelojMarcacionDTO dto);
		int Update(RelojMarcacionDTO dto);
		void Delete(RelojMarcacionDTO dto);
	}
}
