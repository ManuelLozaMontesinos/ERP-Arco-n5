using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoCambioDiaProxy
	{
		TipoCambioDiaDTO GetById(TipoCambioDiaDTO dto);
		List<TipoCambioDiaDTO> Search(TipoCambioDiaDTO dto);
		TipoCambioDiaDTO SingleSearch(TipoCambioDiaDTO dto);
		int Insert(TipoCambioDiaDTO dto);
		int Update(TipoCambioDiaDTO dto);
		void Delete(TipoCambioDiaDTO dto);
	}
}
