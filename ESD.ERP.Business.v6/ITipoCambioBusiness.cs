using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoCambioBusiness
	{
		TipoCambioDTO GetById(int id);
		List<TipoCambioDTO> Search(TipoCambioDTO dto);
		TipoCambioDTO SingleSearch(TipoCambioDTO dto);
		int Insert(TipoCambioDTO dto);
		int Update(TipoCambioDTO dto);
		void Delete(TipoCambioDTO dto);
	}
}
