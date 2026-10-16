using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IPersonalMarcacionHoraRepository : IGenericRepository<PersonalMarcacionHora>
	{
		void ActualizarLote(List<PersonalMarcacionHora> list);
	}
}
