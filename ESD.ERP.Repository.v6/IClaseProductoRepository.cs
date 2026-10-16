using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IClaseProductoRepository : IGenericRepository<ClaseProducto>
	{
		void ActualizarCorrelativo(ClaseProducto entity);
		void EliminarPorGrupoProductoId(ClaseProducto entity);
	}
}
