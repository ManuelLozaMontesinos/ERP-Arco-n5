using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoProductoRepository : IGenericRepository<EstadoProducto>
	{
		EstadoProducto ObtenerPorCodigo(string codigo);
	}
}
