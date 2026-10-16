using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoPlanillaProcesoRepository : IGenericRepository<EstadoPlanillaProceso>
	{
		EstadoPlanillaProceso ObtenerPorCodigo(string codigo);
	}
}
