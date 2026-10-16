using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoComprobanteProxy
	{
		TipoComprobanteDTO GetById(TipoComprobanteDTO dto);
		List<TipoComprobanteDTO> Search(TipoComprobanteDTO dto);
		TipoComprobanteDTO SingleSearch(TipoComprobanteDTO dto);
		int Insert(TipoComprobanteDTO dto);
		int Update(TipoComprobanteDTO dto);
		void Delete(TipoComprobanteDTO dto);
		TipoComprobanteDTO ObtenerPorCodigo(TipoComprobanteDTO dto);
	}
}
