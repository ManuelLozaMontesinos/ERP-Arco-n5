using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoAutorizacionBusiness
	{
		TipoAutorizacionDTO GetById(int id);
		List<TipoAutorizacionDTO> Search(TipoAutorizacionDTO dto);
		TipoAutorizacionDTO SingleSearch(TipoAutorizacionDTO dto);
		int Insert(TipoAutorizacionDTO dto);
		int Update(TipoAutorizacionDTO dto);
		void Delete(TipoAutorizacionDTO dto);
	}
}
