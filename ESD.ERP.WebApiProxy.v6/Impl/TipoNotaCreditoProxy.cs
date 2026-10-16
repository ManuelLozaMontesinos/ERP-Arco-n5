using System.Collections.Generic;
using ESD.Core.Service.WebApi;
using ESD.ERP.DTO;

namespace ESD.ERP.WebApiProxy.Impl
{
	public class TipoNotaCreditoProxy : WebApiProxyClient, ITipoNotaCreditoProxy
	{
		private const string API_CONTROLLER = "TipoNotaCredito";
		public TipoNotaCreditoDTO GetById(TipoNotaCreditoDTO dto)
		{
			var result = RestClient.PostApi<TipoNotaCreditoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.GET_BY_ID), dto);
			return result;
		}
		public List<TipoNotaCreditoDTO> Search(TipoNotaCreditoDTO dto)
		{
			var result = RestClient.PostApi<List<TipoNotaCreditoDTO>>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SEARCH), dto);
			return result;
		}
		public TipoNotaCreditoDTO SingleSearch(TipoNotaCreditoDTO dto)
		{
			var result = RestClient.PostApi<TipoNotaCreditoDTO>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.SINGLE_SEARCH), dto);
			return result;
		}
		public int Insert(TipoNotaCreditoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.INSERT), dto);
			return result;
		}
		public int Update(TipoNotaCreditoDTO dto)
		{
			var result = RestClient.PostApi<int>(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.UPDATE), dto);
			return result;
		}
		public void Delete(TipoNotaCreditoDTO dto)
		{
			RestClient.PostApi(WebApiMethod.Create(API_CONTROLLER, MethodNameConstant.DELETE), dto);
		}
	}
}
