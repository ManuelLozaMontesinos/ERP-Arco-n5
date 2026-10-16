using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IOrdenProduccionRepository : IGenericRepository<OrdenProduccion>
	{
		void ActualizarEstadoDespacho(OrdenProduccion entity);
		void ActualizarEstado(OrdenProduccion entity);
		void EliminarSubProcesos(OrdenProduccion entity);
		void ActualizarFecha(OrdenProduccion entity);
		void CerrarInsumo(OrdenProduccion entity);
		void CambiarEstado(OrdenProduccion entity);
		void EliminarInsumo(int id);
    }
}
