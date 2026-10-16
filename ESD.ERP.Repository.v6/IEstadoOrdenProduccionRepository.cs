using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoOrdenProduccionRepository : IGenericRepository<EstadoOrdenProduccion>
	{
		EstadoOrdenProduccion ObtenerPorCodigo(string codigo);
	}
}
