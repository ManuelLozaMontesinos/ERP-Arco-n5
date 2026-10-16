using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPersonalVacacionProxy
	{
		PersonalVacacionDTO GetById(PersonalVacacionDTO dto);
		List<PersonalVacacionDTO> Search(PersonalVacacionDTO dto);
		PersonalVacacionDTO SingleSearch(PersonalVacacionDTO dto);
		int Insert(PersonalVacacionDTO dto);
		int Update(PersonalVacacionDTO dto);
		void Delete(PersonalVacacionDTO dto);
	}
}
