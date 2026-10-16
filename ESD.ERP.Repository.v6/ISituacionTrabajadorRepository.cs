using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ISituacionTrabajadorRepository : IGenericRepository<SituacionTrabajador>
	{
		SituacionTrabajador ObtenerPorCodigo(string codigo);
	}
}
