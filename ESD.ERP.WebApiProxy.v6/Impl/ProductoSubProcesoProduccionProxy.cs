using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ProductoSubProcesoProduccionProxy : WebApiProxyClient, IProductoSubProcesoProduccionProxy
	{
		private const string API_CONTROLLER = "ProductoSubProcesoProduccion";
		public ProductoSubProcesoProduccionDTO GetById(ProductoSubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<ProductoSubProcesoProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ProductoSubProcesoProduccionDTO> Search(ProductoSubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<List<ProductoSubProcesoProduccionDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ProductoSubProcesoProduccionDTO SingleSearch(ProductoSubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<ProductoSubProcesoProduccionDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ProductoSubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ProductoSubProcesoProduccionDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ProductoSubProcesoProduccionDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
