using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoVariableCalculoProxy
	{
		TipoVariableCalculoDTO GetById(TipoVariableCalculoDTO dto);
		List<TipoVariableCalculoDTO> Search(TipoVariableCalculoDTO dto);
		TipoVariableCalculoDTO SingleSearch(TipoVariableCalculoDTO dto);
		int Insert(TipoVariableCalculoDTO dto);
		int Update(TipoVariableCalculoDTO dto);
		void Delete(TipoVariableCalculoDTO dto);
		TipoVariableCalculoDTO ObtenerPorCodigo(TipoVariableCalculoDTO dto);
	}
}
