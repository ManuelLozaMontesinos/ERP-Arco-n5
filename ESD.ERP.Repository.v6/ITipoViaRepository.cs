using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoViaRepository : IGenericRepository<TipoVia>
	{
		TipoVia ObtenerPorCodigo(string codigo);
	}
}
