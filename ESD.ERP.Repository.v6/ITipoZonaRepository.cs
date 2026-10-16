using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoZonaRepository : IGenericRepository<TipoZona>
	{
		TipoZona ObtenerPorCodigo(string codigo);
	}
}
