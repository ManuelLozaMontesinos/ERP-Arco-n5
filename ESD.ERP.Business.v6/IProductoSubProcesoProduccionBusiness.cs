using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.Business
{
	public interface IProductoSubProcesoProduccionBusiness
	{
		ProductoSubProcesoProduccionDTO GetById(int id);
		List<ProductoSubProcesoProduccionDTO> Search(ProductoSubProcesoProduccionDTO dto);
		ProductoSubProcesoProduccionDTO SingleSearch(ProductoSubProcesoProduccionDTO dto);
		int Insert(ProductoSubProcesoProduccionDTO dto);
		int Update(ProductoSubProcesoProduccionDTO dto);
		void Delete(ProductoSubProcesoProduccionDTO dto);
	}
}
