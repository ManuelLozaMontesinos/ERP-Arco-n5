using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface ITipoMovimientoAlmacenProxy
	{
		TipoMovimientoAlmacenDTO GetById(TipoMovimientoAlmacenDTO dto);
		List<TipoMovimientoAlmacenDTO> Search(TipoMovimientoAlmacenDTO dto);
		TipoMovimientoAlmacenDTO SingleSearch(TipoMovimientoAlmacenDTO dto);
		int Insert(TipoMovimientoAlmacenDTO dto);
		int Update(TipoMovimientoAlmacenDTO dto);
		void Delete(TipoMovimientoAlmacenDTO dto);
		TipoMovimientoAlmacenDTO ObtenerPorCodigo(TipoMovimientoAlmacenDTO dto);
	}
}
