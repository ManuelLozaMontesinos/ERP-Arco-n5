using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalRemuneracionBusiness
	{
		PersonalRemuneracionDTO GetById(int id);
		List<PersonalRemuneracionDTO> Search(PersonalRemuneracionDTO dto);
		PersonalRemuneracionDTO SingleSearch(PersonalRemuneracionDTO dto);
		int Insert(PersonalRemuneracionDTO dto);
		int Update(PersonalRemuneracionDTO dto);
		void Delete(PersonalRemuneracionDTO dto);
	}
}
