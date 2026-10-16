using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoIscProxy
	{
		TipoIscDTO GetById(TipoIscDTO dto);
		List<TipoIscDTO> Search(TipoIscDTO dto);
		TipoIscDTO SingleSearch(TipoIscDTO dto);
		int Insert(TipoIscDTO dto);
		int Update(TipoIscDTO dto);
		void Delete(TipoIscDTO dto);
	}
}
