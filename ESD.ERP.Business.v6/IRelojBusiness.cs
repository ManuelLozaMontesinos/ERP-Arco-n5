using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRelojBusiness
	{
		RelojDTO GetById(int id);
		List<RelojDTO> Search(RelojDTO dto);
		RelojDTO SingleSearch(RelojDTO dto);
		int Insert(RelojDTO dto);
		int Update(RelojDTO dto);
		void Delete(RelojDTO dto);
	}
}
