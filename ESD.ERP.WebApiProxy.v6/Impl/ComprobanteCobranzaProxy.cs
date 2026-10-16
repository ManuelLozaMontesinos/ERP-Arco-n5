using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class ComprobanteCobranzaProxy : WebApiProxyClient, IComprobanteCobranzaProxy
	{
		private const string API_CONTROLLER = "ComprobanteCobranza";
		public ComprobanteCobranzaDTO GetById(ComprobanteCobranzaDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteCobranzaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<ComprobanteCobranzaDTO> Search(ComprobanteCobranzaDTO dto)
		{
			var result = RestClient.PostApi<List<ComprobanteCobranzaDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public ComprobanteCobranzaDTO SingleSearch(ComprobanteCobranzaDTO dto)
		{
			var result = RestClient.PostApi<ComprobanteCobranzaDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(ComprobanteCobranzaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(ComprobanteCobranzaDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(ComprobanteCobranzaDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
