using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoCancelacionCuotaRepository : IGenericRepository<TipoCancelacionCuota>
	{
		TipoCancelacionCuota ObtenerPorCodigo(string codigo);
	}
}
