using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ISituacionEspecialTrabajadorRepository : IGenericRepository<SituacionEspecialTrabajador>
	{
		SituacionEspecialTrabajador ObtenerPorCodigo(string codigo);
	}
}
