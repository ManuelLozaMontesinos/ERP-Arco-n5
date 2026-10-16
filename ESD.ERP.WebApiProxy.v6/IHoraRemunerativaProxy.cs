using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IHoraRemunerativaProxy
	{
		HoraRemunerativaDTO GetById(HoraRemunerativaDTO dto);
		List<HoraRemunerativaDTO> Search(HoraRemunerativaDTO dto);
		HoraRemunerativaDTO SingleSearch(HoraRemunerativaDTO dto);
		int Insert(HoraRemunerativaDTO dto);
		int Update(HoraRemunerativaDTO dto);
		void Delete(HoraRemunerativaDTO dto);
	}
}
