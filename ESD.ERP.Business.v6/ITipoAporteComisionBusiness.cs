using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoAporteComisionBusiness
	{
		TipoAporteComisionDTO GetById(int id);
		List<TipoAporteComisionDTO> Search(TipoAporteComisionDTO dto);
		TipoAporteComisionDTO SingleSearch(TipoAporteComisionDTO dto);
		int Insert(TipoAporteComisionDTO dto);
		int Update(TipoAporteComisionDTO dto);
		void Delete(TipoAporteComisionDTO dto);
	}
}
