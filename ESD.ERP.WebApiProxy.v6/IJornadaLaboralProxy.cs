using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IJornadaLaboralProxy
	{
		JornadaLaboralDTO GetById(JornadaLaboralDTO dto);
		List<JornadaLaboralDTO> Search(JornadaLaboralDTO dto);
		JornadaLaboralDTO SingleSearch(JornadaLaboralDTO dto);
		int Insert(JornadaLaboralDTO dto);
		int Update(JornadaLaboralDTO dto);
		void Delete(JornadaLaboralDTO dto);
	}
}
