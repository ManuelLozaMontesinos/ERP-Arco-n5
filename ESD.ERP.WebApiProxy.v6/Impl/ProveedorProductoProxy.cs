using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ProveedorProductoProxy : WebApiProxyClient, IProveedorProductoProxy
	{
		private const string API_CONTROLLER = "ProveedorProducto";
		public ProveedorProductoDTO GetById(ProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<ProveedorProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ProveedorProductoDTO> Search(ProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<List<ProveedorProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ProveedorProductoDTO SingleSearch(ProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<ProveedorProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ProveedorProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ProveedorProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
