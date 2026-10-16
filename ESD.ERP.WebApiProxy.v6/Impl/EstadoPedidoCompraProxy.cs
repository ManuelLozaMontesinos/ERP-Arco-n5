using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoPedidoCompraProxy : WebApiProxyClient, IEstadoPedidoCompraProxy
	{
		private const string API_CONTROLLER = "EstadoPedidoCompra";
		public EstadoPedidoCompraDTO GetById(EstadoPedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<EstadoPedidoCompraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoPedidoCompraDTO> Search(EstadoPedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoPedidoCompraDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoPedidoCompraDTO SingleSearch(EstadoPedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<EstadoPedidoCompraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoPedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoPedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoPedidoCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public EstadoPedidoCompraDTO ObtenerPorCodigo(EstadoPedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<EstadoPedidoCompraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_CODE), dto);
			return result;
		}
	}
}
