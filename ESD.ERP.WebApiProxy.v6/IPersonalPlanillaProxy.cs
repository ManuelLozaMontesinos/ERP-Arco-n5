using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPersonalPlanillaProxy
	{
		PersonalPlanillaDTO GetById(PersonalPlanillaDTO dto);
		List<PersonalPlanillaDTO> Search(PersonalPlanillaDTO dto);
		PersonalPlanillaDTO SingleSearch(PersonalPlanillaDTO dto);
		int Insert(PersonalPlanillaDTO dto);
		int Update(PersonalPlanillaDTO dto);
		void Delete(PersonalPlanillaDTO dto);
	}
}
