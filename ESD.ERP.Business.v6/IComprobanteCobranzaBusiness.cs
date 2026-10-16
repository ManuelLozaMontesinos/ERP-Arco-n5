using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IComprobanteCobranzaBusiness
	{
		ComprobanteCobranzaDTO GetById(int id);
		List<ComprobanteCobranzaDTO> Search(ComprobanteCobranzaDTO dto);
		ComprobanteCobranzaDTO SingleSearch(ComprobanteCobranzaDTO dto);
		int Insert(ComprobanteCobranzaDTO dto);
		int Update(ComprobanteCobranzaDTO dto);
		void Delete(ComprobanteCobranzaDTO dto);
	}
}
