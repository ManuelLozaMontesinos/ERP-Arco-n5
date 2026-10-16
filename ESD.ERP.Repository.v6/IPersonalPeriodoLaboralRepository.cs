using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IPersonalPeriodoLaboralRepository : IGenericRepository<PersonalPeriodoLaboral>
	{
		void ActualizarCese(PersonalPeriodoLaboral entity);
	}
}
