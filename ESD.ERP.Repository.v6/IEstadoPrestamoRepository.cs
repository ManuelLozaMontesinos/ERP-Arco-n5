using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoPrestamoRepository : IGenericRepository<EstadoPrestamo>
	{
		EstadoPrestamo ObtenerPorCodigo(string codigo);
	}
}
