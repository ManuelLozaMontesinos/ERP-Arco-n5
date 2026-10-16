using System.Collections.Generic;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy
{
	public interface IProductoSubProcesoProduccionProxy
	{
		ProductoSubProcesoProduccionDTO GetById(ProductoSubProcesoProduccionDTO dto);
		List<ProductoSubProcesoProduccionDTO> Search(ProductoSubProcesoProduccionDTO dto);
		ProductoSubProcesoProduccionDTO SingleSearch(ProductoSubProcesoProduccionDTO dto);
		int Insert(ProductoSubProcesoProduccionDTO dto);
		int Update(ProductoSubProcesoProduccionDTO dto);
		void Delete(ProductoSubProcesoProduccionDTO dto);
	}
}
