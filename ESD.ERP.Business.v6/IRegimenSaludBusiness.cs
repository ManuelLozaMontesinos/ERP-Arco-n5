using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRegimenSaludBusiness
	{
		RegimenSaludDTO GetById(int id);
		List<RegimenSaludDTO> Search(RegimenSaludDTO dto);
		RegimenSaludDTO SingleSearch(RegimenSaludDTO dto);
		int Insert(RegimenSaludDTO dto);
		int Update(RegimenSaludDTO dto);
		void Delete(RegimenSaludDTO dto);
		RegimenSaludDTO ObtenerPorCodigo(string codigo);
	}
}
