using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoSunatProxy
	{
		EstadoSunatDTO GetById(EstadoSunatDTO dto);
		List<EstadoSunatDTO> Search(EstadoSunatDTO dto);
		EstadoSunatDTO SingleSearch(EstadoSunatDTO dto);
		int Insert(EstadoSunatDTO dto);
		int Update(EstadoSunatDTO dto);
		void Delete(EstadoSunatDTO dto);
		EstadoSunatDTO ObtenerPorCodigo(EstadoSunatDTO dto);
	}
}
