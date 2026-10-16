using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoOrdenProduccionSubProcesoRepository : IGenericRepository<EstadoOrdenProduccionSubProceso>
	{
		EstadoOrdenProduccionSubProceso ObtenerPorCodigo(string codigo);
	}
}