using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IMovimientoReclasificacionRepository : IGenericRepository<MovimientoReclasificacion>
	{
		void ActualizarEstadoMovimiento(MovimientoReclasificacion entity);
		void ActualizarEstado(MovimientoReclasificacion entity);
		void AprobDesa(MovimientoReclasificacion entity);
    }
}
