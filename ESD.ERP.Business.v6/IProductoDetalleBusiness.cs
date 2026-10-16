using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProductoDetalleBusiness
	{
		ProductoDetalleDTO GetById(int id);
		List<ProductoDetalleDTO> Search(ProductoDetalleDTO dto);
		ProductoDetalleDTO SingleSearch(ProductoDetalleDTO dto);
		int Insert(ProductoDetalleDTO dto);
		int Update(ProductoDetalleDTO dto);
		void Delete(ProductoDetalleDTO dto);
	}
}
