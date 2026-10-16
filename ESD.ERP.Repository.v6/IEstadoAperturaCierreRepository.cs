using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoAperturaCierreRepository : IGenericRepository<EstadoAperturaCierre>
	{
		EstadoAperturaCierre ObtenerPorCodigo(string codigo);
	}
}
