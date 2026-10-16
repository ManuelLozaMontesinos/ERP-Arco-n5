using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoAfectacionIgvBusiness
	{
		TipoAfectacionIgvDTO GetById(int id);
		List<TipoAfectacionIgvDTO> Search(TipoAfectacionIgvDTO dto);
		TipoAfectacionIgvDTO SingleSearch(TipoAfectacionIgvDTO dto);
		int Insert(TipoAfectacionIgvDTO dto);
		int Update(TipoAfectacionIgvDTO dto);
		void Delete(TipoAfectacionIgvDTO dto);
	}
}
