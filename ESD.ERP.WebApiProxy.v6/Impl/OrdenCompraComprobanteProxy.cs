using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OrdenCompraComprobanteProxy : WebApiProxyClient, IOrdenCompraComprobanteProxy
	{
		private const string API_CONTROLLER = "OrdenCompraComprobante";
		public OrdenCompraComprobanteDTO GetById(OrdenCompraComprobanteDTO dto)
		{
			var result = RestClient.PostApi<OrdenCompraComprobanteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OrdenCompraComprobanteDTO> Search(OrdenCompraComprobanteDTO dto)
		{
			var result = RestClient.PostApi<List<OrdenCompraComprobanteDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OrdenCompraComprobanteDTO SingleSearch(OrdenCompraComprobanteDTO dto)
		{
			var result = RestClient.PostApi<OrdenCompraComprobanteDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OrdenCompraComprobanteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OrdenCompraComprobanteDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OrdenCompraComprobanteDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
