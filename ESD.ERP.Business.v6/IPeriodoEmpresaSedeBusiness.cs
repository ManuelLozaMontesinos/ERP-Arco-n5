using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPeriodoEmpresaSedeBusiness
	{
		PeriodoEmpresaSedeDTO GetById(int id);
		List<PeriodoEmpresaSedeDTO> Search(PeriodoEmpresaSedeDTO dto);
		PeriodoEmpresaSedeDTO SingleSearch(PeriodoEmpresaSedeDTO dto);
		int Insert(PeriodoEmpresaSedeDTO dto);
		int Update(PeriodoEmpresaSedeDTO dto);
		void Delete(PeriodoEmpresaSedeDTO dto);
		List<EmpresaDTO> ObtenerEmpresaList(PeriodoEmpresaSedeDTO dto);
	}
}
