using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IPedidoCompraRepository : IGenericRepository<PedidoCompra>
	{
		void ActualizarEstadoAprobacion(PedidoCompra entity);
		void ActualizarEstadoPedido(PedidoCompra entity);
	}
}
