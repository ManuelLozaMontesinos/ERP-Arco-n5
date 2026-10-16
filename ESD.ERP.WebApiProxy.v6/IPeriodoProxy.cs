using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPeriodoProxy
	{
		PeriodoDTO GetById(PeriodoDTO dto);
		List<PeriodoDTO> Search(PeriodoDTO dto);
		PeriodoDTO SingleSearch(PeriodoDTO dto);
		int Insert(PeriodoDTO dto);
		int Update(PeriodoDTO dto);
		void Delete(PeriodoDTO dto);
	}
}
