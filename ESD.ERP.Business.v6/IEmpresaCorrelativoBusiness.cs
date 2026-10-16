using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEmpresaCorrelativoBusiness
	{
		EmpresaCorrelativoDTO GetById(int id);
		List<EmpresaCorrelativoDTO> Search(EmpresaCorrelativoDTO dto);
		EmpresaCorrelativoDTO SingleSearch(EmpresaCorrelativoDTO dto);
		int Insert(EmpresaCorrelativoDTO dto);
		int Update(EmpresaCorrelativoDTO dto);
		void Delete(EmpresaCorrelativoDTO dto);

	}
}
