using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class EstadoPedidoProxy : WebApiProxyClient, IEstadoPedidoProxy
	{
		private const string API_CONTROLLER = "EstadoPedido";
		public EstadoPedidoDTO GetById(EstadoPedidoDTO dto)
		{
			var result = RestClient.PostApi<EstadoPedidoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<EstadoPedidoDTO> Search(EstadoPedidoDTO dto)
		{
			var result = RestClient.PostApi<List<EstadoPedidoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public EstadoPedidoDTO SingleSearch(EstadoPedidoDTO dto)
		{
			var result = RestClient.PostApi<EstadoPedidoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(EstadoPedidoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(EstadoPedidoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(EstadoPedidoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
