using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoSunatRepository : IGenericRepository<EstadoSunat>
	{
		EstadoSunat ObtenerPorCodigo(string codigo);
	}
}
