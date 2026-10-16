using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IProductoPresentacionVentaProxy
	{
		ProductoPresentacionVentaDTO GetById(ProductoPresentacionVentaDTO dto);
		List<ProductoPresentacionVentaDTO> Search(ProductoPresentacionVentaDTO dto);
		ProductoPresentacionVentaDTO SingleSearch(ProductoPresentacionVentaDTO dto);
		int Insert(ProductoPresentacionVentaDTO dto);
		int Update(ProductoPresentacionVentaDTO dto);
		void Delete(ProductoPresentacionVentaDTO dto);
	}
}
