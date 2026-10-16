using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IOcupacionProxy
	{
		OcupacionDTO GetById(OcupacionDTO dto);
		List<OcupacionDTO> Search(OcupacionDTO dto);
		OcupacionDTO SingleSearch(OcupacionDTO dto);
		int Insert(OcupacionDTO dto);
		int Update(OcupacionDTO dto);
		void Delete(OcupacionDTO dto);
		OcupacionDTO ObtenerPorCodigo(OcupacionDTO dto);
	}
}
