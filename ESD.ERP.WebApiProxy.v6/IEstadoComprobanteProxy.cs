using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoComprobanteProxy
	{
		EstadoComprobanteDTO GetById(EstadoComprobanteDTO dto);
		List<EstadoComprobanteDTO> Search(EstadoComprobanteDTO dto);
		EstadoComprobanteDTO SingleSearch(EstadoComprobanteDTO dto);
		int Insert(EstadoComprobanteDTO dto);
		int Update(EstadoComprobanteDTO dto);
		void Delete(EstadoComprobanteDTO dto);
		EstadoComprobanteDTO ObtenerPorCodigo(EstadoComprobanteDTO dto);
	}
}
