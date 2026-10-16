using ESD.Core.Data;
using ESD.ERP.Domain;
using System.Collections.Generic;

namespace ESD.ERP.Repository
{
	public interface IListaPrecioProveedorRepository : IGenericRepository<ListaPrecioProveedor>
	{
		List<ListaPrecioProveedor> ListarPorProveedorId(ListaPrecioProveedor entity);
	}
}
