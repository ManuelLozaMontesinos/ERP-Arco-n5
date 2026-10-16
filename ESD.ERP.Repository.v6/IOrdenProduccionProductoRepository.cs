using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IOrdenProduccionProductoRepository : IGenericRepository<OrdenProduccionProducto>
	{
		void UpdateOrden(OrdenProduccionProducto entity);
		void actualizarEnvioProd(OrdenProduccionProducto entity);
		void actualizarInsumos(OrdenProduccionProducto entity);

    }
}
