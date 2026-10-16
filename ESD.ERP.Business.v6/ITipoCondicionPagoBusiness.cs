using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoCondicionPagoBusiness
	{
		TipoCondicionPagoDTO GetById(int id);
		List<TipoCondicionPagoDTO> Search(TipoCondicionPagoDTO dto);
		TipoCondicionPagoDTO SingleSearch(TipoCondicionPagoDTO dto);
		int Insert(TipoCondicionPagoDTO dto);
		int Update(TipoCondicionPagoDTO dto);
		void Delete(TipoCondicionPagoDTO dto);
		TipoCondicionPagoDTO ObtenerPorCodigo(string codigo);
	}
}
