using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoIscBusiness
	{
		TipoIscDTO GetById(int id);
		List<TipoIscDTO> Search(TipoIscDTO dto);
		TipoIscDTO SingleSearch(TipoIscDTO dto);
		int Insert(TipoIscDTO dto);
		int Update(TipoIscDTO dto);
		void Delete(TipoIscDTO dto);
	}
}
