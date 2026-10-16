using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IComprobanteProductoEntregaProxy
	{
		ComprobanteProductoEntregaDTO GetById(ComprobanteProductoEntregaDTO dto);
		List<ComprobanteProductoEntregaDTO> Search(ComprobanteProductoEntregaDTO dto);
		ComprobanteProductoEntregaDTO SingleSearch(ComprobanteProductoEntregaDTO dto);
		int Insert(ComprobanteProductoEntregaDTO dto);
		int Update(ComprobanteProductoEntregaDTO dto);
		void Delete(ComprobanteProductoEntregaDTO dto);
	}
}
