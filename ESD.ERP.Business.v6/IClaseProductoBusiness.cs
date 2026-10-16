using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IClaseProductoBusiness
	{
		ClaseProductoDTO GetById(int id);
		List<ClaseProductoDTO> Search(ClaseProductoDTO dto);
		ClaseProductoDTO SingleSearch(ClaseProductoDTO dto);
		int Insert(ClaseProductoDTO dto);
		int Update(ClaseProductoDTO dto);
		void Delete(ClaseProductoDTO dto);
	}
}
