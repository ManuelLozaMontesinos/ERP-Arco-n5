using ESD.Core.Data;
using ESD.ERP.Domain;

namespace ESD.ERP.Repository
{
	public interface IProveedorRepository : IGenericRepository<Proveedor>
	{
		void ActualizarUsuario(Proveedor entity);
		void ActualizarTipoProveedor(Proveedor entity);
	}
}
