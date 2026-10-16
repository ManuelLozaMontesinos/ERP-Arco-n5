using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoVariableCalculoBusiness
	{
		TipoVariableCalculoDTO GetById(int id);
		List<TipoVariableCalculoDTO> Search(TipoVariableCalculoDTO dto);
		TipoVariableCalculoDTO SingleSearch(TipoVariableCalculoDTO dto);
		int Insert(TipoVariableCalculoDTO dto);
		int Update(TipoVariableCalculoDTO dto);
		void Delete(TipoVariableCalculoDTO dto);
		TipoVariableCalculoDTO ObtenerPorCodigo(string codigo);
	}
}
