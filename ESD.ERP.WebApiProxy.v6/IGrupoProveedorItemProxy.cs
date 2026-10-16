using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IGrupoProveedorItemProxy
	{
		GrupoProveedorItemDTO GetById(GrupoProveedorItemDTO dto);
		List<GrupoProveedorItemDTO> Search(GrupoProveedorItemDTO dto);
		GrupoProveedorItemDTO SingleSearch(GrupoProveedorItemDTO dto);
		int Insert(GrupoProveedorItemDTO dto);
		int Update(GrupoProveedorItemDTO dto);
		void Delete(GrupoProveedorItemDTO dto);
	}
}
