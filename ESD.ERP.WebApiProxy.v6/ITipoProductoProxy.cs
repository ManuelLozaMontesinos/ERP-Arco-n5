using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoProductoProxy
	{
		TipoProductoDTO GetById(TipoProductoDTO dto);
		List<TipoProductoDTO> Search(TipoProductoDTO dto);
		TipoProductoDTO SingleSearch(TipoProductoDTO dto);
		int Insert(TipoProductoDTO dto);
		int Update(TipoProductoDTO dto);
		void Delete(TipoProductoDTO dto);
		TipoProductoDTO ObtenerPorCodigo(TipoProductoDTO dto);
	}
}
