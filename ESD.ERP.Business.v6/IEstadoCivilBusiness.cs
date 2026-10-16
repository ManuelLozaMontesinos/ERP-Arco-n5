using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoCivilBusiness
	{
		EstadoCivilDTO GetById(int id);
		List<EstadoCivilDTO> Search(EstadoCivilDTO dto);
		EstadoCivilDTO SingleSearch(EstadoCivilDTO dto);
		int Insert(EstadoCivilDTO dto);
		int Update(EstadoCivilDTO dto);
		void Delete(EstadoCivilDTO dto);
		EstadoCivilDTO ObtenerPorCodigo(string codigo);
	}
}
