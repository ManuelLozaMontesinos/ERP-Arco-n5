using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IListaPrecioProductoBusiness
	{
		ListaPrecioProductoDTO GetById(int id);
		List<ListaPrecioProductoDTO> Search(ListaPrecioProductoDTO dto);
		ListaPrecioProductoDTO SingleSearch(ListaPrecioProductoDTO dto);
		int Insert(ListaPrecioProductoDTO dto);
		int Update(ListaPrecioProductoDTO dto);
		void Delete(ListaPrecioProductoDTO dto);
		void ActualizarLote(List<ListaPrecioProductoDTO> list);
		List<dynamic> ListarSaldoAlmacen(ListaPrecioProductoDTO dto);
	}
}