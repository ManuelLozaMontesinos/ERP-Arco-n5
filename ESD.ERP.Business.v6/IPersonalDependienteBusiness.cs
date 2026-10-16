using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IPersonalDependienteBusiness
	{
		PersonalDependienteDTO GetById(int id);
		List<PersonalDependienteDTO> Search(PersonalDependienteDTO dto);
		PersonalDependienteDTO SingleSearch(PersonalDependienteDTO dto);
		int Insert(PersonalDependienteDTO dto);
		int Update(PersonalDependienteDTO dto);
		void Delete(PersonalDependienteDTO dto);
		void ActualizarBaja(PersonalDependienteDTO dto);
	}
}
