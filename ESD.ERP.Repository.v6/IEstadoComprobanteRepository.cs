using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoComprobanteRepository : IGenericRepository<EstadoComprobante>
	{
		EstadoComprobante ObtenerPorCodigo(string codigo);
	}
}
