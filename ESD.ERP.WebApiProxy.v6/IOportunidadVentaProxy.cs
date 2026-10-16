using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IOportunidadVentaProxy
	{
		OportunidadVentaDTO GetById(OportunidadVentaDTO dto);
		List<OportunidadVentaDTO> Search(OportunidadVentaDTO dto);
		OportunidadVentaDTO SingleSearch(OportunidadVentaDTO dto);
		int Insert(OportunidadVentaDTO dto);
		int Update(OportunidadVentaDTO dto);
		void Delete(OportunidadVentaDTO dto);
	}
}
