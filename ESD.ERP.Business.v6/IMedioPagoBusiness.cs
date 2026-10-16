using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMedioPagoBusiness
	{
		MedioPagoDTO GetById(int id);
		List<MedioPagoDTO> Search(MedioPagoDTO dto);
		MedioPagoDTO SingleSearch(MedioPagoDTO dto);
		int Insert(MedioPagoDTO dto);
		int Update(MedioPagoDTO dto);
		void Delete(MedioPagoDTO dto);
	}
}
