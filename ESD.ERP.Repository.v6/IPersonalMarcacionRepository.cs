using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IPersonalMarcacionRepository : IGenericRepository<PersonalMarcacion>
	{
		List<dynamic> ListarMarcacionHoraPivot(PersonalMarcacion entity);
		void SincronizarMarcacion(PersonalMarcacion entity);
		List<Personal> ListarPersonalConHoras(PersonalMarcacion entity);
	}
}
