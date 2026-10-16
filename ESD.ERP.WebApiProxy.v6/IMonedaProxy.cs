using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMonedaProxy
	{
		MonedaDTO GetById(MonedaDTO dto);
		List<MonedaDTO> Search(MonedaDTO dto);
		MonedaDTO SingleSearch(MonedaDTO dto);
		int Insert(MonedaDTO dto);
		int Update(MonedaDTO dto);
		void Delete(MonedaDTO dto);
		MonedaDTO ObtenerPorCodigo(MonedaDTO dto);
	}
}
