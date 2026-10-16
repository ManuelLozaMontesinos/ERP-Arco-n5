using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoOrdenCompraProxy
	{
		EstadoOrdenCompraDTO GetById(EstadoOrdenCompraDTO dto);
		List<EstadoOrdenCompraDTO> Search(EstadoOrdenCompraDTO dto);
		EstadoOrdenCompraDTO SingleSearch(EstadoOrdenCompraDTO dto);
		int Insert(EstadoOrdenCompraDTO dto);
		int Update(EstadoOrdenCompraDTO dto);
		void Delete(EstadoOrdenCompraDTO dto);
		EstadoOrdenCompraDTO ObtenerPorCodigo(EstadoOrdenCompraDTO dto);

	}
}
