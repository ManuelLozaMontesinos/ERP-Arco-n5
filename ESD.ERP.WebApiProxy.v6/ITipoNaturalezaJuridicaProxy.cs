using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoNaturalezaJuridicaProxy
	{
		TipoNaturalezaJuridicaDTO GetById(TipoNaturalezaJuridicaDTO dto);
		List<TipoNaturalezaJuridicaDTO> Search(TipoNaturalezaJuridicaDTO dto);
		TipoNaturalezaJuridicaDTO SingleSearch(TipoNaturalezaJuridicaDTO dto);
		int Insert(TipoNaturalezaJuridicaDTO dto);
		int Update(TipoNaturalezaJuridicaDTO dto);
		void Delete(TipoNaturalezaJuridicaDTO dto);
		TipoNaturalezaJuridicaDTO ObtenerPorCodigo(TipoNaturalezaJuridicaDTO dto);
	}
}
