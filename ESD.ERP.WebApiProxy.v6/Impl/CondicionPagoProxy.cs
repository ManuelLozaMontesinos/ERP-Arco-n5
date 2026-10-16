using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class CondicionPagoProxy : WebApiProxyClient, ICondicionPagoProxy
	{
		private const string API_CONTROLLER = "CondicionPago";
		public CondicionPagoDTO GetById(CondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<CondicionPagoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<CondicionPagoDTO> Search(CondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<List<CondicionPagoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public CondicionPagoDTO SingleSearch(CondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<CondicionPagoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(CondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(CondicionPagoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(CondicionPagoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
