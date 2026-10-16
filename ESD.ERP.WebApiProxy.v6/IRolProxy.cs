using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRolProxy
	{
		RolDTO GetById(RolDTO dto);
		List<RolDTO> Search(RolDTO dto);
		RolDTO SingleSearch(RolDTO dto);
		int Insert(RolDTO dto);
		int Update(RolDTO dto);
		void Delete(RolDTO dto);
	}
}
