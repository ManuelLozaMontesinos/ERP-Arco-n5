using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class OrdenProduccionProductoEntregaProxy : WebApiProxyClient, IOrdenProduccionProductoEntregaProxy
	{
		private const string API_CONTROLLER = "OrdenProduccionProductoEntrega";
		public OrdenProduccionProductoEntregaDTO GetById(OrdenProduccionProductoEntregaDTO dto)
		{
			var result = RestClient.PostApi<OrdenProduccionProductoEntregaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<OrdenProduccionProductoEntregaDTO> Search(OrdenProduccionProductoEntregaDTO dto)
		{
			var result = RestClient.PostApi<List<OrdenProduccionProductoEntregaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public OrdenProduccionProductoEntregaDTO SingleSearch(OrdenProduccionProductoEntregaDTO dto)
		{
			var result = RestClient.PostApi<OrdenProduccionProductoEntregaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(OrdenProduccionProductoEntregaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(OrdenProduccionProductoEntregaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(OrdenProduccionProductoEntregaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
