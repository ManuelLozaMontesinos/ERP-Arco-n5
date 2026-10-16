using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoDespachoRepository : IGenericRepository<EstadoDespacho>
	{
		EstadoDespacho ObtenerPorCodigo(string codigo);
	}
}