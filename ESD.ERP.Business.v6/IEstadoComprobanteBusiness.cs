using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoComprobanteBusiness
	{
		EstadoComprobanteDTO GetById(int id);
		List<EstadoComprobanteDTO> Search(EstadoComprobanteDTO dto);
		EstadoComprobanteDTO SingleSearch(EstadoComprobanteDTO dto);
		int Insert(EstadoComprobanteDTO dto);
		int Update(EstadoComprobanteDTO dto);
		void Delete(EstadoComprobanteDTO dto);
		EstadoComprobanteDTO ObtenerPorCodigo(string codigo);
	}
}
