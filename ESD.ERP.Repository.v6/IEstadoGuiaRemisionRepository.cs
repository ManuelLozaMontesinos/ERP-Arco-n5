using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoGuiaRemisionRepository : IGenericRepository<EstadoGuiaRemision>
	{
		EstadoGuiaRemision ObtenerPorCodigo(string codigo);
	}
}
