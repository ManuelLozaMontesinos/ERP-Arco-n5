using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IListaPrecioProveedorProductoRepository : IGenericRepository<ListaPrecioProveedorProducto>
	{
		List<dynamic> ListarSaldoAlmacen(ListaPrecioProveedorProducto entity);
	}
}
