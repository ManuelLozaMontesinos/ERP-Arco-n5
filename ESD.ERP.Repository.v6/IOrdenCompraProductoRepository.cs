using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IOrdenCompraProductoRepository : IGenericRepository<OrdenCompraProducto>
	{
		void EliminarPorOrdenCompraId(OrdenCompraProducto entity);
		void ActualizarCantidadIngresada(OrdenCompraProducto entity);
	}
}
