using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IClaseProductoProxy
	{
		ClaseProductoDTO GetById(ClaseProductoDTO dto);
		List<ClaseProductoDTO> Search(ClaseProductoDTO dto);
		ClaseProductoDTO SingleSearch(ClaseProductoDTO dto);
		int Insert(ClaseProductoDTO dto);
		int Update(ClaseProductoDTO dto);
		void Delete(ClaseProductoDTO dto);
	}
}
