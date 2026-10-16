using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ITipoDocumentoComercialRepository : IGenericRepository<TipoDocumentoComercial>
	{
		TipoDocumentoComercial ObtenerPorCodigo(string codigo);
	}
}
