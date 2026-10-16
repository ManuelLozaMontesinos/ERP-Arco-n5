using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoIgvBusiness
	{
		TipoIgvDTO GetById(int id);
		List<TipoIgvDTO> Search(TipoIgvDTO dto);
		TipoIgvDTO SingleSearch(TipoIgvDTO dto);
		int Insert(TipoIgvDTO dto);
		int Update(TipoIgvDTO dto);
		void Delete(TipoIgvDTO dto);
		TipoIgvDTO ObtenerPorCodigo(string codigo);
	}
}
