using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ISctrSaludProxy
	{
		SctrSaludDTO GetById(SctrSaludDTO dto);
		List<SctrSaludDTO> Search(SctrSaludDTO dto);
		SctrSaludDTO SingleSearch(SctrSaludDTO dto);
		int Insert(SctrSaludDTO dto);
		int Update(SctrSaludDTO dto);
		void Delete(SctrSaludDTO dto);
		SctrSaludDTO ObtenerPorCodigo(SctrSaludDTO dto);
	}
}
