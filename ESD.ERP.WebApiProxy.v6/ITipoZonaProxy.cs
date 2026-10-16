using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoZonaProxy
	{
		TipoZonaDTO GetById(TipoZonaDTO dto);
		List<TipoZonaDTO> Search(TipoZonaDTO dto);
		TipoZonaDTO SingleSearch(TipoZonaDTO dto);
		int Insert(TipoZonaDTO dto);
		int Update(TipoZonaDTO dto);
		void Delete(TipoZonaDTO dto);
		TipoZonaDTO ObtenerPorCodigo(TipoZonaDTO dto);
	}
}
