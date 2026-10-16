using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OrdenCompraProductoProxy : WebApiProxyClient, IOrdenCompraProductoProxy
	{
		private const string API_CONTROLLER = "OrdenCompraProducto";
		public OrdenCompraProductoDTO GetById(OrdenCompraProductoDTO dto)
		{
			var result = RestClient.PostApi<OrdenCompraProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OrdenCompraProductoDTO> Search(OrdenCompraProductoDTO dto)
		{
			var result = RestClient.PostApi<List<OrdenCompraProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OrdenCompraProductoDTO SingleSearch(OrdenCompraProductoDTO dto)
		{
			var result = RestClient.PostApi<OrdenCompraProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OrdenCompraProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OrdenCompraProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OrdenCompraProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
