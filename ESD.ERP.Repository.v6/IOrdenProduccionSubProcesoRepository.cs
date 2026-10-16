using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IOrdenProduccionSubProcesoRepository : IGenericRepository<OrdenProduccionSubProceso>
	{
		void ActualizarInicio(OrdenProduccionSubProceso entity);
		void ActualizarFin(OrdenProduccionSubProceso entity);
		void Maquina(OrdenProduccionSubProceso entity);
	}
}
