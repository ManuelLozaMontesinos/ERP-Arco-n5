using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IPersonalRepository : IGenericRepository<Personal>
	{
		void ActualizarCese(Personal entity);
		void ActualizarFechaIngreso(Personal entity);
	}
}