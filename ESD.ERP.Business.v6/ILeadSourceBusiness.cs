using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ILeadSourceBusiness
	{
		LeadSourceDTO GetById(int id);
		List<LeadSourceDTO> Search(LeadSourceDTO dto);
		LeadSourceDTO SingleSearch(LeadSourceDTO dto);
		int Insert(LeadSourceDTO dto);
		int Update(LeadSourceDTO dto);
		void Delete(LeadSourceDTO dto);
	}
}
