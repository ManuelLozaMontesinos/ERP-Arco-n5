using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalPlanillaBusiness
	{
		PersonalPlanillaDTO GetById(int id);
		List<PersonalPlanillaDTO> Search(PersonalPlanillaDTO dto);
		PersonalPlanillaDTO SingleSearch(PersonalPlanillaDTO dto);
		int Insert(PersonalPlanillaDTO dto);
		int Update(PersonalPlanillaDTO dto);
		void Delete(PersonalPlanillaDTO dto);
	}
}
