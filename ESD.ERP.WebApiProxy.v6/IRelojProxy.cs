using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRelojProxy
	{
		RelojDTO GetById(RelojDTO dto);
		List<RelojDTO> Search(RelojDTO dto);
		RelojDTO SingleSearch(RelojDTO dto);
		int Insert(RelojDTO dto);
		int Update(RelojDTO dto);
		void Delete(RelojDTO dto);
	}
}
