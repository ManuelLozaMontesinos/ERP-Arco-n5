using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoProveedorProxy
	{
		TipoProveedorDTO GetById(TipoProveedorDTO dto);
		List<TipoProveedorDTO> Search(TipoProveedorDTO dto);
		TipoProveedorDTO SingleSearch(TipoProveedorDTO dto);
		int Insert(TipoProveedorDTO dto);
		int Update(TipoProveedorDTO dto);
		void Delete(TipoProveedorDTO dto);
		TipoProveedorDTO ObtenerPorCodigo(TipoProveedorDTO dto);
	}
}
