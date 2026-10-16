using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRolUsuarioProxy
	{
		RolUsuarioDTO GetById(RolUsuarioDTO dto);
		List<RolUsuarioDTO> Search(RolUsuarioDTO dto);
		RolUsuarioDTO SingleSearch(RolUsuarioDTO dto);
		int Insert(RolUsuarioDTO dto);
		int Update(RolUsuarioDTO dto);
		void Delete(RolUsuarioDTO dto);
	}
}
