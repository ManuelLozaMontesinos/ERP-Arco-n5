using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEPSBusiness
	{
		EPSDTO GetById(int id);
		List<EPSDTO> Search(EPSDTO dto);
		EPSDTO SingleSearch(EPSDTO dto);
		int Insert(EPSDTO dto);
		int Update(EPSDTO dto);
		void Delete(EPSDTO dto);
	}
}
