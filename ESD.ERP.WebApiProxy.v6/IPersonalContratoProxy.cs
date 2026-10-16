using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPersonalContratoProxy
	{
		PersonalContratoDTO GetById(PersonalContratoDTO dto);
		List<PersonalContratoDTO> Search(PersonalContratoDTO dto);
		PersonalContratoDTO SingleSearch(PersonalContratoDTO dto);
		int Insert(PersonalContratoDTO dto);
		int Update(PersonalContratoDTO dto);
		void Delete(PersonalContratoDTO dto);
	}
}
