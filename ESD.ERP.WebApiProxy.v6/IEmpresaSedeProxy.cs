using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEmpresaSedeProxy
	{
		EmpresaSedeDTO GetById(EmpresaSedeDTO dto);
		List<EmpresaSedeDTO> Search(EmpresaSedeDTO dto);
		EmpresaSedeDTO SingleSearch(EmpresaSedeDTO dto);
		int Insert(EmpresaSedeDTO dto);
		int Update(EmpresaSedeDTO dto);
		void Delete(EmpresaSedeDTO dto);
	}
}
