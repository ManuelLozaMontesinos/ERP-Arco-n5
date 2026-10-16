using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ICondicionPagoBusiness
	{
		CondicionPagoDTO GetById(int id);
		List<CondicionPagoDTO> Search(CondicionPagoDTO dto);
		CondicionPagoDTO SingleSearch(CondicionPagoDTO dto);
		int Insert(CondicionPagoDTO dto);
		int Update(CondicionPagoDTO dto);
		void Delete(CondicionPagoDTO dto);
	}
}
