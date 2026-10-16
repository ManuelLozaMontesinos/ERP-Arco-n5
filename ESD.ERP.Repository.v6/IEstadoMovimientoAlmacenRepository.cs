using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoMovimientoAlmacenRepository : IGenericRepository<EstadoMovimientoAlmacen>
	{
		EstadoMovimientoAlmacen ObtenerPorCodigo(string codigo);
	}
}
