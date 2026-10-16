using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PedidoCompraProductoProxy : WebApiProxyClient, IPedidoCompraProductoProxy
	{
		private const string API_CONTROLLER = "PedidoCompraProducto";
		public PedidoCompraProductoDTO GetById(PedidoCompraProductoDTO dto)
		{
			var result = RestClient.PostApi<PedidoCompraProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PedidoCompraProductoDTO> Search(PedidoCompraProductoDTO dto)
		{
			var result = RestClient.PostApi<List<PedidoCompraProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PedidoCompraProductoDTO SingleSearch(PedidoCompraProductoDTO dto)
		{
			var result = RestClient.PostApi<PedidoCompraProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PedidoCompraProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PedidoCompraProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PedidoCompraProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
      public void ActualizarProducto(PedidoCompraProductoDTO dto)
      {
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "ActualizarProducto"), dto);
		}
   }
}