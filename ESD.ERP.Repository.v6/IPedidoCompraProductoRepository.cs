using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IPedidoCompraProductoRepository : IGenericRepository<PedidoCompraProducto>
	{
		void ActualizarLote(List<PedidoCompraProducto> list);
		void ActualizarProducto(PedidoCompraProducto entity);
	}
}
