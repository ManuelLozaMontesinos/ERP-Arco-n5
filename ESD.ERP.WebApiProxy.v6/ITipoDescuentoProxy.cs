using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoDescuentoProxy
	{
		TipoDescuentoDTO GetById(TipoDescuentoDTO dto);
		List<TipoDescuentoDTO> Search(TipoDescuentoDTO dto);
		TipoDescuentoDTO SingleSearch(TipoDescuentoDTO dto);
		int Insert(TipoDescuentoDTO dto);
		int Update(TipoDescuentoDTO dto);
		void Delete(TipoDescuentoDTO dto);
		TipoDescuentoDTO ObtenerPorCodigo(TipoDescuentoDTO dto);
	}
}