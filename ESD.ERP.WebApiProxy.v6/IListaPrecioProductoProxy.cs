using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IListaPrecioProductoProxy
	{
		ListaPrecioProductoDTO GetById(ListaPrecioProductoDTO dto);
		List<ListaPrecioProductoDTO> Search(ListaPrecioProductoDTO dto);
		ListaPrecioProductoDTO SingleSearch(ListaPrecioProductoDTO dto);
		int Insert(ListaPrecioProductoDTO dto);
		int Update(ListaPrecioProductoDTO dto);
		void Delete(ListaPrecioProductoDTO dto);
		void ActualizarLote(List<ListaPrecioProductoDTO> list);
		string ListarSaldoAlmacenGrid(ListaPrecioProductoDTO dto);
	}
}
