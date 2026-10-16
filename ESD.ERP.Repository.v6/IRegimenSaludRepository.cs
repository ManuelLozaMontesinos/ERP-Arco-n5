using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IRegimenSaludRepository : IGenericRepository<RegimenSalud>
	{
		RegimenSalud ObtenerPorCodigo(string codigo);
	}
}
