using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IEstadoOrdenCompraRepository : IGenericRepository<EstadoOrdenCompra>
	{
		EstadoOrdenCompra ObtenerPorCodigo(string codigo);
	}
}
