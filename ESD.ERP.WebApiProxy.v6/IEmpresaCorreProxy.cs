using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEmpresaCorreProxy
	{
		EmpresaCorrelativoDTO GetById(EmpresaCorrelativoDTO dto);
		List<EmpresaCorrelativoDTO> Search(EmpresaCorrelativoDTO dto);
		EmpresaCorrelativoDTO SingleSearch(EmpresaCorrelativoDTO dto);
		int Insert(EmpresaCorrelativoDTO dto);
		int Update(EmpresaCorrelativoDTO dto);
		void Delete(EmpresaCorrelativoDTO dto);
	}
}
