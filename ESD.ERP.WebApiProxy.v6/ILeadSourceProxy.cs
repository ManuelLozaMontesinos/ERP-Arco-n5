using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ILeadSourceProxy
	{
		LeadSourceDTO GetById(LeadSourceDTO dto);
		List<LeadSourceDTO> Search(LeadSourceDTO dto);
		LeadSourceDTO SingleSearch(LeadSourceDTO dto);
		int Insert(LeadSourceDTO dto);
		int Update(LeadSourceDTO dto);
		void Delete(LeadSourceDTO dto);
	}
}
