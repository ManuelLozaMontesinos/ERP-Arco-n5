using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoDatoBusiness
	{
		TipoDatoDTO GetById(int id);
		List<TipoDatoDTO> Search(TipoDatoDTO dto);
		TipoDatoDTO SingleSearch(TipoDatoDTO dto);
		int Insert(TipoDatoDTO dto);
		int Update(TipoDatoDTO dto);
		void Delete(TipoDatoDTO dto);
		TipoDatoDTO ObtenerPorCodigo(string codigo);
	}
}
