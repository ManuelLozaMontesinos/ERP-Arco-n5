using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IPersonalSolicitudAccesoRepository : IGenericRepository<PersonalSolicitudAcceso>
	{
		void ActualizarEstadoAprobacion(PersonalSolicitudAcceso entity);
	}
}