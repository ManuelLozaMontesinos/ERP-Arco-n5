using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IComprobanteProductoEntregaBusiness
	{
		ComprobanteProductoEntregaDTO GetById(int id);
		List<ComprobanteProductoEntregaDTO> Search(ComprobanteProductoEntregaDTO dto);
		ComprobanteProductoEntregaDTO SingleSearch(ComprobanteProductoEntregaDTO dto);
		int Insert(ComprobanteProductoEntregaDTO dto);
		int Update(ComprobanteProductoEntregaDTO dto);
		void Delete(ComprobanteProductoEntregaDTO dto);
	}
}
