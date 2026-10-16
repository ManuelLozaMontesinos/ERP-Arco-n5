using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoProveedorRepository : IGenericRepository<TipoProveedor>
	{
		TipoProveedor ObtenerPorCodigo(string codigo);
	}
}
