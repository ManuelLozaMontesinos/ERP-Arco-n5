using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalVacacionBusiness
	{
		PersonalVacacionDTO GetById(int id);
		List<PersonalVacacionDTO> Search(PersonalVacacionDTO dto);
		PersonalVacacionDTO SingleSearch(PersonalVacacionDTO dto);
		int Insert(PersonalVacacionDTO dto);
		int Update(PersonalVacacionDTO dto);
		void Delete(PersonalVacacionDTO dto);
	}
}
