using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoPagoRemuneracionProxy
	{
		TipoPagoRemuneracionDTO GetById(TipoPagoRemuneracionDTO dto);
		List<TipoPagoRemuneracionDTO> Search(TipoPagoRemuneracionDTO dto);
		TipoPagoRemuneracionDTO SingleSearch(TipoPagoRemuneracionDTO dto);
		int Insert(TipoPagoRemuneracionDTO dto);
		int Update(TipoPagoRemuneracionDTO dto);
		void Delete(TipoPagoRemuneracionDTO dto);
		TipoPagoRemuneracionDTO ObtenerPorCodigo(TipoPagoRemuneracionDTO dto);
	}
}
