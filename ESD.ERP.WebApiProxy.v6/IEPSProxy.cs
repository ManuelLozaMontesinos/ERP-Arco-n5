using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEPSProxy
	{
		EPSDTO GetById(EPSDTO dto);
		List<EPSDTO> Search(EPSDTO dto);
		EPSDTO SingleSearch(EPSDTO dto);
		int Insert(EPSDTO dto);
		int Update(EPSDTO dto);
		void Delete(EPSDTO dto);
	}
}
