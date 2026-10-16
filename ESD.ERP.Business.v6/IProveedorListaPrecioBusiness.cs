using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProveedorListaPrecioBusiness
	{
		ProveedorListaPrecioDTO GetById(int id);
		List<ProveedorListaPrecioDTO> Search(ProveedorListaPrecioDTO dto);
		ProveedorListaPrecioDTO SingleSearch(ProveedorListaPrecioDTO dto);
		int Insert(ProveedorListaPrecioDTO dto);
		int Update(ProveedorListaPrecioDTO dto);
		void Delete(ProveedorListaPrecioDTO dto);
	}
}
