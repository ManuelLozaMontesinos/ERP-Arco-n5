using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRolBusiness
	{
		RolDTO GetById(int id);
		List<RolDTO> Search(RolDTO dto);
		RolDTO SingleSearch(RolDTO dto);
		int Insert(RolDTO dto);
		int Update(RolDTO dto);
		void Delete(RolDTO dto);
	}
}
