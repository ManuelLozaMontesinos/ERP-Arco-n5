using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoTrabajadorRepository : IGenericRepository<TipoTrabajador>
	{
		TipoTrabajador ObtenerPorCodigo(string codigo);
	}
}
