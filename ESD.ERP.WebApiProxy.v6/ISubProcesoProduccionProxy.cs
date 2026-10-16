using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ISubProcesoProduccionProxy
	{
		SubProcesoProduccionDTO GetById(SubProcesoProduccionDTO dto);
		List<SubProcesoProduccionDTO> Search(SubProcesoProduccionDTO dto);
		SubProcesoProduccionDTO SingleSearch(SubProcesoProduccionDTO dto);
		int Insert(SubProcesoProduccionDTO dto);
		int Update(SubProcesoProduccionDTO dto);
		void Delete(SubProcesoProduccionDTO dto);
	}
}
