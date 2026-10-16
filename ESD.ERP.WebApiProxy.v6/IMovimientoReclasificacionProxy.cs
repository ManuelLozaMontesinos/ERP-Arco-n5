using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IMovimientoReclasificacionProxy
	{
		MovimientoReclasificacionDTO GetById(MovimientoReclasificacionDTO dto);
		List<MovimientoReclasificacionDTO> Search(MovimientoReclasificacionDTO dto);
		MovimientoReclasificacionDTO SingleSearch(MovimientoReclasificacionDTO dto);
		int Insert(MovimientoReclasificacionDTO dto);
		int Update(MovimientoReclasificacionDTO dto);
		void Delete(MovimientoReclasificacionDTO dto);
		void Solicitar(MovimientoReclasificacionDTO dto);
		void Aprobar(MovimientoReclasificacionDTO dto);
		void Desaprobar(MovimientoReclasificacionDTO dto);
	}
}
