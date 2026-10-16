using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalMarcacionHoraBusiness
	{
		PersonalMarcacionHoraDTO GetById(int id);
		List<PersonalMarcacionHoraDTO> Search(PersonalMarcacionHoraDTO dto);
		PersonalMarcacionHoraDTO SingleSearch(PersonalMarcacionHoraDTO dto);
		int Insert(PersonalMarcacionHoraDTO dto);
		int Update(PersonalMarcacionHoraDTO dto);
		void Delete(PersonalMarcacionHoraDTO dto);
		void ActualizarLote(List<PersonalMarcacionHoraDTO> list);
	}
}
