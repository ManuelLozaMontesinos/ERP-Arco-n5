using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalMaquinaBusiness
	{
		PersonalMaquinaDTO GetById(int id);
		List<PersonalMaquinaDTO> Search(PersonalMaquinaDTO dto);
        PersonalMaquinaDTO SingleSearch(PersonalMaquinaDTO dto);
		int Insert(PersonalMaquinaDTO dto);
		int Update(PersonalMaquinaDTO dto);
		void Delete(PersonalMaquinaDTO dto);
    }
}
