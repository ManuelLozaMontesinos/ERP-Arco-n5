using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoMovimientoAlmacenBusiness
	{
		EstadoMovimientoAlmacenDTO GetById(int id);
		List<EstadoMovimientoAlmacenDTO> Search(EstadoMovimientoAlmacenDTO dto);
		EstadoMovimientoAlmacenDTO SingleSearch(EstadoMovimientoAlmacenDTO dto);
		int Insert(EstadoMovimientoAlmacenDTO dto);
		int Update(EstadoMovimientoAlmacenDTO dto);
		void Delete(EstadoMovimientoAlmacenDTO dto);
		EstadoMovimientoAlmacenDTO ObtenerPorCodigo(string codigo);
	}
}
