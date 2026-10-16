using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoAperturaCierreBusiness
	{
		EstadoAperturaCierreDTO GetById(int id);
		List<EstadoAperturaCierreDTO> Search(EstadoAperturaCierreDTO dto);
		EstadoAperturaCierreDTO SingleSearch(EstadoAperturaCierreDTO dto);
		int Insert(EstadoAperturaCierreDTO dto);
		int Update(EstadoAperturaCierreDTO dto);
		void Delete(EstadoAperturaCierreDTO dto);
	}
}
