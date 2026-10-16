using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IUnidadMedidaProxy
	{
		UnidadMedidaDTO GetById(UnidadMedidaDTO dto);
		List<UnidadMedidaDTO> Search(UnidadMedidaDTO dto);
		UnidadMedidaDTO SingleSearch(UnidadMedidaDTO dto);
		int Insert(UnidadMedidaDTO dto);
		int Update(UnidadMedidaDTO dto);
		void Delete(UnidadMedidaDTO dto);
		UnidadMedidaDTO ObtenerPorCodigo(UnidadMedidaDTO dto);
	}
}
