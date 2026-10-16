using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProductoPresentacionVentaBusiness
	{
		ProductoPresentacionVentaDTO GetById(int id);
		List<ProductoPresentacionVentaDTO> Search(ProductoPresentacionVentaDTO dto);
		ProductoPresentacionVentaDTO SingleSearch(ProductoPresentacionVentaDTO dto);
		int Insert(ProductoPresentacionVentaDTO dto);
		int Update(ProductoPresentacionVentaDTO dto);
		void Delete(ProductoPresentacionVentaDTO dto);
	}
}
