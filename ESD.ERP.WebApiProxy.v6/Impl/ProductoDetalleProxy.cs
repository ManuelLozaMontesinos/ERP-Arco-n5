using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ProductoDetalleProxy : WebApiProxyClient, IProductoDetalleProxy
	{
		private const string API_CONTROLLER = "ProductoDetalle";
		public ProductoDetalleDTO GetById(ProductoDetalleDTO dto)
		{
			var result = RestClient.PostApi<ProductoDetalleDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ProductoDetalleDTO> Search(ProductoDetalleDTO dto)
		{
			var result = RestClient.PostApi<List<ProductoDetalleDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ProductoDetalleDTO SingleSearch(ProductoDetalleDTO dto)
		{
			var result = RestClient.PostApi<ProductoDetalleDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ProductoDetalleDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ProductoDetalleDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ProductoDetalleDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
