using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IMovimientoAlmacenProductoRepository : IGenericRepository<MovimientoAlmacenProducto>
	{
		void ActualizarProductoReclasificacion(MovimientoAlmacenProducto entity);
	}
}
