using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMenuBusiness
	{
		MenuDTO GetById(int id);
		List<MenuDTO> Search(MenuDTO dto);
		MenuDTO SingleSearch(MenuDTO dto);
		int Insert(MenuDTO dto);
		int Update(MenuDTO dto);
		void Delete(MenuDTO dto);
	}
}
