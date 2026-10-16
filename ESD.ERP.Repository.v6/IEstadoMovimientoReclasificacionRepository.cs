using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoMovimientoReclasificacionRepository : IGenericRepository<EstadoMovimientoReclasificacion>
	{
		EstadoMovimientoReclasificacion ObtenerPorCodigo(string codigo);
	}
}
