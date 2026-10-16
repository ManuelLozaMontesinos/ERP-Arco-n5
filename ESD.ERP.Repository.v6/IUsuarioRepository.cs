using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IUsuarioRepository : IGenericRepository<Usuario>
	{
		Usuario ObtenerPorCodigo(string codigo);
		void ActualizarEstado(Usuario entity);
		void ActualizarContrasena(Usuario entity);
	}
}
