using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITransaccionAlmacenProxy
	{
		TransaccionAlmacenDTO GetById(TransaccionAlmacenDTO dto);
		List<TransaccionAlmacenDTO> Search(TransaccionAlmacenDTO dto);
		TransaccionAlmacenDTO SingleSearch(TransaccionAlmacenDTO dto);
		int Insert(TransaccionAlmacenDTO dto);
		int Update(TransaccionAlmacenDTO dto);
		void Delete(TransaccionAlmacenDTO dto);
	}
}
