using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IDepartamentoBusiness
	{
		DepartamentoDTO GetById(int id);
		List<DepartamentoDTO> Search(DepartamentoDTO dto);
		DepartamentoDTO SingleSearch(DepartamentoDTO dto);
		int Insert(DepartamentoDTO dto);
		int Update(DepartamentoDTO dto);
		void Delete(DepartamentoDTO dto);
	}
}
