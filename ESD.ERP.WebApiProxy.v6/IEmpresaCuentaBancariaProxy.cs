using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEmpresaCuentaBancariaProxy
	{
		EmpresaCuentaBancariaDTO GetById(EmpresaCuentaBancariaDTO dto);
		List<EmpresaCuentaBancariaDTO> Search(EmpresaCuentaBancariaDTO dto);
		EmpresaCuentaBancariaDTO SingleSearch(EmpresaCuentaBancariaDTO dto);
		int Insert(EmpresaCuentaBancariaDTO dto);
		int Update(EmpresaCuentaBancariaDTO dto);
		void Delete(EmpresaCuentaBancariaDTO dto);
	}
}
