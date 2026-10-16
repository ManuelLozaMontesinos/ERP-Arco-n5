using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ISubProcesoProduccionPersonalProxy
	{
		SubProcesoProduccionPersonalDTO GetById(SubProcesoProduccionPersonalDTO dto);
		List<SubProcesoProduccionPersonalDTO> Search(SubProcesoProduccionPersonalDTO dto);
		SubProcesoProduccionPersonalDTO SingleSearch(SubProcesoProduccionPersonalDTO dto);
		int Insert(SubProcesoProduccionPersonalDTO dto);
		int Update(SubProcesoProduccionPersonalDTO dto);
		void Delete(SubProcesoProduccionPersonalDTO dto);
	}
}
