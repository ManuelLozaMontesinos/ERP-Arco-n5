using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalMarcacionBusiness
	{
		PersonalMarcacionDTO GetById(int id);
		List<PersonalMarcacionDTO> Search(PersonalMarcacionDTO dto);
		PersonalMarcacionDTO SingleSearch(PersonalMarcacionDTO dto);
		int Insert(PersonalMarcacionDTO dto);
		int Update(PersonalMarcacionDTO dto);
		void Delete(PersonalMarcacionDTO dto);
		List<dynamic> ListarMarcacionHoraPivot(PersonalMarcacionDTO dto);
		void SincronizarMarcacion(PersonalMarcacionDTO dto);
		List<PersonalDTO> ListarPersonalConHoras(PersonalMarcacionDTO dto);
		void ActualizarHoraLote(PersonalMarcacionDTO dto);
	}
}
