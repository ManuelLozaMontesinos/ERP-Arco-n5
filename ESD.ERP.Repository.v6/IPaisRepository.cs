using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IPaisRepository : IGenericRepository<Pais>
	{
		Pais ObtenerPorCodigo(string codigo);
	}
}
