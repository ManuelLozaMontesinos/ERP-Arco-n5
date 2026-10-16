using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IListaPrecioProductoPresentacionBusiness
	{
		ListaPrecioProductoPresentacionDTO GetById(int id);
		List<ListaPrecioProductoPresentacionDTO> Search(ListaPrecioProductoPresentacionDTO dto);
		ListaPrecioProductoPresentacionDTO SingleSearch(ListaPrecioProductoPresentacionDTO dto);
		int Insert(ListaPrecioProductoPresentacionDTO dto);
		int Update(ListaPrecioProductoPresentacionDTO dto);
		void Delete(ListaPrecioProductoPresentacionDTO dto);
	}
}
