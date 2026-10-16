using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoCotizacionBusiness
	{
		EstadoCotizacionDTO GetById(int id);
		List<EstadoCotizacionDTO> Search(EstadoCotizacionDTO dto);
		EstadoCotizacionDTO SingleSearch(EstadoCotizacionDTO dto);
		int Insert(EstadoCotizacionDTO dto);
		int Update(EstadoCotizacionDTO dto);
		void Delete(EstadoCotizacionDTO dto);
		EstadoCotizacionDTO ObtenerPorCodigo(string codigo);
	}
}
