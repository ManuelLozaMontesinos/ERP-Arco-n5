using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class NotaCreditoProductoProxy : WebApiProxyClient, INotaCreditoProductoProxy
	{
		private const string API_CONTROLLER = "NotaCreditoProducto";
		public NotaCreditoProductoDTO GetById(NotaCreditoProductoDTO dto)
		{
			var result = RestClient.PostApi<NotaCreditoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<NotaCreditoProductoDTO> Search(NotaCreditoProductoDTO dto)
		{
			var result = RestClient.PostApi<List<NotaCreditoProductoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public NotaCreditoProductoDTO SingleSearch(NotaCreditoProductoDTO dto)
		{
			var result = RestClient.PostApi<NotaCreditoProductoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(NotaCreditoProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(NotaCreditoProductoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(NotaCreditoProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
		public void AgregarComprobanteProducto(NotaCreditoProductoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, "AgregarComprobanteProducto"), dto);
		}
	}
}
