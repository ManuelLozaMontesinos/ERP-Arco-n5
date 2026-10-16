using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IConvenioDobleTributacionRepository : IGenericRepository<ConvenioDobleTributacion>
	{
		ConvenioDobleTributacion ObtenerPorCodigo(string codigo);
	}
}
