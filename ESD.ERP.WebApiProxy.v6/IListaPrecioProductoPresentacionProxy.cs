using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IListaPrecioProductoPresentacionProxy
	{
		ListaPrecioProductoPresentacionDTO GetById(ListaPrecioProductoPresentacionDTO dto);
		List<ListaPrecioProductoPresentacionDTO> Search(ListaPrecioProductoPresentacionDTO dto);
		ListaPrecioProductoPresentacionDTO SingleSearch(ListaPrecioProductoPresentacionDTO dto);
		int Insert(ListaPrecioProductoPresentacionDTO dto);
		int Update(ListaPrecioProductoPresentacionDTO dto);
		void Delete(ListaPrecioProductoPresentacionDTO dto);
	}
}
