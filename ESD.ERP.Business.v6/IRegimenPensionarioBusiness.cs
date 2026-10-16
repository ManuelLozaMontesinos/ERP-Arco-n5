using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IRegimenPensionarioBusiness
	{
		RegimenPensionarioDTO GetById(int id);
		List<RegimenPensionarioDTO> Search(RegimenPensionarioDTO dto);
		RegimenPensionarioDTO SingleSearch(RegimenPensionarioDTO dto);
		int Insert(RegimenPensionarioDTO dto);
		int Update(RegimenPensionarioDTO dto);
		void Delete(RegimenPensionarioDTO dto);
		RegimenPensionarioDTO ObtenerPorCodigo(string codigo);
	}
}
