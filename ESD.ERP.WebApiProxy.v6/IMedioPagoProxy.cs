using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMedioPagoProxy
	{
		MedioPagoDTO GetById(MedioPagoDTO dto);
		List<MedioPagoDTO> Search(MedioPagoDTO dto);
		MedioPagoDTO SingleSearch(MedioPagoDTO dto);
		int Insert(MedioPagoDTO dto);
		int Update(MedioPagoDTO dto);
		void Delete(MedioPagoDTO dto);
	}
}
