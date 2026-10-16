using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoProductoBusiness
	{
		EstadoProductoDTO GetById(int id);
		List<EstadoProductoDTO> Search(EstadoProductoDTO dto);
		EstadoProductoDTO SingleSearch(EstadoProductoDTO dto);
		int Insert(EstadoProductoDTO dto);
		int Update(EstadoProductoDTO dto);
		void Delete(EstadoProductoDTO dto);
		EstadoProductoDTO ObtenerPorCodigo(string codigo);
	}
}
