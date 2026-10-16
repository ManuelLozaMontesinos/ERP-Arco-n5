using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoCambioProxy
	{
		TipoCambioDTO GetById(TipoCambioDTO dto);
		List<TipoCambioDTO> Search(TipoCambioDTO dto);
		TipoCambioDTO SingleSearch(TipoCambioDTO dto);
		int Insert(TipoCambioDTO dto);
		int Update(TipoCambioDTO dto);
		void Delete(TipoCambioDTO dto);
	}
}
