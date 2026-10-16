using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalContratoBusiness
	{
		PersonalContratoDTO GetById(int id);
		List<PersonalContratoDTO> Search(PersonalContratoDTO dto);
		PersonalContratoDTO SingleSearch(PersonalContratoDTO dto);
		int Insert(PersonalContratoDTO dto);
		int Update(PersonalContratoDTO dto);
		void Delete(PersonalContratoDTO dto);
	}
}
