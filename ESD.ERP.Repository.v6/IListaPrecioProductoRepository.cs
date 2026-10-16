using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IListaPrecioProductoRepository : IGenericRepository<ListaPrecioProducto>
	{
		void ActualizarLote(List<ListaPrecioProducto> list);
		List<dynamic> ListarSaldoAlmacen(ListaPrecioProducto entity);
	}
}
