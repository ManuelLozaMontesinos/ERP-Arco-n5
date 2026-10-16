using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IUITBusiness
	{
		UITDTO GetById(int id);
		List<UITDTO> Search(UITDTO dto);
		UITDTO SingleSearch(UITDTO dto);
		int Insert(UITDTO dto);
		int Update(UITDTO dto);
		void Delete(UITDTO dto);
	}
}
