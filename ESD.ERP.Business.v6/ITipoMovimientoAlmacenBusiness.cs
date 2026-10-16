using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface ITipoMovimientoAlmacenBusiness
	{
		TipoMovimientoAlmacenDTO GetById(int id);
		List<TipoMovimientoAlmacenDTO> Search(TipoMovimientoAlmacenDTO dto);
		TipoMovimientoAlmacenDTO SingleSearch(TipoMovimientoAlmacenDTO dto);
		int Insert(TipoMovimientoAlmacenDTO dto);
		int Update(TipoMovimientoAlmacenDTO dto);
		void Delete(TipoMovimientoAlmacenDTO dto);
		TipoMovimientoAlmacenDTO ObtenerPorCodigo(string codigo);
	}
}
