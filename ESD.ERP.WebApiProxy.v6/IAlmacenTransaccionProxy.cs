using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IAlmacenTransaccionProxy
	{
		AlmacenTransaccionDTO GetById(AlmacenTransaccionDTO dto);
		List<AlmacenTransaccionDTO> Search(AlmacenTransaccionDTO dto);
		AlmacenTransaccionDTO SingleSearch(AlmacenTransaccionDTO dto);
		int Insert(AlmacenTransaccionDTO dto);
		int Update(AlmacenTransaccionDTO dto);
		void Delete(AlmacenTransaccionDTO dto);
	}
}
