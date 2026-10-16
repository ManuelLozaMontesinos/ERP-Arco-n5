using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoDatoProxy
	{
		TipoDatoDTO GetById(TipoDatoDTO dto);
		List<TipoDatoDTO> Search(TipoDatoDTO dto);
		TipoDatoDTO SingleSearch(TipoDatoDTO dto);
		int Insert(TipoDatoDTO dto);
		int Update(TipoDatoDTO dto);
		void Delete(TipoDatoDTO dto);
		TipoDatoDTO ObtenerPorCodigo(TipoDatoDTO dto);
	}
}
