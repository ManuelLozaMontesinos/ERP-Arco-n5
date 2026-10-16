using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IMesContableRepository : IGenericRepository<MesContable>
	{
		MesContable ObtenerPorCodigo(string codigo);
	}
}
