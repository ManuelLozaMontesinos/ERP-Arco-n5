using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IFamiliaProductoRepository : IGenericRepository<FamiliaProducto>
	{
		void ActualizarCorrelativo(FamiliaProducto entity);
		void EliminarPorClaseProductoId(FamiliaProducto entity);
	}
}
