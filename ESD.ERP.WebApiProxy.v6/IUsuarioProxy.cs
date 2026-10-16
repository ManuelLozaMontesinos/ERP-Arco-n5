using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IUsuarioProxy
	{
		UsuarioDTO GetById(UsuarioDTO dto);
		List<UsuarioDTO> Search(UsuarioDTO dto);
		UsuarioDTO SingleSearch(UsuarioDTO dto);
		int Insert(UsuarioDTO dto);
		int Update(UsuarioDTO dto);
		void Delete(UsuarioDTO dto);
		UsuarioDTO ObtenerPorCodigo(UsuarioDTO dto);
		void ActualizarEstado(UsuarioDTO dto);
		void ActualizarContrasena(UsuarioDTO dto);
		void ActualizarContrasenaAnterior(UsuarioDTO dto);
	}
}
