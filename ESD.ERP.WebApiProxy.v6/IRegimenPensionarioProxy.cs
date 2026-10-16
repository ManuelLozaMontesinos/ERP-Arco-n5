using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRegimenPensionarioProxy
	{
		RegimenPensionarioDTO GetById(RegimenPensionarioDTO dto);
		List<RegimenPensionarioDTO> Search(RegimenPensionarioDTO dto);
		RegimenPensionarioDTO SingleSearch(RegimenPensionarioDTO dto);
		int Insert(RegimenPensionarioDTO dto);
		int Update(RegimenPensionarioDTO dto);
		void Delete(RegimenPensionarioDTO dto);
		RegimenPensionarioDTO ObtenerPorCodigo(RegimenPensionarioDTO dto);
	}
}
