using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IMonedaRepository : IGenericRepository<Moneda>
	{
		Moneda ObtenerPorCodigo(string codigo);
	}
}
