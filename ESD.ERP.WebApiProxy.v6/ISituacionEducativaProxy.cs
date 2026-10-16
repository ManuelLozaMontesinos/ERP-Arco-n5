using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ISituacionEducativaProxy
	{
		SituacionEducativaDTO GetById(SituacionEducativaDTO dto);
		List<SituacionEducativaDTO> Search(SituacionEducativaDTO dto);
		SituacionEducativaDTO SingleSearch(SituacionEducativaDTO dto);
		int Insert(SituacionEducativaDTO dto);
		int Update(SituacionEducativaDTO dto);
		void Delete(SituacionEducativaDTO dto);
		SituacionEducativaDTO ObtenerPorCodigo(SituacionEducativaDTO dto);
	}
}
