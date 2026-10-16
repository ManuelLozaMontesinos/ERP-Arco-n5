using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IProductoRepository : IGenericRepository<Producto>
	{
		void EliminarPorFamiliaProductoId(Producto entity);
		void ActualizarTienePresentacionVenta(Producto entity);
		void ActualizarPesoMasterPack(Producto entity);
		void ActualizarEstado(Producto entity);
	}
}