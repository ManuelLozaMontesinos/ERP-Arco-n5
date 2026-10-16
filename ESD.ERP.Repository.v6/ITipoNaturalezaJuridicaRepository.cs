using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoNaturalezaJuridicaRepository : IGenericRepository<TipoNaturalezaJuridica>
	{
		TipoNaturalezaJuridica ObtenerPorCodigo(string codigo);
	}
}
