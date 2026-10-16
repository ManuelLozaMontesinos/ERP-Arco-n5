using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoImpuestoBusiness
	{
		TipoImpuestoDTO GetById(int id);
		List<TipoImpuestoDTO> Search(TipoImpuestoDTO dto);
		TipoImpuestoDTO SingleSearch(TipoImpuestoDTO dto);
		int Insert(TipoImpuestoDTO dto);
		int Update(TipoImpuestoDTO dto);
		void Delete(TipoImpuestoDTO dto);
	}
}
