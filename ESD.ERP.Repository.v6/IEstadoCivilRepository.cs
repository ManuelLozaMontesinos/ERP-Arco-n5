using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoCivilRepository : IGenericRepository<EstadoCivil>
	{
		EstadoCivil ObtenerPorCodigo(string codigo);
	}
}
