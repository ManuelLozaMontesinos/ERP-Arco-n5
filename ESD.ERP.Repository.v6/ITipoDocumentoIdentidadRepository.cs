using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoDocumentoIdentidadRepository : IGenericRepository<TipoDocumentoIdentidad>
	{
		TipoDocumentoIdentidad ObtenerPorCodigo(string codigo);
	}
}
