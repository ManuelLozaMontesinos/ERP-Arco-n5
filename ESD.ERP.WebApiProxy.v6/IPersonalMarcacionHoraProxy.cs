using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPersonalMarcacionHoraProxy
	{
		PersonalMarcacionHoraDTO GetById(PersonalMarcacionHoraDTO dto);
		List<PersonalMarcacionHoraDTO> Search(PersonalMarcacionHoraDTO dto);
		PersonalMarcacionHoraDTO SingleSearch(PersonalMarcacionHoraDTO dto);
		int Insert(PersonalMarcacionHoraDTO dto);
		int Update(PersonalMarcacionHoraDTO dto);
		void Delete(PersonalMarcacionHoraDTO dto);
		void ActualizarLote(List<PersonalMarcacionHoraDTO> list);
	}
}
