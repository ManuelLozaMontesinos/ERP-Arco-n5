using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IProcesoProduccionProxy
	{
		ProcesoProduccionDTO GetById(ProcesoProduccionDTO dto);
		List<ProcesoProduccionDTO> Search(ProcesoProduccionDTO dto);
		ProcesoProduccionDTO SingleSearch(ProcesoProduccionDTO dto);
		int Insert(ProcesoProduccionDTO dto);
		int Update(ProcesoProduccionDTO dto);
		void Delete(ProcesoProduccionDTO dto);
	}
}
