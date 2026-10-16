using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PedidoProductoProxy : WebApiProxyClient, IPedidoProductoProxy
	{
		private const string API_CONTROLLER = "PedidoProducto";
		public PedidoProductoDTO GetById(PedidoProductoDTO dto)
		{
			var result = RestClient.PostApi<PedidoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PedidoProductoDTO> Search(PedidoProductoDTO dto)
		{
			var result = RestClient.PostApi<List<PedidoProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PedidoProductoDTO SingleSearch(PedidoProductoDTO dto)
		{
			var result = RestClient.PostApi<PedidoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PedidoProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PedidoProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PedidoProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
