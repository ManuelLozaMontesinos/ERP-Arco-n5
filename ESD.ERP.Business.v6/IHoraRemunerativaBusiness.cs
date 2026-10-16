using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IHoraRemunerativaBusiness
	{
		HoraRemunerativaDTO GetById(int id);
		List<HoraRemunerativaDTO> Search(HoraRemunerativaDTO dto);
		HoraRemunerativaDTO SingleSearch(HoraRemunerativaDTO dto);
		int Insert(HoraRemunerativaDTO dto);
		int Update(HoraRemunerativaDTO dto);
		void Delete(HoraRemunerativaDTO dto);
	}
}
