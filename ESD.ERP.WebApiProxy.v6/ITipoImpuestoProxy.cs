using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoImpuestoProxy
	{
		TipoImpuestoDTO GetById(TipoImpuestoDTO dto);
		List<TipoImpuestoDTO> Search(TipoImpuestoDTO dto);
		TipoImpuestoDTO SingleSearch(TipoImpuestoDTO dto);
		int Insert(TipoImpuestoDTO dto);
		int Update(TipoImpuestoDTO dto);
		void Delete(TipoImpuestoDTO dto);
	}
}
