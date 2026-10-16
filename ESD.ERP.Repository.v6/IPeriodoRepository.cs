using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IPeriodoRepository : IGenericRepository<Periodo>
	{
		Periodo ObtenerPorCodigo(int codigo);
	}
}
