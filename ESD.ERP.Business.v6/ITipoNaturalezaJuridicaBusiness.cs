using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoNaturalezaJuridicaBusiness
	{
		TipoNaturalezaJuridicaDTO GetById(int id);
		List<TipoNaturalezaJuridicaDTO> Search(TipoNaturalezaJuridicaDTO dto);
		TipoNaturalezaJuridicaDTO SingleSearch(TipoNaturalezaJuridicaDTO dto);
		int Insert(TipoNaturalezaJuridicaDTO dto);
		int Update(TipoNaturalezaJuridicaDTO dto);
		void Delete(TipoNaturalezaJuridicaDTO dto);
		TipoNaturalezaJuridicaDTO ObtenerPorCodigo(string codigo);
	}
}
