using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRolUsuarioBusiness
	{
		RolUsuarioDTO GetById(int id);
		List<RolUsuarioDTO> Search(RolUsuarioDTO dto);
		RolUsuarioDTO SingleSearch(RolUsuarioDTO dto);
		int Insert(RolUsuarioDTO dto);
		int Update(RolUsuarioDTO dto);
		void Delete(RolUsuarioDTO dto);
	}
}
