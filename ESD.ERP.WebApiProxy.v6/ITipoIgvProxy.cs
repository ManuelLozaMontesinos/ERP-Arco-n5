using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoIgvProxy
	{
		TipoIgvDTO GetById(TipoIgvDTO dto);
		List<TipoIgvDTO> Search(TipoIgvDTO dto);
		TipoIgvDTO SingleSearch(TipoIgvDTO dto);
		int Insert(TipoIgvDTO dto);
		int Update(TipoIgvDTO dto);
		void Delete(TipoIgvDTO dto);
		TipoIgvDTO ObtenerPorCodigo(TipoIgvDTO dto);
	}
}
