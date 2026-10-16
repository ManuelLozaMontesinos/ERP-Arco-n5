using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoContratoProxy
	{
		TipoContratoDTO GetById(TipoContratoDTO dto);
		List<TipoContratoDTO> Search(TipoContratoDTO dto);
		TipoContratoDTO SingleSearch(TipoContratoDTO dto);
		int Insert(TipoContratoDTO dto);
		int Update(TipoContratoDTO dto);
		void Delete(TipoContratoDTO dto);
		TipoContratoDTO ObtenerPorCodigo(TipoContratoDTO dto);
	}
}
