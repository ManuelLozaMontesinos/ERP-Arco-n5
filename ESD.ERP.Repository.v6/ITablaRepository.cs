using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITablaRepository : IGenericRepository<Tabla>
	{
		Tabla ObtenerPorCodigo(string codigo);
	}
}
