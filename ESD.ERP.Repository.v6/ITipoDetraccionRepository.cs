using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoDetraccionRepository : IGenericRepository<TipoDetraccion>
	{
		TipoDetraccion ObtenerPorCodigo(string codigo);
	}
}
