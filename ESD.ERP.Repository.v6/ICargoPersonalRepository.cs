using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface ICargoPersonalRepository : IGenericRepository<CargoPersonal>
	{
		CargoPersonal ObtenerPorCodigo(string codigo);
	}
}
