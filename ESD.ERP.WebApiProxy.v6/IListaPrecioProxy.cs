using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IListaPrecioProxy
	{
		ListaPrecioDTO GetById(ListaPrecioDTO dto);
		List<ListaPrecioDTO> Search(ListaPrecioDTO dto);
		ListaPrecioDTO SingleSearch(ListaPrecioDTO dto);
		int Insert(ListaPrecioDTO dto);
		int Update(ListaPrecioDTO dto);
		void Delete(ListaPrecioDTO dto);

    }
}
