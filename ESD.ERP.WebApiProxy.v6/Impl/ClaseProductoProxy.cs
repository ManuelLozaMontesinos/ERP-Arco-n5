using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ClaseProductoProxy : WebApiProxyClient, IClaseProductoProxy
	{
		private const string API_CONTROLLER = "ClaseProducto";
		public ClaseProductoDTO GetById(ClaseProductoDTO dto)
		{
			var result = RestClient.PostApi<ClaseProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ClaseProductoDTO> Search(ClaseProductoDTO dto)
		{
			var result = RestClient.PostApi<List<ClaseProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ClaseProductoDTO SingleSearch(ClaseProductoDTO dto)
		{
			var result = RestClient.PostApi<ClaseProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ClaseProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ClaseProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ClaseProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
