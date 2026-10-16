using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IProveedorListaPrecioProxy
	{
		ProveedorListaPrecioDTO GetById(ProveedorListaPrecioDTO dto);
		List<ProveedorListaPrecioDTO> Search(ProveedorListaPrecioDTO dto);
		ProveedorListaPrecioDTO SingleSearch(ProveedorListaPrecioDTO dto);
		int Insert(ProveedorListaPrecioDTO dto);
		int Update(ProveedorListaPrecioDTO dto);
		void Delete(ProveedorListaPrecioDTO dto);
	}
}
