using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IAlmacenProxy
	{
		AlmacenDTO GetById(AlmacenDTO dto);
		List<AlmacenDTO> Search(AlmacenDTO dto);
		AlmacenDTO SingleSearch(AlmacenDTO dto);
		int Insert(AlmacenDTO dto);
		int Update(AlmacenDTO dto);
		void Delete(AlmacenDTO dto);
	}
}
