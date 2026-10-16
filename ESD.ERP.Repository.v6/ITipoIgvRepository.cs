using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoIgvRepository : IGenericRepository<TipoIgv>
	{
		TipoIgv ObtenerPorCodigo(string codigo);
	}
}
