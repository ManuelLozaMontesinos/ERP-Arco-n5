using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEmpresaCuentaBancariaBusiness
	{
		EmpresaCuentaBancariaDTO GetById(int id);
		List<EmpresaCuentaBancariaDTO> Search(EmpresaCuentaBancariaDTO dto);
		EmpresaCuentaBancariaDTO SingleSearch(EmpresaCuentaBancariaDTO dto);
		int Insert(EmpresaCuentaBancariaDTO dto);
		int Update(EmpresaCuentaBancariaDTO dto);
		void Delete(EmpresaCuentaBancariaDTO dto);
	}
}
