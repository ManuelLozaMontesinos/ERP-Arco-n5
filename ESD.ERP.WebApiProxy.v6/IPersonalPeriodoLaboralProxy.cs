using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IPersonalPeriodoLaboralProxy
	{
		PersonalPeriodoLaboralDTO GetById(PersonalPeriodoLaboralDTO dto);
		List<PersonalPeriodoLaboralDTO> Search(PersonalPeriodoLaboralDTO dto);
		PersonalPeriodoLaboralDTO SingleSearch(PersonalPeriodoLaboralDTO dto);
		int Insert(PersonalPeriodoLaboralDTO dto);
		int Update(PersonalPeriodoLaboralDTO dto);
		void Delete(PersonalPeriodoLaboralDTO dto);
		void ActualizarCese(PersonalPeriodoLaboralDTO dto);
	}
}
