using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IGrupoProductoBusiness
	{
		GrupoProductoDTO GetById(int id);
		List<GrupoProductoDTO> Search(GrupoProductoDTO dto);
		GrupoProductoDTO SingleSearch(GrupoProductoDTO dto);
		int Insert(GrupoProductoDTO dto);
		int Update(GrupoProductoDTO dto);
		void Delete(GrupoProductoDTO dto);
	}
}
