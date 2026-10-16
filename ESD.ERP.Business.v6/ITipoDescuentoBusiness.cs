using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoDescuentoBusiness
	{
		TipoDescuentoDTO GetById(int id);
		List<TipoDescuentoDTO> Search(TipoDescuentoDTO dto);
		TipoDescuentoDTO SingleSearch(TipoDescuentoDTO dto);
		int Insert(TipoDescuentoDTO dto);
		int Update(TipoDescuentoDTO dto);
		void Delete(TipoDescuentoDTO dto);
		TipoDescuentoDTO ObtenerPorCodigo(string codigo);
	}
}
