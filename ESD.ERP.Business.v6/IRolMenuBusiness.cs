using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRolMenuBusiness
	{
		RolMenuDTO GetById(int id);
		List<RolMenuDTO> Search(RolMenuDTO dto);
		RolMenuDTO SingleSearch(RolMenuDTO dto);
		int Insert(RolMenuDTO dto);
		int Update(RolMenuDTO dto);
		void Delete(RolMenuDTO dto);
		List<RolMenuDTO> ListarPorRolId(RolMenuDTO dto);
	}
}
