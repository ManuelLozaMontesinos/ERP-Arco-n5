using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IProcesoIntegracionRepository : IGenericRepository<ProcesoIntegracion>
	{
		void EjecutarJob(ProcesoIntegracion entity);
	}
}
