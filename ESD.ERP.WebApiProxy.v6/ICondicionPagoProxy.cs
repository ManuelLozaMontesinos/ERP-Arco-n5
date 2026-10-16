using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ICondicionPagoProxy
	{
		CondicionPagoDTO GetById(CondicionPagoDTO dto);
		List<CondicionPagoDTO> Search(CondicionPagoDTO dto);
		CondicionPagoDTO SingleSearch(CondicionPagoDTO dto);
		int Insert(CondicionPagoDTO dto);
		int Update(CondicionPagoDTO dto);
		void Delete(CondicionPagoDTO dto);
	}
}
