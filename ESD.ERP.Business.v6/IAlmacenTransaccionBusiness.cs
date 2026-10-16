using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IAlmacenTransaccionBusiness
	{
		AlmacenTransaccionDTO GetById(int id);
		List<AlmacenTransaccionDTO> Search(AlmacenTransaccionDTO dto);
		AlmacenTransaccionDTO SingleSearch(AlmacenTransaccionDTO dto);
		int Insert(AlmacenTransaccionDTO dto);
		int Update(AlmacenTransaccionDTO dto);
		void Delete(AlmacenTransaccionDTO dto);
	}
}
