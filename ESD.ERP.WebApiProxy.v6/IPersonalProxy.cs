using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPersonalProxy
	{
		PersonalDTO GetById(PersonalDTO dto);
		List<PersonalDTO> Search(PersonalDTO dto);
		PersonalDTO SingleSearch(PersonalDTO dto);
		int Insert(PersonalDTO dto);
		int Update(PersonalDTO dto);
		void Delete(PersonalDTO dto);
	}
}
