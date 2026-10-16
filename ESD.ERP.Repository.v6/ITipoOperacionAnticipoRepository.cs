using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoOperacionAnticipoRepository : IGenericRepository<TipoOperacionAnticipo>
	{
		TipoOperacionAnticipo ObtenerPorCodigo(string codigo);
	}
}
