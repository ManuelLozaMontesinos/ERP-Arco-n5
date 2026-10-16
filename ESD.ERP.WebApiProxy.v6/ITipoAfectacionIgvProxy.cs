using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoAfectacionIgvProxy
	{
		TipoAfectacionIgvDTO GetById(TipoAfectacionIgvDTO dto);
		List<TipoAfectacionIgvDTO> Search(TipoAfectacionIgvDTO dto);
		TipoAfectacionIgvDTO SingleSearch(TipoAfectacionIgvDTO dto);
		int Insert(TipoAfectacionIgvDTO dto);
		int Update(TipoAfectacionIgvDTO dto);
		void Delete(TipoAfectacionIgvDTO dto);
	}
}
