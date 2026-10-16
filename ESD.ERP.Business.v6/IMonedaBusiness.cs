using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IMonedaBusiness
	{
		MonedaDTO GetById(int id);
		List<MonedaDTO> Search(MonedaDTO dto);
		MonedaDTO SingleSearch(MonedaDTO dto);
		int Insert(MonedaDTO dto);
		int Update(MonedaDTO dto);
		void Delete(MonedaDTO dto);
		MonedaDTO ObtenerPorCodigo(string codigo);
	}
}
