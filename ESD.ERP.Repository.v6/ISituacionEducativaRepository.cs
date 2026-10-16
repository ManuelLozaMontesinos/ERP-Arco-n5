using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ISituacionEducativaRepository : IGenericRepository<SituacionEducativa>
	{
		SituacionEducativa ObtenerPorCodigo(string codigo);
	}
}
