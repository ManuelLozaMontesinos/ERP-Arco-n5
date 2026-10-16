using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoComprobanteBusiness
	{
		TipoComprobanteDTO GetById(int id);
		List<TipoComprobanteDTO> Search(TipoComprobanteDTO dto);
		TipoComprobanteDTO SingleSearch(TipoComprobanteDTO dto);
		int Insert(TipoComprobanteDTO dto);
		int Update(TipoComprobanteDTO dto);
		void Delete(TipoComprobanteDTO dto);
		TipoComprobanteDTO ObtenerPorCodigo(string codigo);
	}
}
