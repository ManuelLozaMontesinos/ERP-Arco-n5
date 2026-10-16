using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IAlmacenBusiness
	{
		AlmacenDTO GetById(int id);
		List<AlmacenDTO> Search(AlmacenDTO dto);
		AlmacenDTO SingleSearch(AlmacenDTO dto);
		int Insert(AlmacenDTO dto);
		int Update(AlmacenDTO dto);
		void Delete(AlmacenDTO dto);
	}
}
