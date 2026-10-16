using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoCargoAbonoRepository : IGenericRepository<TipoCargoAbono>
	{
		TipoCargoAbono ObtenerPorCodigo(string codigo);
	}
}
