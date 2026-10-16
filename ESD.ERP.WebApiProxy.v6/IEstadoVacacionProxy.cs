using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoVacacionProxy
	{
		EstadoVacacionDTO GetById(EstadoVacacionDTO dto);
		List<EstadoVacacionDTO> Search(EstadoVacacionDTO dto);
		EstadoVacacionDTO SingleSearch(EstadoVacacionDTO dto);
		int Insert(EstadoVacacionDTO dto);
		int Update(EstadoVacacionDTO dto);
		void Delete(EstadoVacacionDTO dto);
	}
}
