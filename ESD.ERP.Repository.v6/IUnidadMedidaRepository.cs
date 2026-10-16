using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IUnidadMedidaRepository : IGenericRepository<UnidadMedida>
	{
		UnidadMedida ObtenerPorCodigo(string codigo);
	}
}
