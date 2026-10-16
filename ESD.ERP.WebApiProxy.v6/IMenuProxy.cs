using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMenuProxy
	{
		MenuDTO GetById(MenuDTO dto);
		List<MenuDTO> Search(MenuDTO dto);
		MenuDTO SingleSearch(MenuDTO dto);
		int Insert(MenuDTO dto);
		int Update(MenuDTO dto);
		void Delete(MenuDTO dto);
	}
}
