using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoContratoRepository : IGenericRepository<TipoContrato>
	{
		TipoContrato ObtenerPorCodigo(string codigo);
	}
}
