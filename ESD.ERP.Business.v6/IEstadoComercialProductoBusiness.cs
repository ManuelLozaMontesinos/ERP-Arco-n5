using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IEstadoComercialProductoBusiness
	{
		EstadoComercialProductoDTO GetById(int id);
		List<EstadoComercialProductoDTO> Search(EstadoComercialProductoDTO dto);
		EstadoComercialProductoDTO SingleSearch(EstadoComercialProductoDTO dto);
		int Insert(EstadoComercialProductoDTO dto);
		int Update(EstadoComercialProductoDTO dto);
		void Delete(EstadoComercialProductoDTO dto);
		EstadoComercialProductoDTO ObtenerPorCodigo(string codigo);
	}
}
