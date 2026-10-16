using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IUnidadMedidaBusiness
	{
		UnidadMedidaDTO GetById(int id);
		List<UnidadMedidaDTO> Search(UnidadMedidaDTO dto);
		UnidadMedidaDTO SingleSearch(UnidadMedidaDTO dto);
		int Insert(UnidadMedidaDTO dto);
		int Update(UnidadMedidaDTO dto);
		void Delete(UnidadMedidaDTO dto);
		UnidadMedidaDTO ObtenerPorCodigo(string codigo);
	}
}
