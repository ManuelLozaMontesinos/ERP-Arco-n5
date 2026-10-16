using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IGrupoProductoProxy
	{
		GrupoProductoDTO GetById(GrupoProductoDTO dto);
		List<GrupoProductoDTO> Search(GrupoProductoDTO dto);
		GrupoProductoDTO SingleSearch(GrupoProductoDTO dto);
		int Insert(GrupoProductoDTO dto);
		int Update(GrupoProductoDTO dto);
		void Delete(GrupoProductoDTO dto);
	}
}
