using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IDepartamentoProxy
	{
		DepartamentoDTO GetById(DepartamentoDTO dto);
		List<DepartamentoDTO> Search(DepartamentoDTO dto);
		DepartamentoDTO SingleSearch(DepartamentoDTO dto);
		int Insert(DepartamentoDTO dto);
		int Update(DepartamentoDTO dto);
		void Delete(DepartamentoDTO dto);
	}
}
