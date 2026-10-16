using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IGrupoProveedorProxy
	{
		GrupoProveedorDTO GetById(GrupoProveedorDTO dto);
		List<GrupoProveedorDTO> Search(GrupoProveedorDTO dto);
		GrupoProveedorDTO SingleSearch(GrupoProveedorDTO dto);
		int Insert(GrupoProveedorDTO dto);
		int Update(GrupoProveedorDTO dto);
		void Delete(GrupoProveedorDTO dto);
	}
}
