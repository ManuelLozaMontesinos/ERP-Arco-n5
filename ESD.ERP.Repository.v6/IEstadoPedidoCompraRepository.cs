using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoPedidoCompraRepository : IGenericRepository<EstadoPedidoCompra>
	{
		EstadoPedidoCompra ObtenerPorCodigo(string codigo);
	}
}
