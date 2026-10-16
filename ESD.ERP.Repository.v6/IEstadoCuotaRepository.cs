using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoCuotaRepository : IGenericRepository<EstadoCuota>
	{
		EstadoCuota ObtenerPorCodigo(string codigo);
	}
}
