using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPeriodoBusiness
	{
		PeriodoDTO GetById(int id);
		List<PeriodoDTO> Search(PeriodoDTO dto);
		PeriodoDTO SingleSearch(PeriodoDTO dto);
		int Insert(PeriodoDTO dto);
		int Update(PeriodoDTO dto);
		void Delete(PeriodoDTO dto);
	}
}
