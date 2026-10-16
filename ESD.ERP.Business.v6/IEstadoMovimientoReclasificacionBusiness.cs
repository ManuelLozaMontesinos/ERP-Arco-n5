using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoMovimientoReclasificacionBusiness
	{
		EstadoMovimientoReclasificacionDTO GetById(int id);
		List<EstadoMovimientoReclasificacionDTO> Search(EstadoMovimientoReclasificacionDTO dto);
		EstadoMovimientoReclasificacionDTO SingleSearch(EstadoMovimientoReclasificacionDTO dto);
		int Insert(EstadoMovimientoReclasificacionDTO dto);
		int Update(EstadoMovimientoReclasificacionDTO dto);
		void Delete(EstadoMovimientoReclasificacionDTO dto);
		EstadoMovimientoReclasificacionDTO ObtenerPorCodigo(string codigo);
	}
}
