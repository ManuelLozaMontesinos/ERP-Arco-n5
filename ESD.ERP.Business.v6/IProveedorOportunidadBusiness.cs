using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProveedorOportunidadBusiness
	{
		ProveedorOportunidadDTO GetById(int id);
		List<ProveedorOportunidadDTO> Search(ProveedorOportunidadDTO dto);
		ProveedorOportunidadDTO SingleSearch(ProveedorOportunidadDTO dto);
		int Insert(ProveedorOportunidadDTO dto);
		int Update(ProveedorOportunidadDTO dto);
		void Delete(ProveedorOportunidadDTO dto);
	}
}
