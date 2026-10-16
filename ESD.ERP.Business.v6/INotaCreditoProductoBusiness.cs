using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface INotaCreditoProductoBusiness
	{
		NotaCreditoProductoDTO GetById(int id);
		List<NotaCreditoProductoDTO> Search(NotaCreditoProductoDTO dto);
		NotaCreditoProductoDTO SingleSearch(NotaCreditoProductoDTO dto);
		int Insert(NotaCreditoProductoDTO dto);
		int Update(NotaCreditoProductoDTO dto);
		void Delete(NotaCreditoProductoDTO dto);
		void AgregarComprobanteProducto(NotaCreditoProductoDTO dto);
	}
}
