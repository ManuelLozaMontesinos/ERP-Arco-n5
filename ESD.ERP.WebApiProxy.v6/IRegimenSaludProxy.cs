using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IRegimenSaludProxy
	{
		RegimenSaludDTO GetById(RegimenSaludDTO dto);
		List<RegimenSaludDTO> Search(RegimenSaludDTO dto);
		RegimenSaludDTO SingleSearch(RegimenSaludDTO dto);
		int Insert(RegimenSaludDTO dto);
		int Update(RegimenSaludDTO dto);
		void Delete(RegimenSaludDTO dto);
		RegimenSaludDTO ObtenerPorCodigo(RegimenSaludDTO dto);
	}
}
