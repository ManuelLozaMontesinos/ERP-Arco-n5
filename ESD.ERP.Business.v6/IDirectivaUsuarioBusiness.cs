using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IDirectivaUsuarioBusiness
	{
		DirectivaUsuarioDTO GetById(int id);
		List<DirectivaUsuarioDTO> Search(DirectivaUsuarioDTO dto);
		DirectivaUsuarioDTO SingleSearch(DirectivaUsuarioDTO dto);
		int Insert(DirectivaUsuarioDTO dto);
		int Update(DirectivaUsuarioDTO dto);
		void Delete(DirectivaUsuarioDTO dto);
	}
}
