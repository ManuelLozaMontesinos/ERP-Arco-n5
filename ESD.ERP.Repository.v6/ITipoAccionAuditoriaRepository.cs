using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoAccionAuditoriaRepository : IGenericRepository<TipoAccionAuditoria>
	{
		TipoAccionAuditoria ObtenerPorCodigo(string codigo);
	}
}