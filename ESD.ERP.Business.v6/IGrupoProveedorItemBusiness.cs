using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IGrupoProveedorItemBusiness
	{
		GrupoProveedorItemDTO GetById(int id);
		List<GrupoProveedorItemDTO> Search(GrupoProveedorItemDTO dto);
		GrupoProveedorItemDTO SingleSearch(GrupoProveedorItemDTO dto);
		int Insert(GrupoProveedorItemDTO dto);
		int Update(GrupoProveedorItemDTO dto);
		void Delete(GrupoProveedorItemDTO dto);
	}
}
