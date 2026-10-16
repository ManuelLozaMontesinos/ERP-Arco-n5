using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IOrigenRegistroRepository : IGenericRepository<OrigenRegistro>
	{
		OrigenRegistro ObtenerPorCodigo(string codigo);
	}
}
