using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPeriodoEmpresaCorrelativoBusiness
	{
		PeriodoEmpresaCorrelativoDTO GetById(int id);
		List<PeriodoEmpresaCorrelativoDTO> Search(PeriodoEmpresaCorrelativoDTO dto);
		PeriodoEmpresaCorrelativoDTO SingleSearch(PeriodoEmpresaCorrelativoDTO dto);
		int Insert(PeriodoEmpresaCorrelativoDTO dto);
		int Update(PeriodoEmpresaCorrelativoDTO dto);
		void Delete(PeriodoEmpresaCorrelativoDTO dto);
	}
}
