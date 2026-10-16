using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEmpresaSedeBusiness
	{
		EmpresaSedeDTO GetById(int id);
		List<EmpresaSedeDTO> Search(EmpresaSedeDTO dto);
		EmpresaSedeDTO SingleSearch(EmpresaSedeDTO dto);
		int Insert(EmpresaSedeDTO dto);
		int Update(EmpresaSedeDTO dto);
		void Delete(EmpresaSedeDTO dto);
	}
}
