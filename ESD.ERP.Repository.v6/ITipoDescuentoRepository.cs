using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoDescuentoRepository : IGenericRepository<TipoDescuento>
	{
		TipoDescuento ObtenerPorCodigo(string codigo);
	}
}
