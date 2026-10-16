using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoDatoRepository : IGenericRepository<TipoDato>
	{
		TipoDato ObtenerPorCodigo(string codigo);
	}
}
