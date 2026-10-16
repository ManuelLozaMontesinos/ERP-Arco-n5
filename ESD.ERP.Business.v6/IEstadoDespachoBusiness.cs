using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoDespachoBusiness
	{
		EstadoDespachoDTO GetById(int id);
		List<EstadoDespachoDTO> Search(EstadoDespachoDTO dto);
		EstadoDespachoDTO SingleSearch(EstadoDespachoDTO dto);
		int Insert(EstadoDespachoDTO dto);
		int Update(EstadoDespachoDTO dto);
		void Delete(EstadoDespachoDTO dto);
		EstadoDespachoDTO ObtenerPorCodigo(string codigo);
	}
}
