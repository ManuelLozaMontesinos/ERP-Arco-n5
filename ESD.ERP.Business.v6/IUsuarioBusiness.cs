using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IUsuarioBusiness
	{
		UsuarioDTO GetById(int id);
		List<UsuarioDTO> Search(UsuarioDTO dto);
		UsuarioDTO SingleSearch(UsuarioDTO dto);
		int Insert(UsuarioDTO dto);
		int Update(UsuarioDTO dto);
		void Delete(UsuarioDTO dto);
		UsuarioDTO ObtenerPorCodigo(string codigo);
		void ActualizarEstado(UsuarioDTO dto);
		void ActualizarContrasena(UsuarioDTO dto);
		void ActualizarContrasenaAnterior(UsuarioDTO dto);
		bool ValidarContrasena(UsuarioDTO dto);
	}
}
