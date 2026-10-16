using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IUsuarioHorarioProxy
	{
		UsuarioHorarioDTO GetById(UsuarioHorarioDTO dto);
		List<UsuarioHorarioDTO> Search(UsuarioHorarioDTO dto);
		UsuarioHorarioDTO SingleSearch(UsuarioHorarioDTO dto);
		int Insert(UsuarioHorarioDTO dto);
		int Update(UsuarioHorarioDTO dto);
		void Delete(UsuarioHorarioDTO dto);
		List<UsuarioHorarioDTO> ListarPorUsuarioId(UsuarioHorarioDTO dto);
		void ActualizarLote(List<UsuarioHorarioDTO> list);
	}
}
