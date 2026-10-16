using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProveedorBusiness
	{
		ProveedorDTO GetById(int id);
		List<ProveedorDTO> Search(ProveedorDTO dto);
		ProveedorDTO SingleSearch(ProveedorDTO dto);
		int Insert(ProveedorDTO dto);
		int Update(ProveedorDTO dto);
		void Delete(ProveedorDTO dto);
		void ActualizarTipoProveedor(ProveedorDTO dto);
	}
}
