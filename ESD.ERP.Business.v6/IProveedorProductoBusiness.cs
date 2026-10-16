using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProveedorProductoBusiness
	{
		ProveedorProductoDTO GetById(int id);
		List<ProveedorProductoDTO> Search(ProveedorProductoDTO dto);
		ProveedorProductoDTO SingleSearch(ProveedorProductoDTO dto);
		int Insert(ProveedorProductoDTO dto);
		int Update(ProveedorProductoDTO dto);
		void Delete(ProveedorProductoDTO dto);
	}
}
