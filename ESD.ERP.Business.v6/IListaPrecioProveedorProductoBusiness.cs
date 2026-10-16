using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IListaPrecioProveedorProductoBusiness
	{
		ListaPrecioProveedorProductoDTO GetById(int id);
		List<ListaPrecioProveedorProductoDTO> Search(ListaPrecioProveedorProductoDTO dto);
		ListaPrecioProveedorProductoDTO SingleSearch(ListaPrecioProveedorProductoDTO dto);
		int Insert(ListaPrecioProveedorProductoDTO dto);
		int Update(ListaPrecioProveedorProductoDTO dto);
		void Delete(ListaPrecioProveedorProductoDTO dto);
		List<dynamic> ListarSaldoAlmacen(ListaPrecioProveedorProductoDTO dto);
	}
}
