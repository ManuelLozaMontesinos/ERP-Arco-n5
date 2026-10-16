using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IEstadoProductoProxy
	{
		EstadoProductoDTO GetById(EstadoProductoDTO dto);
		List<EstadoProductoDTO> Search(EstadoProductoDTO dto);
		EstadoProductoDTO SingleSearch(EstadoProductoDTO dto);
		int Insert(EstadoProductoDTO dto);
		int Update(EstadoProductoDTO dto);
		void Delete(EstadoProductoDTO dto);
		EstadoProductoDTO ObtenerPorCodigo(EstadoProductoDTO dto);
	}
}
