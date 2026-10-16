using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRutaSubProcesoProduccionBusiness
	{
		RutaSubProcesoProduccionDTO GetById(int id);
		List<RutaSubProcesoProduccionDTO> Search(RutaSubProcesoProduccionDTO dto);
		RutaSubProcesoProduccionDTO SingleSearch(RutaSubProcesoProduccionDTO dto);
		int Insert(RutaSubProcesoProduccionDTO dto);
		int Update(RutaSubProcesoProduccionDTO dto);
		void Delete(RutaSubProcesoProduccionDTO dto);
	}
}