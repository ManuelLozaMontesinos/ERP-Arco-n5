using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IListaPrecioProveedorProductoProxy
	{
		ListaPrecioProveedorProductoDTO GetById(ListaPrecioProveedorProductoDTO dto);
		List<ListaPrecioProveedorProductoDTO> Search(ListaPrecioProveedorProductoDTO dto);
		ListaPrecioProveedorProductoDTO SingleSearch(ListaPrecioProveedorProductoDTO dto);
		int Insert(ListaPrecioProveedorProductoDTO dto);
		int Update(ListaPrecioProveedorProductoDTO dto);
		void Delete(ListaPrecioProveedorProductoDTO dto);
		string ListarSaldoAlmacenGrid(ListaPrecioProveedorProductoDTO dto);
	}
}
