using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoVacacionBusiness
	{
		EstadoVacacionDTO GetById(int id);
		List<EstadoVacacionDTO> Search(EstadoVacacionDTO dto);
		EstadoVacacionDTO SingleSearch(EstadoVacacionDTO dto);
		int Insert(EstadoVacacionDTO dto);
		int Update(EstadoVacacionDTO dto);
		void Delete(EstadoVacacionDTO dto);
	}
}
