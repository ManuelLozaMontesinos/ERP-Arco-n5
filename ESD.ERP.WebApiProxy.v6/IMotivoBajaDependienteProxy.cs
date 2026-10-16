using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMotivoBajaDependienteProxy
	{
		MotivoBajaDependienteDTO GetById(MotivoBajaDependienteDTO dto);
		List<MotivoBajaDependienteDTO> Search(MotivoBajaDependienteDTO dto);
		MotivoBajaDependienteDTO SingleSearch(MotivoBajaDependienteDTO dto);
		int Insert(MotivoBajaDependienteDTO dto);
		int Update(MotivoBajaDependienteDTO dto);
		void Delete(MotivoBajaDependienteDTO dto);
	}
}
