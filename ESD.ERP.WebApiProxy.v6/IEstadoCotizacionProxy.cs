using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoCotizacionProxy
	{
		EstadoCotizacionDTO GetById(EstadoCotizacionDTO dto);
		List<EstadoCotizacionDTO> Search(EstadoCotizacionDTO dto);
		EstadoCotizacionDTO SingleSearch(EstadoCotizacionDTO dto);
		int Insert(EstadoCotizacionDTO dto);
		int Update(EstadoCotizacionDTO dto);
		void Delete(EstadoCotizacionDTO dto);
		EstadoCotizacionDTO ObtenerPorCodigo(EstadoCotizacionDTO dto);
	}
}
