using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IUITProxy
	{
		UITDTO GetById(UITDTO dto);
		List<UITDTO> Search(UITDTO dto);
		UITDTO SingleSearch(UITDTO dto);
		int Insert(UITDTO dto);
		int Update(UITDTO dto);
		void Delete(UITDTO dto);
	}
}
