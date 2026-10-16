using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoZonaBusiness
	{
		TipoZonaDTO GetById(int id);
		List<TipoZonaDTO> Search(TipoZonaDTO dto);
		TipoZonaDTO SingleSearch(TipoZonaDTO dto);
		int Insert(TipoZonaDTO dto);
		int Update(TipoZonaDTO dto);
		void Delete(TipoZonaDTO dto);
		TipoZonaDTO ObtenerPorCodigo(string codigo);
	}
}
