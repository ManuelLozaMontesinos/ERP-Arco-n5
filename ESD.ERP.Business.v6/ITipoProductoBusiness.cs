using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoProductoBusiness
	{
		TipoProductoDTO GetById(int id);
		List<TipoProductoDTO> Search(TipoProductoDTO dto);
		TipoProductoDTO SingleSearch(TipoProductoDTO dto);
		int Insert(TipoProductoDTO dto);
		int Update(TipoProductoDTO dto);
		void Delete(TipoProductoDTO dto);
		TipoProductoDTO ObtenerPorCodigo(string codigo);
	}
}
