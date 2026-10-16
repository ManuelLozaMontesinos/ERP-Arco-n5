using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEmpresaBusiness
	{
		EmpresaDTO GetById(int id);
		List<EmpresaDTO> Search(EmpresaDTO dto);
		EmpresaDTO SingleSearch(EmpresaDTO dto);
		int Insert(EmpresaDTO dto);
		int Update(EmpresaDTO dto);
		void Delete(EmpresaDTO dto);
	}
}
