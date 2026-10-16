using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoContratoBusiness
	{
		TipoContratoDTO GetById(int id);
		List<TipoContratoDTO> Search(TipoContratoDTO dto);
		TipoContratoDTO SingleSearch(TipoContratoDTO dto);
		int Insert(TipoContratoDTO dto);
		int Update(TipoContratoDTO dto);
		void Delete(TipoContratoDTO dto);
		TipoContratoDTO ObtenerPorCodigo(string codigo);
	}
}
