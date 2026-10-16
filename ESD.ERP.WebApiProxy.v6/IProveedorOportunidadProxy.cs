using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IProveedorOportunidadProxy
	{
		ProveedorOportunidadDTO GetById(ProveedorOportunidadDTO dto);
		List<ProveedorOportunidadDTO> Search(ProveedorOportunidadDTO dto);
		ProveedorOportunidadDTO SingleSearch(ProveedorOportunidadDTO dto);
		int Insert(ProveedorOportunidadDTO dto);
		int Update(ProveedorOportunidadDTO dto);
		void Delete(ProveedorOportunidadDTO dto);
	}
}
