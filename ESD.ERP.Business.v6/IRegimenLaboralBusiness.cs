using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRegimenLaboralBusiness
	{
		RegimenLaboralDTO GetById(int id);
		List<RegimenLaboralDTO> Search(RegimenLaboralDTO dto);
		RegimenLaboralDTO SingleSearch(RegimenLaboralDTO dto);
		int Insert(RegimenLaboralDTO dto);
		int Update(RegimenLaboralDTO dto);
		void Delete(RegimenLaboralDTO dto);
		RegimenLaboralDTO ObtenerPorCodigo(string codigo);
	}
}
