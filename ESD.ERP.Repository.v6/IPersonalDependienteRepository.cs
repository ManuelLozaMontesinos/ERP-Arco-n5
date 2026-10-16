using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IPersonalDependienteRepository : IGenericRepository<PersonalDependiente>
	{
		void ActualizarBaja(PersonalDependiente entity);
	}
}
