using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITablaBusiness
	{
		TablaDTO GetById(int id);
		List<TablaDTO> Search(TablaDTO dto);
		TablaDTO SingleSearch(TablaDTO dto);
		int Insert(TablaDTO dto);
		int Update(TablaDTO dto);
		void Delete(TablaDTO dto);
		string ObtenerCorrelativo(TablaCorrelativoDTO dto);
	}
}
