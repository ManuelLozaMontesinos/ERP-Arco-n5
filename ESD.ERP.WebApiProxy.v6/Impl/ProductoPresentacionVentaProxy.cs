using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ProductoPresentacionVentaProxy : WebApiProxyClient, IProductoPresentacionVentaProxy
	{
		private const string API_CONTROLLER = "ProductoPresentacionVenta";
		public ProductoPresentacionVentaDTO GetById(ProductoPresentacionVentaDTO dto)
		{
			var result = RestClient.PostApi<ProductoPresentacionVentaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ProductoPresentacionVentaDTO> Search(ProductoPresentacionVentaDTO dto)
		{
			var result = RestClient.PostApi<List<ProductoPresentacionVentaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ProductoPresentacionVentaDTO SingleSearch(ProductoPresentacionVentaDTO dto)
		{
			var result = RestClient.PostApi<ProductoPresentacionVentaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ProductoPresentacionVentaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ProductoPresentacionVentaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ProductoPresentacionVentaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
