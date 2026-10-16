using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalPeriodoLaboralBusiness
	{
		PersonalPeriodoLaboralDTO GetById(int id);
		List<PersonalPeriodoLaboralDTO> Search(PersonalPeriodoLaboralDTO dto);
		PersonalPeriodoLaboralDTO SingleSearch(PersonalPeriodoLaboralDTO dto);
		int Insert(PersonalPeriodoLaboralDTO dto);
		int Update(PersonalPeriodoLaboralDTO dto);
		void Delete(PersonalPeriodoLaboralDTO dto);
		void ActualizarCese(PersonalPeriodoLaboralDTO dto);
	}
}
