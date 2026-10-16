using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoAporteComisionProxy
	{
		TipoAporteComisionDTO GetById(TipoAporteComisionDTO dto);
		List<TipoAporteComisionDTO> Search(TipoAporteComisionDTO dto);
		TipoAporteComisionDTO SingleSearch(TipoAporteComisionDTO dto);
		int Insert(TipoAporteComisionDTO dto);
		int Update(TipoAporteComisionDTO dto);
		void Delete(TipoAporteComisionDTO dto);
	}
}
