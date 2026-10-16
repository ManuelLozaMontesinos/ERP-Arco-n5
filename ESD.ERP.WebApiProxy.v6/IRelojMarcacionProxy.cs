using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRelojMarcacionProxy
	{
		RelojMarcacionDTO GetById(RelojMarcacionDTO dto);
		List<RelojMarcacionDTO> Search(RelojMarcacionDTO dto);
		RelojMarcacionDTO SingleSearch(RelojMarcacionDTO dto);
		int Insert(RelojMarcacionDTO dto);
		int Update(RelojMarcacionDTO dto);
		void Delete(RelojMarcacionDTO dto);
	}
}
