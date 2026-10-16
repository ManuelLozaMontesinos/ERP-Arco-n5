using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoCambioDiaBusiness
	{
		TipoCambioDiaDTO GetById(int id);
		List<TipoCambioDiaDTO> Search(TipoCambioDiaDTO dto);
		TipoCambioDiaDTO SingleSearch(TipoCambioDiaDTO dto);
		int Insert(TipoCambioDiaDTO dto);
		int Update(TipoCambioDiaDTO dto);
		void Delete(TipoCambioDiaDTO dto);
	}
}
