using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRutaProduccionBusiness
	{
		RutaProduccionDTO GetById(int id);
		List<RutaProduccionDTO> Search(RutaProduccionDTO dto);
		RutaProduccionDTO SingleSearch(RutaProduccionDTO dto);
		int Insert(RutaProduccionDTO dto);
		int Update(RutaProduccionDTO dto);
		void Delete(RutaProduccionDTO dto);
	}
}
