using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ComprobanteProductoEntregaProxy : WebApiProxyClient, IComprobanteProductoEntregaProxy
	{
		private const string API_CONTROLLER = "ComprobanteProductoEntrega";
		public ComprobanteProductoEntregaDTO GetById(ComprobanteProductoEntregaDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteProductoEntregaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ComprobanteProductoEntregaDTO> Search(ComprobanteProductoEntregaDTO dto)
		{
			var result = RestClient.PostApi<List<ComprobanteProductoEntregaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ComprobanteProductoEntregaDTO SingleSearch(ComprobanteProductoEntregaDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteProductoEntregaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ComprobanteProductoEntregaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ComprobanteProductoEntregaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ComprobanteProductoEntregaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
