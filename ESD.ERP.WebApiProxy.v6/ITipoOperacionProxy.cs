using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoOperacionProxy
	{
		TipoOperacionDTO GetById(TipoOperacionDTO dto);
		List<TipoOperacionDTO> Search(TipoOperacionDTO dto);
		TipoOperacionDTO SingleSearch(TipoOperacionDTO dto);
		int Insert(TipoOperacionDTO dto);
		int Update(TipoOperacionDTO dto);
		void Delete(TipoOperacionDTO dto);
	}
}
