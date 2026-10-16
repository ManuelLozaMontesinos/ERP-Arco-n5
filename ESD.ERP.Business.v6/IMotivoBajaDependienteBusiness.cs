using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMotivoBajaDependienteBusiness
	{
		MotivoBajaDependienteDTO GetById(int id);
		List<MotivoBajaDependienteDTO> Search(MotivoBajaDependienteDTO dto);
		MotivoBajaDependienteDTO SingleSearch(MotivoBajaDependienteDTO dto);
		int Insert(MotivoBajaDependienteDTO dto);
		int Update(MotivoBajaDependienteDTO dto);
		void Delete(MotivoBajaDependienteDTO dto);
	}
}
