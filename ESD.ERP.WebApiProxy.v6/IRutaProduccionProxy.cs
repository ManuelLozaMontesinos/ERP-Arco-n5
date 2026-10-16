using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRutaProduccionProxy
	{
		RutaProduccionDTO GetById(RutaProduccionDTO dto);
		List<RutaProduccionDTO> Search(RutaProduccionDTO dto);
		RutaProduccionDTO SingleSearch(RutaProduccionDTO dto);
		int Insert(RutaProduccionDTO dto);
		int Update(RutaProduccionDTO dto);
		void Delete(RutaProduccionDTO dto);
	}
}
