using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IVariableCalculoBusiness
	{
		VariableCalculoDTO GetById(int id);
		List<VariableCalculoDTO> Search(VariableCalculoDTO dto);
		VariableCalculoDTO SingleSearch(VariableCalculoDTO dto);
		int Insert(VariableCalculoDTO dto);
		int Update(VariableCalculoDTO dto);
		void Delete(VariableCalculoDTO dto);
	}
}
