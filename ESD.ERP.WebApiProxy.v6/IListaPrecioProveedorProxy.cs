using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IListaPrecioProveedorProxy
	{
		ListaPrecioProveedorDTO GetById(ListaPrecioProveedorDTO dto);
		List<ListaPrecioProveedorDTO> Search(ListaPrecioProveedorDTO dto);
		ListaPrecioProveedorDTO SingleSearch(ListaPrecioProveedorDTO dto);
		int Insert(ListaPrecioProveedorDTO dto);
		int Update(ListaPrecioProveedorDTO dto);
		void Delete(ListaPrecioProveedorDTO dto);
		List<ListaPrecioProveedorDTO> ListarPorProveedorId(ListaPrecioProveedorDTO dto);
	}
}
