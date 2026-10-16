using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoOperacionBusiness
	{
		TipoOperacionDTO GetById(int id);
		List<TipoOperacionDTO> Search(TipoOperacionDTO dto);
		TipoOperacionDTO SingleSearch(TipoOperacionDTO dto);
		int Insert(TipoOperacionDTO dto);
		int Update(TipoOperacionDTO dto);
		void Delete(TipoOperacionDTO dto);
	}
}
