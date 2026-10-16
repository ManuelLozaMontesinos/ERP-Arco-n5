using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoComprobanteRepository : IGenericRepository<TipoComprobante>
	{
		TipoComprobante ObtenerPorCodigo(string codigo);
	}
}
