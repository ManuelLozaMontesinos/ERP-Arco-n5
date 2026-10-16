using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoPagoRemuneracionBusiness
	{
		TipoPagoRemuneracionDTO GetById(int id);
		List<TipoPagoRemuneracionDTO> Search(TipoPagoRemuneracionDTO dto);
		TipoPagoRemuneracionDTO SingleSearch(TipoPagoRemuneracionDTO dto);
		int Insert(TipoPagoRemuneracionDTO dto);
		int Update(TipoPagoRemuneracionDTO dto);
		void Delete(TipoPagoRemuneracionDTO dto);
		TipoPagoRemuneracionDTO ObtenerPorCodigo(string codigo);
	}
}
