using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IOcupacionRepository : IGenericRepository<Ocupacion>
	{
		Ocupacion ObtenerPorCodigo(string codigo);
	}
}
