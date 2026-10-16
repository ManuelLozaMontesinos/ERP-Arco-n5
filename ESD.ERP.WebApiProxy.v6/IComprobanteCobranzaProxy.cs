using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IComprobanteCobranzaProxy
	{
		ComprobanteCobranzaDTO GetById(ComprobanteCobranzaDTO dto);
		List<ComprobanteCobranzaDTO> Search(ComprobanteCobranzaDTO dto);
		ComprobanteCobranzaDTO SingleSearch(ComprobanteCobranzaDTO dto);
		int Insert(ComprobanteCobranzaDTO dto);
		int Update(ComprobanteCobranzaDTO dto);
		void Delete(ComprobanteCobranzaDTO dto);
	}
}
