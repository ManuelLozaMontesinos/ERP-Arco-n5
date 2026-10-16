using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRutaSubProcesoProduccionProxy
	{
		RutaSubProcesoProduccionDTO GetById(RutaSubProcesoProduccionDTO dto);
		List<RutaSubProcesoProduccionDTO> Search(RutaSubProcesoProduccionDTO dto);
		RutaSubProcesoProduccionDTO SingleSearch(RutaSubProcesoProduccionDTO dto);
		int Insert(RutaSubProcesoProduccionDTO dto);
		int Update(RutaSubProcesoProduccionDTO dto);
		void Delete(RutaSubProcesoProduccionDTO dto);
	}
}
