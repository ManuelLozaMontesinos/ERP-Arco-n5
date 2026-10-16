using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoAprobacionRepository : IGenericRepository<EstadoAprobacion>
	{
		EstadoAprobacion ObtenerPorCodigo(string codigo);
	}
}
