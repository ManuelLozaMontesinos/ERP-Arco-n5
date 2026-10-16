using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoAutorizacionProxy
	{
		TipoAutorizacionDTO GetById(TipoAutorizacionDTO dto);
		List<TipoAutorizacionDTO> Search(TipoAutorizacionDTO dto);
		TipoAutorizacionDTO SingleSearch(TipoAutorizacionDTO dto);
		int Insert(TipoAutorizacionDTO dto);
		int Update(TipoAutorizacionDTO dto);
		void Delete(TipoAutorizacionDTO dto);
	}
}
