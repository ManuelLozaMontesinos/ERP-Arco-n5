using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoProductoRepository : IGenericRepository<TipoProducto>
	{
		TipoProducto ObtenerPorCodigo(string codigo);
	}
}
