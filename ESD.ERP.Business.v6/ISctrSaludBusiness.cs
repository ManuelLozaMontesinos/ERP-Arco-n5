using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ISctrSaludBusiness
	{
		SctrSaludDTO GetById(int id);
		List<SctrSaludDTO> Search(SctrSaludDTO dto);
		SctrSaludDTO SingleSearch(SctrSaludDTO dto);
		int Insert(SctrSaludDTO dto);
		int Update(SctrSaludDTO dto);
		void Delete(SctrSaludDTO dto);
		SctrSaludDTO ObtenerPorCodigo(string codigo);
	}
}
