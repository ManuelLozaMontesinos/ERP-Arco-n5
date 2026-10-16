using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class PedidoCompraProxy : WebApiProxyClient, IPedidoCompraProxy
	{
		private const string API_CONTROLLER = "PedidoCompra";
		public PedidoCompraDTO GetById(PedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<PedidoCompraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<PedidoCompraDTO> Search(PedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<List<PedidoCompraDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public PedidoCompraDTO SingleSearch(PedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<PedidoCompraDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(PedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(PedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(PedidoCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public int AgregarDesdeStockMinimo(PedidoCompraDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, "AgregarDesdeStockMinimo"), dto);
			return result;
		}
		public void SolicitarAutorizacion(PedidoCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "SolicitarAutorizacion"), dto);
		}
		public void Aprobar(PedidoCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Aprobar"), dto);
		}
		public void Desaprobar(PedidoCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "Desaprobar"), dto);
		}
		public void SolicitarModificacion(PedidoCompraDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "SolicitarModificacion"), dto);
		}
	}
}