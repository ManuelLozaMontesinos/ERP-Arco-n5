using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoCivilProxy
	{
		EstadoCivilDTO GetById(EstadoCivilDTO dto);
		List<EstadoCivilDTO> Search(EstadoCivilDTO dto);
		EstadoCivilDTO SingleSearch(EstadoCivilDTO dto);
		int Insert(EstadoCivilDTO dto);
		int Update(EstadoCivilDTO dto);
		void Delete(EstadoCivilDTO dto);
		EstadoCivilDTO ObtenerPorCodigo(EstadoCivilDTO dto);
	}
}
