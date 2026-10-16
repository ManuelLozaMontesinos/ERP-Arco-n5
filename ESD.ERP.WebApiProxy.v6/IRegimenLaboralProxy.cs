using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRegimenLaboralProxy
	{
		RegimenLaboralDTO GetById(RegimenLaboralDTO dto);
		List<RegimenLaboralDTO> Search(RegimenLaboralDTO dto);
		RegimenLaboralDTO SingleSearch(RegimenLaboralDTO dto);
		int Insert(RegimenLaboralDTO dto);
		int Update(RegimenLaboralDTO dto);
		void Delete(RegimenLaboralDTO dto);
		RegimenLaboralDTO ObtenerPorCodigo(RegimenLaboralDTO dto);
	}
}
