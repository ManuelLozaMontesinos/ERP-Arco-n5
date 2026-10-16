using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPersonalRemuneracionProxy
	{
		PersonalRemuneracionDTO GetById(PersonalRemuneracionDTO dto);
		List<PersonalRemuneracionDTO> Search(PersonalRemuneracionDTO dto);
		PersonalRemuneracionDTO SingleSearch(PersonalRemuneracionDTO dto);
		int Insert(PersonalRemuneracionDTO dto);
		int Update(PersonalRemuneracionDTO dto);
		void Delete(PersonalRemuneracionDTO dto);
	}
}
