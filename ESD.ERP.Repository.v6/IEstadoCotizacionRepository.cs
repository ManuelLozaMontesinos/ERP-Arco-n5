using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoCotizacionRepository : IGenericRepository<EstadoCotizacion>
	{
		EstadoCotizacion ObtenerPorCodigo(string codigo);
	}
}
