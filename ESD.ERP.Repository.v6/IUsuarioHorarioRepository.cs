using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IUsuarioHorarioRepository : IGenericRepository<UsuarioHorario>
	{
		List<UsuarioHorario> ListarPorUsuarioId(UsuarioHorario entity);
      void ActualizarLote(List<UsuarioHorario> usuarioHorarioList);
   }
}
