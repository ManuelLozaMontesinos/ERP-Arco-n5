using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPersonalMarcacionProxy
	{
		PersonalMarcacionDTO GetById(PersonalMarcacionDTO dto);
		List<PersonalMarcacionDTO> Search(PersonalMarcacionDTO dto);
		PersonalMarcacionDTO SingleSearch(PersonalMarcacionDTO dto);
		int Insert(PersonalMarcacionDTO dto);
		int Update(PersonalMarcacionDTO dto);
		void Delete(PersonalMarcacionDTO dto);
		string ListarMarcacionHoraGrid(PersonalMarcacionDTO dto);
		void SincronizarMarcacion(PersonalMarcacionDTO dto);
		List<PersonalDTO> ListarPersonalConHoras(PersonalMarcacionDTO dto);
		void ActualizarHoraLote(PersonalMarcacionDTO dto);
	}
}
