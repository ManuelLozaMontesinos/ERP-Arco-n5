using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalBusiness
	{
		PersonalDTO GetById(int id);
		List<PersonalDTO> Search(PersonalDTO dto);
		PersonalDTO SingleSearch(PersonalDTO dto);
		int Insert(PersonalDTO dto);
		int Update(PersonalDTO dto);
		void Delete(PersonalDTO dto);
	}
}
