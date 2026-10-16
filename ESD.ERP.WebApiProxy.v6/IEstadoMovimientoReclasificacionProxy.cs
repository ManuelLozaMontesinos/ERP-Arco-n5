using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoMovimientoReclasificacionProxy
	{
		EstadoMovimientoReclasificacionDTO GetById(EstadoMovimientoReclasificacionDTO dto);
		List<EstadoMovimientoReclasificacionDTO> Search(EstadoMovimientoReclasificacionDTO dto);
		EstadoMovimientoReclasificacionDTO SingleSearch(EstadoMovimientoReclasificacionDTO dto);
		int Insert(EstadoMovimientoReclasificacionDTO dto);
		int Update(EstadoMovimientoReclasificacionDTO dto);
		void Delete(EstadoMovimientoReclasificacionDTO dto);
		EstadoMovimientoReclasificacionDTO ObtenerPorCodigo(EstadoMovimientoReclasificacionDTO dto);
	}
}
