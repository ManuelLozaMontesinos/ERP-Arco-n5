using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITablaProxy
	{
		TablaDTO GetById(TablaDTO dto);
		List<TablaDTO> Search(TablaDTO dto);
		TablaDTO SingleSearch(TablaDTO dto);
		int Insert(TablaDTO dto);
		int Update(TablaDTO dto);
		void Delete(TablaDTO dto);
	}
}
