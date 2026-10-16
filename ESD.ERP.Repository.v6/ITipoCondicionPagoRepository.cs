using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoCondicionPagoRepository : IGenericRepository<TipoCondicionPago>
	{
		TipoCondicionPago ObtenerPorCodigo(string codigo);
	}
}
