using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoSunatBusiness
	{
		EstadoSunatDTO GetById(int id);
		List<EstadoSunatDTO> Search(EstadoSunatDTO dto);
		EstadoSunatDTO SingleSearch(EstadoSunatDTO dto);
		int Insert(EstadoSunatDTO dto);
		int Update(EstadoSunatDTO dto);
		void Delete(EstadoSunatDTO dto);
		EstadoSunatDTO ObtenerPorCodigo(string codigo);
	}
}
