using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoRemuneracionProxy
	{
		TipoRemuneracionDTO GetById(TipoRemuneracionDTO dto);
		List<TipoRemuneracionDTO> Search(TipoRemuneracionDTO dto);
		TipoRemuneracionDTO SingleSearch(TipoRemuneracionDTO dto);
		int Insert(TipoRemuneracionDTO dto);
		int Update(TipoRemuneracionDTO dto);
		void Delete(TipoRemuneracionDTO dto);
	}
}
