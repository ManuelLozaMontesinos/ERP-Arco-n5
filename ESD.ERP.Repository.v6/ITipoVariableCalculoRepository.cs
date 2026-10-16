using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoVariableCalculoRepository : IGenericRepository<TipoVariableCalculo>
	{
		TipoVariableCalculo ObtenerPorCodigo(string codigo);
	}
}
