using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoMovimientoAlmacenProxy
	{
		EstadoMovimientoAlmacenDTO GetById(EstadoMovimientoAlmacenDTO dto);
		List<EstadoMovimientoAlmacenDTO> Search(EstadoMovimientoAlmacenDTO dto);
		EstadoMovimientoAlmacenDTO SingleSearch(EstadoMovimientoAlmacenDTO dto);
		int Insert(EstadoMovimientoAlmacenDTO dto);
		int Update(EstadoMovimientoAlmacenDTO dto);
		void Delete(EstadoMovimientoAlmacenDTO dto);
		EstadoMovimientoAlmacenDTO ObtenerPorCodigo(EstadoMovimientoAlmacenDTO dto);
	}
}