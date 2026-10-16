using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IVariableCalculoProxy
	{
		VariableCalculoDTO GetById(VariableCalculoDTO dto);
		List<VariableCalculoDTO> Search(VariableCalculoDTO dto);
		VariableCalculoDTO SingleSearch(VariableCalculoDTO dto);
		int Insert(VariableCalculoDTO dto);
		int Update(VariableCalculoDTO dto);
		void Delete(VariableCalculoDTO dto);
	}
}
