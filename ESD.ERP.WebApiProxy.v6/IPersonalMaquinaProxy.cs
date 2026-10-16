using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPersonalMaquinaProxy
	{
		PersonalMaquinaDTO GetById(PersonalMaquinaDTO dto);
		List<PersonalMaquinaDTO> Search(PersonalMaquinaDTO dto);
		PersonalMaquinaDTO SingleSearch(PersonalMaquinaDTO dto);
		int Insert(PersonalMaquinaDTO dto);
		int Update(PersonalMaquinaDTO dto);
		void Delete(PersonalMaquinaDTO dto);
    }
}
