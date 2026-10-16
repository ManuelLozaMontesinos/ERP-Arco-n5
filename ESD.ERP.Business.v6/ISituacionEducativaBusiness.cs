using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ISituacionEducativaBusiness
	{
		SituacionEducativaDTO GetById(int id);
		List<SituacionEducativaDTO> Search(SituacionEducativaDTO dto);
		SituacionEducativaDTO SingleSearch(SituacionEducativaDTO dto);
		int Insert(SituacionEducativaDTO dto);
		int Update(SituacionEducativaDTO dto);
		void Delete(SituacionEducativaDTO dto);
		SituacionEducativaDTO ObtenerPorCodigo(string codigo);
	}
}
