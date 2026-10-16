using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRolMenuProxy
	{
		RolMenuDTO GetById(RolMenuDTO dto);
		List<RolMenuDTO> Search(RolMenuDTO dto);
		RolMenuDTO SingleSearch(RolMenuDTO dto);
		int Insert(RolMenuDTO dto);
		int Update(RolMenuDTO dto);
		void Delete(RolMenuDTO dto);
		List<RolMenuDTO> ListarPorRolId(RolMenuDTO dto);
	}
}
