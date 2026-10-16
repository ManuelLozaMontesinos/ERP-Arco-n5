using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPeriodoEmpresaSedeProxy
	{
		PeriodoEmpresaSedeDTO GetById(PeriodoEmpresaSedeDTO dto);
		List<PeriodoEmpresaSedeDTO> Search(PeriodoEmpresaSedeDTO dto);
		PeriodoEmpresaSedeDTO SingleSearch(PeriodoEmpresaSedeDTO dto);
		int Insert(PeriodoEmpresaSedeDTO dto);
		int Update(PeriodoEmpresaSedeDTO dto);
		void Delete(PeriodoEmpresaSedeDTO dto);
		List<EmpresaDTO> ObtenerEmpresaList(PeriodoEmpresaSedeDTO dto);
	}
}
