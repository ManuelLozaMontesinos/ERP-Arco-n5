using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoComercialProductoProxy
	{
		EstadoComercialProductoDTO GetById(EstadoComercialProductoDTO dto);
		List<EstadoComercialProductoDTO> Search(EstadoComercialProductoDTO dto);
		EstadoComercialProductoDTO SingleSearch(EstadoComercialProductoDTO dto);
		int Insert(EstadoComercialProductoDTO dto);
		int Update(EstadoComercialProductoDTO dto);
		void Delete(EstadoComercialProductoDTO dto);
		EstadoComercialProductoDTO ObtenerPorCodigo(EstadoComercialProductoDTO dto);
	}
}
