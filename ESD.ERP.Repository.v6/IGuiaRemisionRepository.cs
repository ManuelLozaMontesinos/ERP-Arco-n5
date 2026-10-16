using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IGuiaRemisionRepository : IGenericRepository<GuiaRemision>
	{
		void ActualizarEstado(GuiaRemision entity);
		void ActualizarEstadoAprobacion(GuiaRemision entity);
		void ActualizarEmisionElectronica(GuiaRemision entity);
		void ActualizarEmision(GuiaRemision entity);
		void ActualizarPeso(GuiaRemision entity);

    }
}
