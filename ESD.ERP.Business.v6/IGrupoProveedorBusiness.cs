using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IGrupoProveedorBusiness
	{
		GrupoProveedorDTO GetById(int id);
		List<GrupoProveedorDTO> Search(GrupoProveedorDTO dto);
		GrupoProveedorDTO SingleSearch(GrupoProveedorDTO dto);
		int Insert(GrupoProveedorDTO dto);
		int Update(GrupoProveedorDTO dto);
		void Delete(GrupoProveedorDTO dto);
	}
}
