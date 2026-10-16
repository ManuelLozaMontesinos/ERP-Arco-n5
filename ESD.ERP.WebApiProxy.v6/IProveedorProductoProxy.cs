using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IProveedorProductoProxy
	{
		ProveedorProductoDTO GetById(ProveedorProductoDTO dto);
		List<ProveedorProductoDTO> Search(ProveedorProductoDTO dto);
		ProveedorProductoDTO SingleSearch(ProveedorProductoDTO dto);
		int Insert(ProveedorProductoDTO dto);
		int Update(ProveedorProductoDTO dto);
		void Delete(ProveedorProductoDTO dto);
	}
}
