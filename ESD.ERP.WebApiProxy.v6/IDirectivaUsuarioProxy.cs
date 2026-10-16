using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IDirectivaUsuarioProxy
	{
		DirectivaUsuarioDTO GetById(DirectivaUsuarioDTO dto);
		List<DirectivaUsuarioDTO> Search(DirectivaUsuarioDTO dto);
		DirectivaUsuarioDTO SingleSearch(DirectivaUsuarioDTO dto);
		int Insert(DirectivaUsuarioDTO dto);
		int Update(DirectivaUsuarioDTO dto);
		void Delete(DirectivaUsuarioDTO dto);
	}
}
