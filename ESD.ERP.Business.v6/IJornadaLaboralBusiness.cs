using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IJornadaLaboralBusiness
	{
		JornadaLaboralDTO GetById(int id);
		List<JornadaLaboralDTO> Search(JornadaLaboralDTO dto);
		JornadaLaboralDTO SingleSearch(JornadaLaboralDTO dto);
		int Insert(JornadaLaboralDTO dto);
		int Update(JornadaLaboralDTO dto);
		void Delete(JornadaLaboralDTO dto);
	}
}
