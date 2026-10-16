using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoProveedorBusiness
	{
		TipoProveedorDTO GetById(int id);
		List<TipoProveedorDTO> Search(TipoProveedorDTO dto);
		TipoProveedorDTO SingleSearch(TipoProveedorDTO dto);
		int Insert(TipoProveedorDTO dto);
		int Update(TipoProveedorDTO dto);
		void Delete(TipoProveedorDTO dto);
		TipoProveedorDTO ObtenerPorCodigo(string codigo);
	}
}
