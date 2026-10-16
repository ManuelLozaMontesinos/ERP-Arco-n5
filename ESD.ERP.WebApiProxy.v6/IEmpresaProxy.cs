using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEmpresaProxy
	{
		EmpresaDTO GetById(EmpresaDTO dto);
		List<EmpresaDTO> Search(EmpresaDTO dto);
		EmpresaDTO SingleSearch(EmpresaDTO dto);
		int Insert(EmpresaDTO dto);
		int Update(EmpresaDTO dto);
		void Delete(EmpresaDTO dto);
	}
}
