using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoAperturaCierreProxy
	{
		EstadoAperturaCierreDTO GetById(EstadoAperturaCierreDTO dto);
		List<EstadoAperturaCierreDTO> Search(EstadoAperturaCierreDTO dto);
		EstadoAperturaCierreDTO SingleSearch(EstadoAperturaCierreDTO dto);
		int Insert(EstadoAperturaCierreDTO dto);
		int Update(EstadoAperturaCierreDTO dto);
		void Delete(EstadoAperturaCierreDTO dto);
	}
}
