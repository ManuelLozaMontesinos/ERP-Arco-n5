using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IRegimenPensionarioRepository : IGenericRepository<RegimenPensionario>
	{
		RegimenPensionario ObtenerPorCodigo(string codigo);
	}
}
