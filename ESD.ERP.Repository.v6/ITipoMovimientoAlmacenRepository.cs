using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoMovimientoAlmacenRepository : IGenericRepository<TipoMovimientoAlmacen>
	{
		TipoMovimientoAlmacen ObtenerPorCodigo(string codigo);
	}
}
