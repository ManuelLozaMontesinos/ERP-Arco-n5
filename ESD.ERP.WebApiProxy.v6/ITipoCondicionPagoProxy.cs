using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoCondicionPagoProxy
	{
		TipoCondicionPagoDTO GetById(TipoCondicionPagoDTO dto);
		List<TipoCondicionPagoDTO> Search(TipoCondicionPagoDTO dto);
		TipoCondicionPagoDTO SingleSearch(TipoCondicionPagoDTO dto);
		int Insert(TipoCondicionPagoDTO dto);
		int Update(TipoCondicionPagoDTO dto);
		void Delete(TipoCondicionPagoDTO dto);
		TipoCondicionPagoDTO ObtenerPorCodigo(TipoCondicionPagoDTO dto);
	}
}