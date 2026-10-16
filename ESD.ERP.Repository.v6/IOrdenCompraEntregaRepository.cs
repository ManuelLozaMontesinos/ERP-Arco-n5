using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IOrdenCompraEntregaRepository : IGenericRepository<OrdenCompraEntrega>
	{
		void AgregarDesdeOrdenCompraProducto(OrdenCompraEntrega entity);
		void EliminarPorOrdenCompraId(OrdenCompraEntrega entity);
	}
}
