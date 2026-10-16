using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IListaPrecioBusiness
	{
		ListaPrecioDTO GetById(int id);
		List<ListaPrecioDTO> Search(ListaPrecioDTO dto);
		ListaPrecioDTO SingleSearch(ListaPrecioDTO dto);
		int Insert(ListaPrecioDTO dto);
		int Update(ListaPrecioDTO dto);
		void Delete(ListaPrecioDTO dto);
    }
}
