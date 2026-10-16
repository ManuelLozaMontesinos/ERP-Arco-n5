using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITransaccionAlmacenBusiness
	{
		TransaccionAlmacenDTO GetById(int id);
		List<TransaccionAlmacenDTO> Search(TransaccionAlmacenDTO dto);
		TransaccionAlmacenDTO SingleSearch(TransaccionAlmacenDTO dto);
		int Insert(TransaccionAlmacenDTO dto);
		int Update(TransaccionAlmacenDTO dto);
		void Delete(TransaccionAlmacenDTO dto);
	}
}
