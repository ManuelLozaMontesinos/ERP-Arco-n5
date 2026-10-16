using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ISctrSaludRepository : IGenericRepository<SctrSalud>
	{
		SctrSalud ObtenerPorCodigo(string codigo);
	}
}
