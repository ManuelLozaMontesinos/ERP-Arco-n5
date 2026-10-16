using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IOportunidadVentaBusiness
	{
		OportunidadVentaDTO GetById(int id);
		List<OportunidadVentaDTO> Search(OportunidadVentaDTO dto);
		OportunidadVentaDTO SingleSearch(OportunidadVentaDTO dto);
		int Insert(OportunidadVentaDTO dto);
		int Update(OportunidadVentaDTO dto);
		void Delete(OportunidadVentaDTO dto);
	}
}
