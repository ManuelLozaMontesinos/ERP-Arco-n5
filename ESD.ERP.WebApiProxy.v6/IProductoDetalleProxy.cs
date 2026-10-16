using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IProductoDetalleProxy
	{
		ProductoDetalleDTO GetById(ProductoDetalleDTO dto);
		List<ProductoDetalleDTO> Search(ProductoDetalleDTO dto);
		ProductoDetalleDTO SingleSearch(ProductoDetalleDTO dto);
		int Insert(ProductoDetalleDTO dto);
		int Update(ProductoDetalleDTO dto);
		void Delete(ProductoDetalleDTO dto);
	}
}
