using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IRegimenLaboralRepository : IGenericRepository<RegimenLaboral>
	{
		RegimenLaboral ObtenerPorCodigo(string codigo);
	}
}
