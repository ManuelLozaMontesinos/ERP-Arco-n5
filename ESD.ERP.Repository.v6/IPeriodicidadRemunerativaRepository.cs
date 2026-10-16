using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IPeriodicidadRemunerativaRepository : IGenericRepository<PeriodicidadRemunerativa>
	{
		PeriodicidadRemunerativa ObtenerPorCodigo(string codigo);
	}
}
